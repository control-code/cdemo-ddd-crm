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
			_service.Register("admin", "test").Wait();
			Assert.Equal(1, _repo.Entities.Count);
			Assert.True(_repo.Entities.First().State.IsAdmin);
		}

		[Fact]
		public void RegisterNextUser()
		{
			_service.Register("admin", "test").Wait();
			_service.Register("test", "test").Wait();
			Assert.True(_repo.Entities.Single(e => e.State.Name == "admin").State.IsAdmin);
			Assert.False(_repo.Entities.Single(e => e.State.Name == "test").State.IsAdmin);
		}

		[Fact]
		public void LoginUser()
		{
			_service.Register("admin", "test").Wait();
			Assert.Equal(_repo.Entities.First().Id, _service.Login("admin", "test").Result);
		}

		[Fact]
		public void LoginUserUnsuccessful()
		{
			_service.Register("admin", "test").Wait();
			Assert.Equal(Guid.Empty, _service.Login("admin", "incorrect").Result);
		}

		[Fact]
		public void LoginUserNonexistent()
		{
			_service.Register("admin", "test").Wait();
			Assert.Equal(Guid.Empty, _service.Login("test", "test").Result);
		}

		[Fact]
		public void SetAdminFlag()
		{
			_service.Register("admin", "test").Wait();
			_service.Register("test", "test").Wait();
			Assert.True(_repo.Entities.Single(e => e.State.Name == "admin").State.IsAdmin);
			Assert.False(_repo.Entities.Single(e => e.State.Name == "test").State.IsAdmin);

			var adminId = _repo.Entities.Single(e => e.State.Name == "admin").Id;
			var userId = _repo.Entities.Single(e => e.State.Name == "test").Id;
			_service.SetAdminFlag(userId, adminId).Wait();
			Assert.True(_repo.Entities.Single(e => e.State.Name == "test").State.IsAdmin);
		}

		[Fact]
		public void ResetAdminFlag()
		{
			_service.Register("admin", "test").Wait();
			_service.Register("test", "test").Wait();
			Assert.True(_repo.Entities.Single(e => e.State.Name == "admin").State.IsAdmin);
			Assert.False(_repo.Entities.Single(e => e.State.Name == "test").State.IsAdmin);

			var adminId = _repo.Entities.Single(e => e.State.Name == "admin").Id;
			var userId = _repo.Entities.Single(e => e.State.Name == "test").Id;
			_service.SetAdminFlag(userId, adminId).Wait();
			Assert.True(_repo.Entities.Single(e => e.State.Name == "test").State.IsAdmin);
			_service.ResetAdminFlag(userId, adminId).Wait();
			Assert.False(_repo.Entities.Single(e => e.State.Name == "test").State.IsAdmin);
		}

		[Fact]
		public void SetAdminFlagUnauthorized()
		{
			_service.Register("admin", "test").Wait();
			_service.Register("test", "test").Wait();
			Assert.True(_repo.Entities.Single(e => e.State.Name == "admin").State.IsAdmin);
			Assert.False(_repo.Entities.Single(e => e.State.Name == "test").State.IsAdmin);

			var userId = _repo.Entities.Single(e => e.State.Name == "test").Id;
			Assert.ThrowsAsync<UnauthorizedException>(() => _service.SetAdminFlag(userId, userId)).Wait();
		}
	}
}