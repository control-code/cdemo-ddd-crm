using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cdemo.Customers.Services;

namespace Cdemo.WebApi.Controllers
{
	/// <summary>
	/// Customers management
	/// </summary>
	[ApiController]
	[Route("api/v1/customers")]
	public class CustomerController : ApiControllerBase
	{
		private readonly ICustomerService _service;
		private readonly ILogger<CustomerController> _logger;

		public CustomerController(ICustomerService service, ILogger<CustomerController> logger)
		{
			_service = service;
			_logger = logger;
		}

		/// <summary>
		/// New customer registration
		/// </summary>
		/// <param name="firstName">Customer first name</param>
		/// <param name="lastName">Customer last name</param>
		/// <param name="phone">Customer phone number</param>
		/// <param name="email">Customer email address</param>
		/// <response code="200">Successful</response>
		[Authorize]
		[HttpPost]
		[Route("actions/register")]
		public async Task<IActionResult> PostRegisterNewCustomer(string firstName, string lastName, string phone, string email)
		{
			try
			{
				var now = DateTime.UtcNow;
				await _service.RegisterNewCustomer(now, firstName, lastName, phone, email, GetInitiatorId());
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error");
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Get the list of customers assigned to the current user
		/// </summary>
		/// <returns>List of customers</returns>
		/// <response code="200">Successful</response>
		[Authorize]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CustomerData>>> GetByUser()
		{
			var customers = await _service.GetCustomers(GetInitiatorId());
			return Ok(customers.ToList());
		}
	}
}
