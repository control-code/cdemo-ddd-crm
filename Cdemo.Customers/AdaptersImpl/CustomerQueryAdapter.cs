using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using Cdemo.Customers.Adapters;
using Cdemo.Customers.Services;

namespace Cdemo.Customers.AdaptersImpl
{
	public class CustomerQueryAdapter : ICustomerQueryAdapter
	{
		private readonly string _connectionStr;

		public CustomerQueryAdapter(IConfiguration configuration)
			: this(configuration.GetConnectionString(typeof(CustomerQueryAdapter).Name) ??
			       configuration.GetConnectionString("DefaultMsSql"))
		{ }

		public CustomerQueryAdapter(string connectionStr)
		{
			_connectionStr = connectionStr;
		}

		public async Task<CustomerExtData?> GetCustomer(Guid custormerId)
		{
			var q = "SELECT c.[Id], c.[RegistrationDateTime], c.[FirstName], c.[LastName], " +
					"c.[Phone], c.[Email], COUNT(n.Id) NotesCount " +
					"FROM [CustomerStates] c " +
					"LEFT JOIN [CustomerNoteStates] n ON n.[CustomerId] = c.[Id] " +
					"WHERE c.[Id] = @custormerId " +
					"GROUP BY c.[Id], c.[RegistrationDateTime], c.[FirstName], c.[LastName], c.[Phone], c.[Email]";

			using var connection = new SqlConnection(_connectionStr);
			var record = await connection.QuerySingleAsync<CustomerExtData>(q, new { custormerId });
			return record;
		}

		public async Task<IEnumerable<CustomerData>> GetCustomers(Guid responsibleUserId)
		{
			var q = "SELECT c.[Id], c.[FirstName], c.[LastName], " +
					"e.[FirstName] ResponsibleUserFirstName, e.[LastName] ResponsibleUserLastName " + 
			        "FROM [CustomerStates] c " +
					"LEFT JOIN [EmployeeStates] e ON e.[UserId] = c.[ResponsibleUserId] " +
					"WHERE c.[ResponsibleUserId] = @responsibleUserId";

			using var connection = new SqlConnection(_connectionStr);
			var records = await connection.QueryAsync<CustomerData>(q, new { responsibleUserId });
			return records;
		}
	}
}
