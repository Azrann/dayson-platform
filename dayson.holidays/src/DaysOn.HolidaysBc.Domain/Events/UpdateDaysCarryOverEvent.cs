using System;
using DaysOn.Common.Domain.DomainEvents;

namespace DaysOn.HolidaysBc.BoundedContext.Events
{
    public class UpdateDaysCarryOverEvent : IDomainEvent
    {
        public readonly Guid EmployeeId;
        public readonly int Year;
        public readonly decimal Numdays;

        public UpdateDaysCarryOverEvent(Guid employeeId, int year, decimal numdays)
        {
            EmployeeId = employeeId;
            Year = year;
            Numdays = numdays;
        }
    }
}
