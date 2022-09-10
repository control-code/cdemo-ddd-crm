using Cdemo.AdaptersImpl;
using Cdemo.Identity.Entities;
using Cdemo.Identity.Services;
using Cdemo.Identity.ServicesImpl;

namespace Cdemo.Identity.UnitTests
{
	public class UserServiceTests
	{
		private readonly InMemoryRepository<User, UserState> _repo;
		private readonly FakeUserQueryAdapter _query;
		private readonly UserService _service;

		public UserServiceTests()
		{
			_repo = new InMemoryRepository<User, UserState>();
			_query = new FakeUserQueryAdapter(_repo);
			_service = new UserService(_repo, _query);
		}

		[Fact]
		public void RegisterFirstUserAsAdmin()
		{
			_service.Register("admin", "admin").Wait();
			Assert.Equal(1, _repo.States.Count);
			Assert.True(_repo.States.Single().Value.IsAdmin);
		}

		[Fact]
		public void RegisterNextUser()
		{
			_service.Register("admin", "admin").Wait();
			_service.Register("test", "test").Wait();
			Assert.True(_repo.States.Single(e => e.Value.Name == "admin").Value.IsAdmin);
			Assert.False(_repo.States.Single(e => e.Value.Name == "test").Value.IsAdmin);
		}

		[Fact]
		public void LoginUser()
		{
			_service.Register("admin", "admin").Wait();
			Assert.Equal(_repo.States.First().Key, _service.Login("admin", "admin").Result);
		}

		[Fact]
		public void LoginUserUnsuccessful()
		{
			_service.Register("admin", "admin").Wait();
			Assert.Equal(Guid.Empty, _service.Login("admin", "incorrect").Result);
		}

		[Fact]
		public void LoginUserNonexistent()
		{
			_service.Register("admin", "admin").Wait();
			Assert.Equal(Guid.Empty, _service.Login("test", "test").Result);
		}

		[Fact]
		public void SetAdminFlag()
		{
			_service.Register("admin", "admin").Wait();
			_service.Register("test", "test").Wait();
			Assert.True(_repo.States.Single(e => e.Value.Name == "admin").Value.IsAdmin);
			Assert.False(_repo.States.Single(e => e.Value.Name == "test").Value.IsAdmin);

			var adminId = _repo.States.Single(e => e.Value.Name == "admin").Key;
			var userId = _repo.States.Single(e => e.Value.Name == "test").Key;
			_service.SetAdminFlag(userId, adminId).Wait();
			Assert.True(_repo.States.Single(e => e.Value.Name == "test").Value.IsAdmin);
		}

		[Fact]
		public void ResetAdminFlag()
		{
			_service.Register("admin", "admin").Wait();
			_service.Register("test", "test").Wait();
			Assert.True(_repo.States.Single(e => e.Value.Name == "admin").Value.IsAdmin);
			Assert.False(_repo.States.Single(e => e.Value.Name == "test").Value.IsAdmin);

			var adminId = _repo.States.Single(e => e.Value.Name == "admin").Key;
			var userId = _repo.States.Single(e => e.Value.Name == "test").Key;
			_service.SetAdminFlag(userId, adminId).Wait();
			Assert.True(_repo.States.Single(e => e.Value.Name == "test").Value.IsAdmin);
			_service.ResetAdminFlag(userId, adminId).Wait();
			Assert.False(_repo.States.Single(e => e.Value.Name == "test").Value.IsAdmin);
		}

		[Fact]
		public void SetAdminFlagUnauthorized()
		{
			_service.Register("admin", "admin").Wait();
			_service.Register("test", "test").Wait();
			Assert.True(_repo.States.Single(e => e.Value.Name == "admin").Value.IsAdmin);
			Assert.False(_repo.States.Single(e => e.Value.Name == "test").Value.IsAdmin);

			var userId = _repo.States.Single(e => e.Value.Name == "test").Key;
			Assert.ThrowsAsync<UnauthorizedException>(() => _service.SetAdminFlag(userId, userId)).Wait();
		}

		[Fact]
		public void GetUser()
		{
			_service.Register("admin", "admin").Wait();
			_service.Register("test", "test").Wait();

			var adminId = _repo.States.Single(e => e.Value.Name == "admin").Key;
			var userId = _repo.States.Single(e => e.Value.Name == "test").Key;

			var user1 = _service.GetUser(userId, adminId).Result;
			Assert.Equal("test", user1.Name);

			var user2 = _service.GetUser(userId, userId).Result;
			Assert.Equal("test", user2.Name);

			var admin = _service.GetUser(adminId, adminId).Result;
			Assert.Equal("admin", admin.Name);
		}

		[Fact]
		public void GetUserUnauthorized()
		{
			_service.Register("admin", "admin").Wait();
			_service.Register("test", "test").Wait();

			var adminId = _repo.States.Single(e => e.Value.Name == "admin").Key;
			var userId = _repo.States.Single(e => e.Value.Name == "test").Key;
			Assert.ThrowsAsync<UnauthorizedException>(() => _service.GetUser(adminId, userId)).Wait();
		}
	}
}