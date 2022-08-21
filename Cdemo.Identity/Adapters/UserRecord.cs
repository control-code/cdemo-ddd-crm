namespace Cdemo.Identity.Adapters
{
	public record UserRecord(Guid Id, string Name, string PassHash, bool IsAdmin);
}
