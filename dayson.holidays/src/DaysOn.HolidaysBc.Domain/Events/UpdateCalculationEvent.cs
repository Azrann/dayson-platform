using System;
using DaysOn.Common.Domain.DomainEvents;

namespace DaysOn.HolidaysBc.BoundedContext.Events
{
    public class UpdateCalculationsEvent : IDomainEvent
    {
        public readonly Guid EmployeeId;
        public readonly int Year;

        public UpdateCalculationsEvent(Guid employeeId, int year)
        {
            EmployeeId = employeeId;
            Year = year;
        }
    }
}
