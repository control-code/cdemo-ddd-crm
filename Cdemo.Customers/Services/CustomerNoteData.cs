using System;

namespace Cdemo.Customers.Services
{
	public record CustomerNoteData(Guid Id, Guid AuthorUserId, Guid CustomerId, DateTime DateTime, string Text);
}
