using System;

namespace DaysOn.Common.Events.Events
{
    public class TemporalManagerAddedToTeamEvent : BaseEvent
    {
        public Guid EmployeeId { get; set; }
        public Guid TeamId { get; set; }
    }
}
