using Cdemo.Adapters;
using Cdemo.Customers.Adapters;
using Cdemo.Customers.Entities;
using Cdemo.Customers.Services;

namespace Cdemo.Customers.ServicesImpl
{
	public class CustomerService : ICustomerService
	{
		private readonly IRepository<Customer, CustomerState> _customerRepo;
		private readonly IRepository<CustomerNote, CustomerNoteState> _customerNoteRepo;
		private readonly ICustomerQueryAdapter _query;

		public CustomerService(IRepository<Customer, CustomerState> customerRepo, 
			IRepository<CustomerNote, CustomerNoteState> customerNoteRepo, 
			ICustomerQueryAdapter query)
		{
			_customerRepo = customerRepo;
			_query = query;
			_customerNoteRepo = customerNoteRepo;
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

		public Task AddCustomerNote(Guid custormerId, DateTime now, string text, Guid initiatorId)
		{
			var id = Guid.NewGuid();
			var note = new CustomerNote(id, initiatorId, custormerId, now, text);
			return _customerNoteRepo.Add(note);
		}

		public Task<IEnumerable<CustomerNoteData>> GetCustomerNotes(Guid custormerId, Guid initiatorId)
		{
			return _query.GetCustomerNotes(custormerId);
		}
	}
}
