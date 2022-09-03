using System;

namespace Cdemo.Customers.Services
{
	public interface ICustomerService
	{
		Task RegisterNewCustomer(DateTime now, string firstName, string lastName, string phone, string email, Guid initiatorId);
		Task<IEnumerable<CustomerData>> GetCustomers(Guid initiatorId);
		Task<CustomerExtData> GetCustomerExt(Guid custormerId, Guid initiatorId);
	}
}
