using System;

namespace DaysOn.Common.Events
{
    public interface IBusEvent
    {
        Guid CorrelationId { get; set; }
    }
}