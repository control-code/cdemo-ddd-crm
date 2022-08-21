using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Cdemo.Identity.Services;

namespace Cdemo.Identity.Controllers
{
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

		[Authorize]
		[HttpPost]
		[Route("actions/log-out")]
		public async Task<IActionResult> PostLogout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Ok();
		}

		[HttpPost]
		[Route("actions/register")]
		public async Task<IActionResult> PostRegister(string name, string pass)
		{
			try
			{
				await _service.Register(name, pass);
				return Ok();
			}
			catch (NameAlreadyTakenException ex)
			{
				return BadRequest(ex);
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
