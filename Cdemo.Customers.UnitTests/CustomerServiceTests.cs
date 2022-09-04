using Cdemo.AdaptersImpl;
using Cdemo.Customers.Entities;
using Cdemo.Customers.ServicesImpl;
using Cdemo.Identity.Entities;
using Cdemo.Identity.ServicesImpl;
using Cdemo.Identity.UnitTests;

namespace Cdemo.Customers.UnitTests
{
	public class CustomerServiceTests
	{
		private readonly InMemoryRepository<User, UserState> _userRepo;
		private readonly UserService _userService;
		private readonly InMemoryRepository<Customer, CustomerState> _repo;
		private readonly FakeCustomerQueryAdapter _query;
		private readonly CustomerService _service;

		public CustomerServiceTests()
		{
			_userRepo = new InMemoryRepository<User, UserState>();
			var userQuery = new FakeUserQueryAdapter(_userRepo);
			_userService = new UserService(_userRepo, userQuery);

			_repo = new InMemoryRepository<Customer, CustomerState>();
			var notesRepo = new InMemoryRepository<CustomerNote, CustomerNoteState>();
			_query = new FakeCustomerQueryAdapter(_repo);
			_service = new CustomerService(_repo, notesRepo, _query);
		}

		[Fact]
		public void RegisterNewCustomer()
		{
			_userService.Register("admin", "admin").Wait();
			var userId = _userRepo.Entities.Single(e => e.State.Name == "admin").Id;

			var now = DateTime.Parse("2022-08-01T20:20:00Z");
			_service.RegisterNewCustomer(now, "Test", "Customer", "1234567890", "email@test.com", userId).Wait();
			var customer = _repo.Entities.Single();
			Assert.Equal("Test", customer.State.FirstName);
			Assert.Equal("Customer", customer.State.LastName);
			Assert.Equal("1234567890", customer.State.Phone);
			Assert.Equal("email@test.com", customer.State.Email);
			Assert.Equal(now, customer.State.RegistrationDateTime);
			Assert.Equal(userId, customer.State.ResponsibleUserId);
		}

		[Fact]
		public void GetCustomers()
		{
			_userService.Register("admin", "admin").Wait();
			var userId = _userRepo.Entities.Single(e => e.State.Name == "admin").Id;

			var now = DateTime.Parse("2022-08-01T20:20:00Z");
			_service.RegisterNewCustomer(now, "Test1", "Customer1", "12345678901", "email1@test.com", userId).Wait();
			_service.RegisterNewCustomer(now, "Test2", "Customer2", "12345678902", "email2@test.com", userId).Wait();
			_service.RegisterNewCustomer(now, "Test3", "Customer3", "12345678903", "email3@test.com", userId).Wait();

			var list = _service.GetCustomers(userId).Result;
			Assert.Equal(3, list.Count());
		}
	}
}