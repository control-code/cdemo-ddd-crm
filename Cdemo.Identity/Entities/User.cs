using Cdemo.Entities;

namespace Cdemo.Identity.Entities
{
	public class User : Entity<UserState>
	{
		public User(Guid id, UserState state) 
			: base(id, state)
		{ }

		public User(Guid id, string name, string pass, bool isAdmin)
			: base(id, new UserState(name, pass, isAdmin))
		{ }
	}
}
