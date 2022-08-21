using Microsoft.AspNetCore.Authentication.Cookies;
using Cdemo.AdaptersImpl;
using Cdemo.Identity.AdaptersImpl;
using Cdemo.Identity.Entities;
using Cdemo.Identity.Services;
using Cdemo.Identity.ServicesImpl;

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

			var userRepo = new Repository<User, UserState>(connectionString);
			var userQuery = new UserQueryAdapter(connectionString);
			builder.Services.AddSingleton<IUserService>(new UserService(userRepo, userQuery));

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