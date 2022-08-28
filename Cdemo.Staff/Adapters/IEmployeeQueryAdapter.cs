using System;
using Cdemo.Staff.Service;

namespace Cdemo.Staff.Adapters
{
	public interface IEmployeeQueryAdapter
	{
		Task<IEnumerable<EmployeeData>> GetEmployees();
	}
}
