using Cdemo.Identity.Services;

namespace Cdemo.Identity.Adapters
{
    public interface IUserQueryAdapter
	{
		Task<IEnumerable<ShortUserRecord>> GetAllUsers();
		Task<UserRecord?> FindByName(string name);
		Task<int> GetUsersCount();
	}
}
