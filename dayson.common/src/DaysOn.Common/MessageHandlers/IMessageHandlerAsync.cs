using System;
using System.Threading.Tasks;
using DaysOn.Common.Events;

namespace DaysOn.Common.MessageHandlers
{
    public interface IMessageHandlerAsync<T> where T : IBusEvent
    {
        Task<bool> Handle(T message);
        void LogError(Exception ex);
        void LogMessage(string message);
    }
}
