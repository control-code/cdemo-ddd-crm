using Cdemo.Entities;

namespace Cdemo.Customers.Entities
{
	public class Customer: Entity<CustomerState>
	{
		public Customer(Guid id, DateTime registrationDateTime, Guid responsibleUserId, 
			string firstName, string lastName, string phone, string email)
			: base(id, new CustomerState(registrationDateTime, responsibleUserId, firstName, lastName, phone, email))
		{ }

		public Customer(Guid id, CustomerState state)
			: base(id, state)
		{ }
	}
}
