using Cdemo.Adapters;
using Cdemo.Identity.Services;
using Cdemo.Staff.Adapters;
using Cdemo.Staff.Entities;
using Cdemo.Staff.Service;

namespace Cdemo.Staff.ServiceImpl
{
	public class EmployeeService : IEmployeeService
	{
		private readonly IUserService _userService;
		private readonly IRepository<Employee, EmployeeState> _repo;
		private readonly IEmployeeQueryAdapter _query;

		public EmployeeService(IUserService userService, IRepository<Employee, EmployeeState> repo, IEmployeeQueryAdapter query)
		{
			_userService = userService;
			_repo = repo;
			_query = query;
		}

		public async Task AddEmployee(Guid userId, string firstName, string lastName, Guid initiatorId)
		{
			await CheckAdminAccess(initiatorId);
			await _userService.GetUser(userId, initiatorId); // check if user exists
			var id = Guid.NewGuid();
			var newEployee = new Employee(id, userId, firstName, lastName);
			await _repo.Add(newEployee);
		}

		public async Task<IEnumerable<EmployeeData>> GetEmployees(Guid initiatorId)
		{
			await CheckAdminAccess(initiatorId);
			return await _query.GetEmployees();
		}

		private async Task CheckAdminAccess(Guid initiatorId)
		{
			var initiator = await _userService.GetUser(initiatorId, initiatorId);
			if (initiator == null || !initiator.IsAdmin)
			{
				throw new UnauthorizedException();
			}
		}
	}
}
