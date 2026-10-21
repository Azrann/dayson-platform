using Itenso.TimePeriod;
using System;

namespace DaysOn.HolidaysBc.BoundedContext.Value
{
    public static class TimePeriodExtensions
    {
        public static TimeRange GetTimeRange(this Entities.HolidayRequest holidayRequest)
        {
            DateTime virtualTo = holidayRequest.To.Hour == 0 ? holidayRequest.To.AddDays(1) : holidayRequest.To;
            return new TimeRange(holidayRequest.From, virtualTo);
        }
    }
}
