using System;

namespace DaysOn.Common.Events.Events
{
    public class EmployeeUpdatedBusEvent : BaseEvent
    {
        public string EmployeeEmailAddress { get; set; }
        public Guid CompanyId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Role { get; set; }
    }
}
