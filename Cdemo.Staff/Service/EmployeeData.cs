using System;

namespace Cdemo.Staff.Service
{
    public record EmployeeData(Guid Id, Guid UserId, string FirstName, string LastName);
}
