using System;
using DaysOn.Common.Domain.DomainEvents;

namespace DaysOn.CompanyBc.BoundedContext.Events
{
    public class EmployeeUpdatedEvent : IDomainEvent
    {
        public readonly string AdminEmailAddress;
        public readonly Guid CompanyId;
        public readonly Guid CorrelationId;
        public readonly Guid EmployeeId;
        public readonly string Role;

        public EmployeeUpdatedEvent(Guid employeeId, string employeeEmailAddress, Guid companyId, Guid correlationId, string role = "")
        {
            EmployeeId = employeeId;
            AdminEmailAddress = employeeEmailAddress;
            CompanyId = companyId;
            CorrelationId = correlationId;
            Role = role;
        }
    }
}