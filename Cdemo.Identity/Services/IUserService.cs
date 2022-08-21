using System;

namespace Cdemo.Identity.Services
{
	public interface IUserService
	{
		Task<Guid> Login(string name, string pass);

		Task Register(string name, string pass);
	}
}
