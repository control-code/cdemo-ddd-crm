namespace Cdemo.Identity.Services
{
	public class NameAlreadyTakenException : Exception
	{
		public NameAlreadyTakenException()
		{
		}

		public NameAlreadyTakenException(string message)
			: base(message)
		{
		}

		public NameAlreadyTakenException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
