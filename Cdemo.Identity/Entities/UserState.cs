namespace Cdemo.Identity.Entities
{
	public record UserState(string Name, string PassHash, bool IsAdmin);
}
