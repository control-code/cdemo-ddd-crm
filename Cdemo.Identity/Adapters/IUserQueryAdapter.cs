using System;

namespace Cdemo.Identity.Adapters
{
	public interface IUserQueryAdapter
	{
		Task<UserRecord?> FindByName(string name);
		Task<int> GetUsersCount();
	}
}
