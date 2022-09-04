using System;

namespace Cdemo.Customers.Services
{
	public interface ICustomerService
	{
		Task RegisterNewCustomer(DateTime now, string firstName, string lastName, string phone, string email, Guid initiatorId);
		Task<IEnumerable<CustomerData>> GetCustomers(Guid initiatorId);
		Task<CustomerExtData?> GetCustomer(Guid custormerId, Guid initiatorId);

		Task AddCustomerNote(Guid custormerId, DateTime now, string text, Guid initiatorId);
		Task<IEnumerable<CustomerNoteData>> GetCustomerNotes(Guid custormerId, Guid initiatorId);
	}
}
