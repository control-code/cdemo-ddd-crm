using Microsoft.AspNetCore.Authentication.Cookies;
using Cdemo.AdaptersImpl;
using Cdemo.Identity.AdaptersImpl;
using Cdemo.Identity.Entities;
using Cdemo.Identity.Services;
using Cdemo.Identity.ServicesImpl;
using Microsoft.Extensions.DependencyInjection;
using Cdemo.Identity.Adapters;
using Cdemo.Adapters;

namespace Cdemo.WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
			builder.Services.AddControllers();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var connectionString = builder.Configuration.GetConnectionString("MsSql");

			builder.Services.AddSingleton<IRepository<User, UserState>, Repository<User, UserState>>();
			builder.Services.AddSingleton<IUserQueryAdapter, UserQueryAdapter>();
			builder.Services.AddSingleton<IUserService, UserService>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}