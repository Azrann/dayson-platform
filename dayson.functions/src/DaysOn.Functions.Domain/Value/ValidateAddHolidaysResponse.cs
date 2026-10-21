using System;
using System.Collections.Generic;
using DaysOn.Common.Events.ResultEvents;

namespace DaysOn.Functions.BoundedContext.Value
{
    public class ValidateAddHolidaysResponse : BaseResultEvent
    {
        public Dictionary<DayOfWeek, bool> WorkingDaysOfTheWeek { get; set; }
        public List<DateTime> NonWorkingDays { get; set; }
        public List<DateTime> NonWorkingHalfDays { get; set; }
        public DateTime YearStartDay { get; set; }
    }
}
