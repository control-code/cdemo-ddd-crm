using Cdemo.Entities;

namespace Cdemo.Customers.Entities
{
	public class CustomerNote: Entity<CustomerNoteState>
	{
		public CustomerNote(Guid id, Guid authorUserId, Guid customerId, DateTime dateTime, string text)
			: base(id, new CustomerNoteState(authorUserId, customerId, dateTime, text))
		{ }

		public CustomerNote(Guid id, CustomerNoteState state) : base(id, state)
		{ }
	}
}
