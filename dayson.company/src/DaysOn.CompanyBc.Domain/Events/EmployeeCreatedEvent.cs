using System;
using System.IdentityModel.Metadata;
using DaysOn.Common.Domain.DomainEvents;

namespace DaysOn.CompanyBc.BoundedContext.Events
{
    public class EmployeeCreatedEvent : IDomainEvent
    {
        public readonly string AdminEmailAddress;
        public readonly string FirstName;
        public readonly string LastName;
        public readonly Guid CompanyId;
        public readonly Guid CorrelationId;
        public readonly Guid EmployeeId;
        public readonly string Role;

        public EmployeeCreatedEvent(Guid employeeId, string firstName, string lastName, string employeeEmailAddress, Guid companyId, Guid correlationId, string role = "")
        {
            EmployeeId = employeeId;
            AdminEmailAddress = employeeEmailAddress;
            CompanyId = companyId;
            CorrelationId = correlationId;
            Role = role;
            FirstName = firstName;
            LastName= lastName;
        }
    }
}