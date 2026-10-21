using System;

namespace DaysOn.Common.Events.Events
{
    public class PingEvent: BaseEvent
    {
        public DateTime SendDate { get; set; }
    }

    public class FunctionsPingEvent : PingEvent
    {
    }

    public class AuthPingEvent : PingEvent
    {
    }

    public class CompanyPingEvent : PingEvent
    {
    }

    public class HolidaysPingEvent : PingEvent
    {
    }
}
