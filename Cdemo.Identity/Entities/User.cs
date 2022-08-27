using Cdemo.Entities;

namespace Cdemo.Identity.Entities
{
	public class User : Entity<UserState>
	{
		public bool IsAdmin => State.IsAdmin;

		public User(Guid id, UserState state) 
			: base(id, state)
		{ }

		public User(Guid id, string name, string pass, bool isAdmin)
			: base(id, new UserState(name, pass, isAdmin))
		{ }

		public void SetAdmin()
		{
			State = State with { IsAdmin = true };
		}

		public void ResetAdmin()
		{
			State = State with { IsAdmin = false };
		}
	}
}
