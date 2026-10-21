using System;

namespace DaysOn.Common.Events.Events
{
    public class EmployeeCreatedBusEvent : BaseEvent
    {
        public string EmployeeEmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid CompanyId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Role { get; set; }
    }
}
