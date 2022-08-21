using Cdemo.AdaptersImpl;
using Cdemo.Identity.Adapters;
using Cdemo.Identity.Entities;

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
			var e = _repo.Entities.FirstOrDefault(e => e.State.Name == name);
			if (e != null)
			{
				var rec = new UserRecord(e.Id, e.State.Name, e.State.PassHash, e.State.IsAdmin);
				return Task.FromResult((UserRecord?)rec);
			}
			return Task.FromResult((UserRecord?)null);
		}

		public Task<int> GetUsersCount()
		{
			return Task.FromResult(_repo.Entities.Count);
		}
	}
}