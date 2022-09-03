using Cdemo.Adapters;
using Cdemo.Customers.Adapters;
using Cdemo.Customers.Entities;
using Cdemo.Customers.Services;

namespace Cdemo.Customers.ServicesImpl
{
	public class CustomerService : ICustomerService
	{
		private readonly IRepository<Customer, CustomerState> _customerRepo;
		private readonly ICustomerQueryAdapter _query;

		public CustomerService(IRepository<Customer, CustomerState> customerRepo, ICustomerQueryAdapter query)
		{
			_customerRepo = customerRepo;
			_query = query;
		}

		public Task RegisterNewCustomer(DateTime now, string firstName, string lastName, string phone, string email, Guid initiatorId)
		{
			var id = Guid.NewGuid();
			var customer = new Customer(id, now, initiatorId, firstName, lastName, phone, email);
			return _customerRepo.Add(customer);
		}

		public Task<IEnumerable<CustomerData>> GetCustomers(Guid initiatorId)
		{
			return _query.GetCustomers(initiatorId);
		}

		public Task<CustomerExtData?> GetCustomer(Guid custormerId, Guid initiatorId)
		{
			return _query.GetCustomer(custormerId);
		}
	}
}
