namespace Cdemo.Customers.Entities
{
	public record CustomerState(DateTime RegistrationDateTime, Guid ResponsibleUserId, 
		string FirstName, string LastName, 
		string Phone, string Email);
}
