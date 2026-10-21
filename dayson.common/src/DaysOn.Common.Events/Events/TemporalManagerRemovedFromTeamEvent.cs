using System;

namespace DaysOn.Common.Events.Events
{
    public class TemporalManagerRemovedFromTeamEvent : BaseEvent
    {
        public Guid EmployeeId { get; set; }
        public Guid TeamId { get; set; }
    }
}
