using System.Security.Cryptography;
using System.Text;
using Cdemo.Adapters;
using Cdemo.Identity.Adapters;
using Cdemo.Identity.Entities;
using Cdemo.Identity.Services;

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

		public async Task SetAdminFlag(Guid userId, Guid initiatorId)
		{
			await CheckAdminAccess(initiatorId);

			var user = await _repo.Get(userId);
			if (user != null)
			{
				user.SetAdmin();
				await _repo.Update(user);
			}
			else
			{
				throw new UserNotFoundException();
			}
		}

		public async Task ResetAdminFlag(Guid userId, Guid initiatorId)
		{
			await CheckAdminAccess(initiatorId);

			var user = await _repo.Get(userId);
			if (user != null)
			{
				user.ResetAdmin();
				await _repo.Update(user);
			}
			else
			{
				throw new UserNotFoundException();
			}
		}

		public async Task<IEnumerable<ShortUserRecord>> GetAllUsers(Guid initiatorId)
		{
			await CheckAdminAccess(initiatorId);
			return await _query.GetAllUsers();
		}

		public async Task<ShortUserRecord> GetUser(Guid userId, Guid initiatorId)
		{
			if (userId != initiatorId)
			{
				await CheckAdminAccess(initiatorId);
			}

			var user = await _repo.Get(userId);

			if (user != null)
			{
				return new ShortUserRecord(user.Id, user.State.Name, user.IsAdmin);
			}
			else
			{
				throw new UserNotFoundException();
			}
		}

		private static string ComputeHash(Guid id, string pass)
		{
			using (var sha = SHA256.Create())
			{
				var text = pass + id;
				return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(text)));
			}
		}

		private async Task CheckAdminAccess(Guid initiatorId)
		{
			var initiator = await _repo.Get(initiatorId);
			if (initiator == null || !initiator.IsAdmin)
			{
				throw new UnauthorizedException();
			}
		}
	}
}
