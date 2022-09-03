using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using Cdemo.Adapters;
using Cdemo.AdaptersImpl;
using Cdemo.Identity.AdaptersImpl;
using Cdemo.Identity.Entities;
using Cdemo.Identity.Services;
using Cdemo.Identity.ServicesImpl;
using Cdemo.Identity.Adapters;
using Cdemo.Staff.Entities;
using Cdemo.Staff.Adapters;
using Cdemo.Staff.AdaptersImpl;
using Cdemo.Staff.ServiceImpl;
using Cdemo.Staff.Service;
using Cdemo.Customers.Entities;
using Cdemo.Customers.Adapters;
using Cdemo.Customers.AdaptersImpl;
using Cdemo.Customers.ServicesImpl;
using Cdemo.Customers.Services;

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
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Demo CRM API Example",
					Description = "An example implementation of Web API for simple demo CRM",
				});

				// using System.Reflection;
				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});

			var connectionString = builder.Configuration.GetConnectionString("MsSql");

			builder.Services.AddSingleton<IRepository<User, UserState>, Repository<User, UserState>>();
			builder.Services.AddSingleton<IUserQueryAdapter, UserQueryAdapter>();
			builder.Services.AddSingleton<IUserService, UserService>();

			builder.Services.AddSingleton<IRepository<Employee, EmployeeState>, Repository<Employee, EmployeeState>>();
			builder.Services.AddSingleton<IEmployeeQueryAdapter, EmployeeQueryAdapter>();
			builder.Services.AddSingleton<IEmployeeService, EmployeeService>();

			builder.Services.AddSingleton<IRepository<Customer, CustomerState>, Repository<Customer, CustomerState>>();
			builder.Services.AddSingleton<ICustomerQueryAdapter, CustomerQueryAdapter>();
			builder.Services.AddSingleton<ICustomerService, CustomerService>();

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