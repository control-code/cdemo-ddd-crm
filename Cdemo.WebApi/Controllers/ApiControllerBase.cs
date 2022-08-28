using Microsoft.AspNetCore.Mvc;

namespace Cdemo.WebApi.Controllers
{
	public class ApiControllerBase : Controller
	{
		protected Guid GetInitiatorId()
		{
			return Guid.Parse(User.Claims.Single(i => i.Type == "id").Value);
		}
	}
}
