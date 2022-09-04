using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cdemo.Customers.Services;
using Cdemo.Customers.Entities;

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
			try
			{
				var customers = await _service.GetCustomers(GetInitiatorId());
				return Ok(customers.ToList());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error");
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Get customer data by id
		/// </summary>
		/// <param name="customerId">Customer id</param>
		/// <returns>Customer data</returns>
		/// <response code="200">Successful</response>
		/// <response code="404">Customer not found</response>
		[Authorize]
		[HttpGet]
		[Route("{customerId}")]
		public async Task<ActionResult<CustomerExtData?>> GetById(Guid customerId)
		{
			try
			{
				var customer = await _service.GetCustomer(customerId, GetInitiatorId());

				if (customer == null)
				{
					return NotFound();
				}
				else
				{
					return Ok(customer);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error");
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Add customer note
		/// </summary>
		/// <param name="customerId">Customer id</param>
		/// <param name="text">Note text</param>
		/// <response code="200">Successful</response>
		[Authorize]
		[HttpPost]
		[Route("{customerId}/notes/actions/add")]
		public async Task<IActionResult> PostAddCustomerNote(Guid customerId, string text)
		{
			try
			{
				var now = DateTime.UtcNow;
				await _service.AddCustomerNote(customerId, now, text, GetInitiatorId());
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error");
				return StatusCode(500);
			}
		}

		/// <summary>
		/// Get all customer notes
		/// </summary>
		/// <param name="customerId">Customer Id</param>
		/// <returns>Customer notes list</returns>
		/// <response code="200">Successful</response>
		[Authorize]
		[HttpGet]
		[Route("{customerId}/notes")]
		public async Task<ActionResult<IEnumerable<CustomerData>>> GetCustomerNotes(Guid customerId)
		{
			try
			{
				var notes = await _service.GetCustomerNotes(customerId, GetInitiatorId());
				return Ok(notes.ToList());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error");
				return StatusCode(500);
			}
		}
	}
}
