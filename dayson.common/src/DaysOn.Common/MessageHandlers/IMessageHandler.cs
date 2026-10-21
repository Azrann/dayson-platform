using System;
using DaysOn.Common.Events;

namespace DaysOn.Common.MessageHandlers
{
    public interface IMessageHandler<T> where T : IBusEvent
    {
        bool Handle(T message);
        void LogError(Exception ex);
        void LogMessage(string message);
    }
}
