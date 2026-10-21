using System;
using DaysOn.Common.Events;
using DaysOn.Common.MessageHandlers;

namespace DaysOn.Common.AzureBus
{
    public interface IServiceBus
    {
        void Publish<T>(T busEvent) where T : IBusEvent;

        void Subscribe<T>(IMessageHandler<T> handler) where T : class, IBusEvent;
        void Subscribe<T>(IMessageHandlerAsync<T> handler) where T : class, IBusEvent;

        void RegisterEvent(Type busEventType);
    }
}