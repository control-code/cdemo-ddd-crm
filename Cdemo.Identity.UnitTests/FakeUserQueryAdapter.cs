using Cdemo.AdaptersImpl;
using Cdemo.Identity.Adapters;
using Cdemo.Identity.Entities;
using Cdemo.Identity.Services;

namespace Cdemo.Identity.UnitTests
{
    public class FakeUserQueryAdapter : IUserQueryAdapter
	{
		private readonly InMemoryRepository<User, UserState> _repo;

		public FakeUserQueryAdapter(InMemoryRepository<User, UserState> repo)
		{
			_repo = repo;
		}

		public Task<UserRecord?> FindByName(string name)
		{
			var e = _repo.States.FirstOrDefault(e => e.Value.Name == name);
			if (e.Key != Guid.Empty)
			{
				var rec = new UserRecord(e.Key, e.Value.Name, e.Value.PassHash, e.Value.IsAdmin);
				return Task.FromResult((UserRecord?)rec);
			}
			return Task.FromResult((UserRecord?)null);
		}

		public Task<IEnumerable<ShortUserRecord>> GetAllUsers()
		{
			return Task.FromResult(_repo.States.Select(e => new ShortUserRecord(e.Key, e.Value.Name, e.Value.IsAdmin)));
		}

		public Task<int> GetUsersCount()
		{
			return Task.FromResult(_repo.States.Count);
		}
	}
}