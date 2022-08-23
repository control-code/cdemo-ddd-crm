using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Cdemo.Identity.Services;

namespace Cdemo.Identity.Controllers
{
	/// <summary>
	/// Application user actions
	/// </summary>
	[ApiController]
	[Route("api/v1/users")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _service;
		private readonly ILogger<UserController> _logger;

		public UserController(IUserService service, ILogger<UserController> logger)
		{
			_service = service;
			_logger = logger;
		}

		/// <summary>
		/// User login action
		/// </summary>
		/// <param name="name">User name</param>
		/// <param name="pass">User password</param>
		/// <returns>Authentication cookie</returns>
		/// <response code="200">Successful login</response>
		/// <response code="404">User not found</response>
		[HttpPost]
		[Route("actions/log-in")]
		public async Task<IActionResult> PostLogin(string name, string pass)
		{
			var id = await _service.Login(name, pass);

			if (id == Guid.Empty)
			{
				return Forbid();
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, name),
				new Claim("id", id.ToString())
			};

			var cid = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(cid));

			return Ok();
		}

		/// <summary>
		/// User logout action
		/// </summary>
		/// <returns>Success if user authenticated</returns>
		/// <response code="200">Successful logout</response>
		/// <response code="404">Not authenticated</response>
		[Authorize]
		[HttpPost]
		[Route("actions/log-out")]
		public async Task<IActionResult> PostLogout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Ok();
		}

		/// <summary>
		/// New user registration
		/// </summary>
		/// <param name="name">New user name</param>
		/// <param name="pass">New user password</param>
		/// <returns>Success if the name is not already taken</returns>
		/// <response code="200">User successfully registered</response>
		/// <response code="400">User name already taken</response>
		[HttpPost]
		[Route("actions/register")]
		public async Task<IActionResult> PostRegister(string name, string pass)
		{
			try
			{
				await _service.Register(name, pass);
				return Ok();
			}
			catch (NameAlreadyTakenException)
			{
				return BadRequest($"User name {name} already taken");
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[Authorize]
		[HttpGet]
		[Route("current/name")]
		public string GetName()
		{
			var n = User.Claims.Single(i => i.Type == "id") + " " + User.Identity?.Name;
			return n;
		}
	}
}
