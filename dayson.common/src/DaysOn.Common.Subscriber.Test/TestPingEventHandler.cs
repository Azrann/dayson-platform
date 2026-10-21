using System;
using DaysOn.Common.Events;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using Newtonsoft.Json;

namespace DaysOn.Common.Subscriber.Test
{
    public class TestPingEventHandler : IMessageHandler<TestPingEvent>
    {
        public bool Handle(TestPingEvent message)
        {
            return false;
        }

        public void LogError(Exception ex)
        {
            Console.WriteLine("Exception thrown {0}", ex);
        }

        public void LogMessage(string message)
        {
            DateTime time = DateTime.Now;
            Console.WriteLine($"{time.Minute}:{time.Second}:{time.Millisecond} {message}");
        }
    }

    public class TestPingEvent : BaseEvent
    {
        public int LegacyId { get; set; }
    }
}
