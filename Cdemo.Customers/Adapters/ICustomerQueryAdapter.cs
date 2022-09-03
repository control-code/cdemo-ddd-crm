using Cdemo.Customers.Services;

namespace Cdemo.Customers.Adapters
{
	public interface ICustomerQueryAdapter
	{
		Task<IEnumerable<CustomerData>> GetCustomers(Guid responsibleUserId);
		Task<CustomerExtData?> GetCustomer(Guid custormerId);
	}
}
