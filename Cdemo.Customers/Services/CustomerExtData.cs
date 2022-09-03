namespace Cdemo.Customers.Services
{
	public record CustomerExtData(Guid Id, DateTime RegistrationDateTime, string FirstName, string LastName,
		string Phone, string Email, int NotesCount);
}
