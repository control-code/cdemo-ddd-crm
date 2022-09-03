namespace Cdemo.Customers.Services
{
	public record CustomerData(Guid Id, string FirstName, string LastName, 
		string ResponsibleUserFirstName, string ResponsibleUserLastName);
}
