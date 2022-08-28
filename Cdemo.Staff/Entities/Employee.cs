using Cdemo.Entities;

namespace Cdemo.Staff.Entities
{
    public class Employee: Entity<EmployeeState>
    {
        public Employee(Guid id, Guid userId, string firstName, string lastName)
            : base(id, new EmployeeState(userId, firstName, lastName))
        { }
		public Employee(Guid id, EmployeeState state)
			: base(id, state)
		{ }
	}
}
