using System;

namespace DaysOn.Common.Events.Events
{
    public class BaseEvent : IBusEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid ActionBy { get; set; }
        public DateTimeOffset ActionTime {get ;set;}

        public BaseEvent()
        {
            ActionTime = DateTimeOffset.UtcNow;
        }
    }
}
