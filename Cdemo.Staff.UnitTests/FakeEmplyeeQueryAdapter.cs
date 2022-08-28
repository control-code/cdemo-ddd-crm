using Cdemo.AdaptersImpl;
using Cdemo.Staff.Adapters;
using Cdemo.Staff.Entities;
using Cdemo.Staff.Service;

namespace Cdemo.Staff.UnitTests
{
	public class FakeEmplyeeQueryAdapter : IEmployeeQueryAdapter
	{
		private readonly InMemoryRepository<Employee, EmployeeState> _repo;

		public FakeEmplyeeQueryAdapter(InMemoryRepository<Employee, EmployeeState> repo)
		{
			_repo = repo;
		}

		public Task<IEnumerable<EmployeeData>> GetEmployees()
		{
			return Task.FromResult(
				_repo.Entities.Select(
					e => new EmployeeData(e.Id, e.State.UserId, e.State.FirstName, e.State.LastName)));
		}
	}
}