﻿using Cdemo.Adapters;
using Cdemo.Identity.Adapters;
using Cdemo.Identity.Entities;
using Cdemo.Identity.Services;
using System.Security.Cryptography;
using System.Text;

namespace Cdemo.Identity.ServicesImpl
{
	public class UserService : IUserService
	{
		private readonly IUserQueryAdapter _query;
		private readonly IRepository<User, UserState> _repo;

		public UserService(IRepository<User, UserState> repo, IUserQueryAdapter query)
		{
			_repo = repo;
			_query = query;
		}

		public async Task<Guid> Login(string name, string pass)
		{
			var item = await _query.FindByName(name);
			
			if (item != null)
			{
				var hash = ComputeHash(item.Id, pass);

				if (hash == item.PassHash)
				{
					return item.Id;
				}
			}
			
			return Guid.Empty;
		}

		public async Task Register(string name, string pass)
		{
			var item = await _query.FindByName(name);

			if (item != null)
			{
				throw new NameAlreadyTakenException();
			}

			var usersCount = await _query.GetUsersCount();

			var id = Guid.NewGuid();
			var hash = ComputeHash(id, pass);
			var user = new User(id, name, hash, usersCount == 0); // make first user admin

			await _repo.Add(user);
		}

		private static string ComputeHash(Guid id, string pass)
		{
			using (var sha = SHA256.Create())
			{
				var text = pass + id;
				return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(text)));
			}
		}
	}
}