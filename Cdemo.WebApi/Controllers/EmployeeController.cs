using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cdemo.Staff.Service;
using Cdemo.Identity.Services;

namespace Cdemo.WebApi.Controllers
{
	/// <summary>
	/// Employees management
	/// </summary>
	[ApiController]
	[Route("api/v1/employees")]
	public class EmployeeController : ApiControllerBase
	{
		private readonly IEmployeeService _service;
		private readonly ILogger<EmployeeController> _logger;

		public EmployeeController(IEmployeeService service, ILogger<EmployeeController> logger)
		{
			_service = service;
			_logger = logger;
		}

		/// <summary>
		/// Add new employee
		/// </summary>
		/// <param name="userId">Registered user account Id to bind</param>
		/// <param name="firstName">Employee first name</param>
		/// <param name="lastName">Employee last name</param>
		/// <returns>Success</returns>
		/// <response code="200">Successful</response>
		/// <response code="401">Only admin can perform this operation</response>
		/// <response code="404">User not found</response>
		[Authorize]
		[HttpPost]
		[Route("actions/add")]
		public async Task<IActionResult> PostAddEmployee(Guid userId, string firstName, string lastName)
		{
			try
			{
				await _service.AddEmployee(userId, firstName, lastName, GetInitiatorId());
				return Ok();
			}
			catch (UnauthorizedException)
			{
				return StatusCode(StatusCodes.Status401Unauthorized);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error");
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Get all employee list
		/// </summary>
		/// <returns>Lits of employees</returns>
		/// <response code="200">Successful</response>
		/// <response code="401">Only admin can perform this operation</response>
		[Authorize]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<EmployeeData>>> GetAll()
		{
			try
			{
				var eployees = await _service.GetEmployees(GetInitiatorId());
				return Ok(eployees.ToList());
			}
			catch (UnauthorizedException)
			{
				return StatusCode(StatusCodes.Status401Unauthorized);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error");
				return StatusCode(500);
			}
		}
	}
}
