using System;
using System.Collections.Generic;
using DaysOn.Common.Events.Events;

namespace DaysOn.Common.Events.FunctionsEvents
{
    public class CalculateBusinessDaysEvent : BaseEvent
    {
        public Dictionary<DayOfWeek, bool> WorkingDaysOfTheWeek { get; set; }
        public Guid SubscriberId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<DateTime> NonWorkingDays { get; set; }
        public List<DateTime> NonWorkingHalfDays { get; set; }
    }
}
