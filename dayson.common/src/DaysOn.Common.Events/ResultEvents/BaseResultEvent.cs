using System;
using System.Collections.Generic;

namespace DaysOn.Common.Events.ResultEvents
{
    public class BaseResultEvent : IBusEvent
    {
        public int RetryCount { get; set; }
        public bool Ok { get; set; }
        public List<string> Errors { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
