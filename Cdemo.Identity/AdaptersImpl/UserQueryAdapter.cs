using System.Data.SqlClient;
using Dapper;
using Cdemo.Identity.Adapters;
using System.Xml.Linq;

namespace Cdemo.Identity.AdaptersImpl
{
	public class UserQueryAdapter : IUserQueryAdapter
	{
		private readonly string _connectionStr;

		public UserQueryAdapter(string connectionStr)
		{
			_connectionStr = connectionStr;
		}

		public async Task<UserRecord?> FindByName(string name)
		{
			var q = "SELECT [Id], [Name], [PassHash], [IsAdmin] FROM [UserStates] WHERE [Name] = @name";

			using var connection = new SqlConnection(_connectionStr);
			var rec = await connection.QuerySingleOrDefaultAsync<UserRecord>(q, new { name });
			return rec;
		}

		public async Task<int> GetUsersCount()
		{
			var q = "SELECT COUNT(*) FROM [UserStates]";

			using var connection = new SqlConnection(_connectionStr);
			var count = await connection.ExecuteScalarAsync<int>(q);
			return count;
		}
	}
}
