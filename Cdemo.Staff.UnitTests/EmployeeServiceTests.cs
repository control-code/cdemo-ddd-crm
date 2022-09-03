using Cdemo.AdaptersImpl;
using Cdemo.Identity.Entities;
using Cdemo.Identity.Services;
using Cdemo.Identity.ServicesImpl;
using Cdemo.Identity.UnitTests;
using Cdemo.Staff.Entities;
using Cdemo.Staff.ServiceImpl;

namespace Cdemo.Staff.UnitTests
{
	public class EmployeeServiceTests
	{
		private readonly InMemoryRepository<User, UserState> _userRepo;
		private readonly UserService _userService;
		private readonly InMemoryRepository<Employee, EmployeeState> _repo;
		private readonly FakeEmplyeeQueryAdapter _query;
		private readonly EmployeeService _service;

		public EmployeeServiceTests()
		{
			_userRepo = new InMemoryRepository<User, UserState>();
			var userQuery = new FakeUserQueryAdapter(_userRepo);
			_userService = new UserService(_userRepo, userQuery);

			_repo = new InMemoryRepository<Employee, EmployeeState>();
			_query = new FakeEmplyeeQueryAdapter(_repo);
			_service = new EmployeeService(_userService, _repo, _query);
		}

		[Fact]
		public void AddEmployee()
		{
			_userService.Register("admin", "admin").Wait();
			var userId = _userRepo.Entities.Single(e => e.State.Name == "admin").Id;

			_service.AddEmployee(userId, "Test", "Employee", userId).Wait();
			var employee = _repo.Entities.Single();
			Assert.Equal("Test", employee.State.FirstName);
			Assert.Equal("Employee", employee.State.LastName);
			Assert.Equal(userId, employee.State.UserId);
		}

		[Fact]
		public void AddEmployeeUnauthorized()
		{
			_userService.Register("admin", "admin").Wait();
			_userService.Register("test", "test").Wait();
			var userId = _userRepo.Entities.Single(e => e.State.Name == "test").Id;

			Assert.ThrowsAsync<UnauthorizedException>(() => _service.AddEmployee(userId, "Test", "Employee", userId)).Wait();
		}
	}
}