using System;

namespace Cdemo.Staff.Service
{
	public interface IEmployeeService
	{
		Task AddEmployee(Guid userId, string firstName, string lastName, Guid initiatorId);
		Task<IEnumerable<EmployeeData>> GetEmployees(Guid initiatorId);
	}
}
