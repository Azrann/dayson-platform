using System;
using DaysOn.Common.Events;
using DaysOn.Common.MessageHandlers;
using PubnubApi;

namespace DaysOn.Common
{
    public interface IPubnubMessageBus
    {
        void Publish<T>(T busEvent, Guid ChannelId) where T : IBusEvent;

        void Publish<T>(T busEvent, string channelName) where T : IBusEvent;

        void Subscribe<T>(IMessageHandler<T> handler, Guid ChannelId) where T : IBusEvent;

        void Subscribe<T>(IMessageHandler<T> handler, string channelName) where T : IBusEvent;

        void GrantRead(string authKey, string channel);
        void GrantAll(string authKey, string channel);

        IPubnubMessageBus SetAuth(string authKey);
        IPubnubMessageBus SetSecretKey();
        IPubnubMessageBus SetUuid(string uuid);

        IPubnubMessageBus Start();
    }
}