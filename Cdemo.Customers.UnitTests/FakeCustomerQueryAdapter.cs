using Cdemo.AdaptersImpl;
using Cdemo.Customers.Adapters;
using Cdemo.Customers.Entities;
using Cdemo.Customers.Services;

namespace Cdemo.Customers.UnitTests
{
	public class FakeCustomerQueryAdapter : ICustomerQueryAdapter
	{
		private readonly InMemoryRepository<Customer, CustomerState> _repo;

		public FakeCustomerQueryAdapter(InMemoryRepository<Customer, CustomerState> repo)
		{
			_repo = repo;
		}

		public Task<IEnumerable<CustomerData>> GetCustomers(Guid responsibleUserId)
		{
			return Task.FromResult(
				_repo.States.Select(
					e => new CustomerData(e.Key, e.Value.FirstName, e.Value.LastName, "", "")));
		}

		public Task<CustomerExtData?> GetCustomer(Guid custormerId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<CustomerNoteData>> GetCustomerNotes(Guid custormerId)
		{
			throw new NotImplementedException();
		}
	}
}