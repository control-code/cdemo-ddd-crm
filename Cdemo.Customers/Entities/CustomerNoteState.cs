namespace Cdemo.Customers.Entities
{
    public record CustomerNoteState(Guid  AuthorUserId, Guid CustomerId, DateTime DateTime, string Text);
}
