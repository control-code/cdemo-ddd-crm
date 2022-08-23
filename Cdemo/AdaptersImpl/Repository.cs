﻿using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Dynamic;
using Microsoft.Extensions.Configuration;
using Dapper;
using Cdemo.Entities;
using Cdemo.Adapters;

namespace Cdemo.AdaptersImpl
{
	public class Repository<T, TState>: IRepository<T, TState> where T: Entity<TState>
	{
		private readonly string _connectionStr;
		private readonly string _tableName;
		private readonly string _columnList;
		private readonly string _valueList;

		public Repository(IConfiguration configuration)
			: this(configuration.GetConnectionString(typeof(T).Name + "Repository") ??
			       configuration.GetConnectionString("DefaultMsSql"))
		{ }

		public Repository(string connectionStr)
		{
			_connectionStr = connectionStr;
			_tableName = GetTableName();
			_columnList = GetColumnList();
			_valueList = GetValueList();
		}

		public async Task<T?> Get(Guid id)
		{
			using var connection = new SqlConnection(_connectionStr);
			var state = await connection.QuerySingleOrDefaultAsync<TState>($"SELECT * FROM {_tableName} WHERE [Id] = @id");

			if (state == null)
			{
				return null;
			}
			
			ConstructorInfo? ctor = typeof(T).GetConstructor(new[] { typeof(Guid), typeof(TState) });
			object? instance = ctor?.Invoke(new object[] { id, state });

			return (T?)instance;
		}

		public async Task Add(T entity)
		{
			var cmd = $"INSERT INTO {_tableName} ({_columnList}) VALUES ({_valueList})";

			var parameters = ConvertToFlatObject(entity);

			using var connection = new SqlConnection(_connectionStr);
			await connection.ExecuteAsync(cmd, parameters);
		}

		private string GetValueList()
		{
			var list = new StringBuilder("@Id");
			var properties = typeof(TState).GetProperties();
			foreach (var p in properties)
			{
				list.Append(", @");
				list.Append(p.Name);
			}

			return list.ToString();
		}

		private string GetColumnList()
		{
			var list = new StringBuilder("[Id]");
			var properties = typeof(TState).GetProperties();
			foreach (var p in properties)
			{
				list.Append(", [");
				list.Append(p.Name);
				list.Append("]");
			}

			return list.ToString();
		}

		private string GetTableName()
		{
			return typeof(TState).Name + "s";
		}

		private ExpandoObject ConvertToFlatObject(T entity)
		{
			if (entity == null || entity.State == null)
			{
				throw new NullReferenceException("Null entity or entity.State");
			}

			var flatObject = new ExpandoObject();
			var dictionary = flatObject as IDictionary<string, object>;

			dictionary.Add("Id", entity.Id);
			foreach (var property in entity.State.GetType().GetProperties())
			{
				var value = property.GetValue(entity.State);
				if (value != null)
				{
					dictionary.Add(property.Name, value);
				}
			}

			return flatObject;
		}
	}
}