using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using Cdemo.Staff.Service;
using Cdemo.Staff.Adapters;


namespace Cdemo.Staff.AdaptersImpl
{
	public class EmployeeQueryAdapter : IEmployeeQueryAdapter
	{
		private readonly string _connectionStr;

		public EmployeeQueryAdapter(IConfiguration configuration)
			: this(configuration.GetConnectionString(typeof(EmployeeQueryAdapter).Name) ??
			       configuration.GetConnectionString("DefaultMsSql"))
		{ }

		public EmployeeQueryAdapter(string connectionStr)
		{
			_connectionStr = connectionStr;
		}

		public async Task<IEnumerable<EmployeeData>> GetEmployees()
		{
			var q = "SELECT [Id], [UserId], [FirstName], [LastName] FROM [EmployeeStates]";

			using var connection = new SqlConnection(_connectionStr);
			var records = await connection.QueryAsync<EmployeeData>(q);
			return records;
		}
	}
}
