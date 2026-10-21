using System;
using DaysOn.Common.Events.ResultEvents;

namespace DaysOn.Functions.BoundedContext.Value
{
    public class GetBusinessDaysResponse : BaseResultEvent
    {
        public decimal Days { get; set; }
        public DateTime StartDayOfYear { get; set; }
    }
}
