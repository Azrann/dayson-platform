using System;
using System.Threading;
using PubnubApi;
using Newtonsoft.Json;
using log4net;
using DaysOn.Common.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;

namespace DaysOn.Common
{
    public class PubnubMessageBus : IPubnubMessageBus
    {
        Pubnub _pubnub;
        readonly PNConfiguration _config;

        readonly ILog _logger;
        private readonly string _enviroment;

        public PubnubMessageBus(ILog logger, string enviroment)
        {
            _logger = logger;
            _enviroment = enviroment;
            var subscriberKey = System.Configuration.ConfigurationManager.AppSettings["REPLACE_WITH_YOUR_VALUE"];
            var publisherKey = System.Configuration.ConfigurationManager.AppSettings["REPLACE_WITH_YOUR_VALUE"];
            
            if (string.IsNullOrEmpty(subscriberKey))
            {
                throw new Exception("PubNub REPLACE_WITH_YOUR_VALUE is missing from the _config file.");
            }
            if (string.IsNullOrEmpty(publisherKey))
            {
                throw new Exception("PubNub REPLACE_WITH_YOUR_VALUE is missing from the _config file.");
            }

            _config = new PNConfiguration()
            {
                PublishKey = publisherKey,
                SubscribeKey = subscriberKey,
                Secure = true,
                LogVerbosity = PNLogVerbosity.BODY
            };

           _logger.Info("PubnubMessageBus configured");
        }

        public IPubnubMessageBus SetUuid(string uuid)
        {
             _config.Uuid = uuid;
             return this;
        }

        public IPubnubMessageBus Start()
        {
            _logger.Info("PubnubMessageBus Start called");
            if (string.IsNullOrEmpty(_config.Uuid))
            {
                _config.Uuid = Guid.NewGuid().ToString();
            }
            _pubnub = new Pubnub(_config) { };
            _logger.Info("PubnubMessageBus started");
            return this;
        }

        public void Publish<T>(T busEvent, Guid channelId) where T : IBusEvent
        {
            var channelName = GetChannelNameFromType(busEvent.GetType(), channelId);
            Publish<T>(busEvent, channelName);
        }

        public void Publish<T>(T busEvent, string channelName) where T : IBusEvent
        {
            _logger.Info($"Publishing message to channel {channelName} with data {JsonConvertExt.SerializeObject(busEvent)}");

            _pubnub.Publish()
                .Message(busEvent)
                .Channel(channelName)
                .UsePOST(true)
                .Async(new PNPublishResultExt(
                    (result, status) => {
                        if (status.Error)
                        {
                            _logger.Error($"Error publishing message {JsonConvertExt.SerializeObject(busEvent)} to channel {channelName} error info {JsonConvertExt.SerializeObject(status.ErrorData)}");
                        }
                    }
                ));
        }

        public void Subscribe<T>(IMessageHandler<T> handler, Guid channelId) where T : IBusEvent
        {
            var channelName = GetChannelNameFromType(typeof(T), channelId);
            Subscribe<T>(handler, channelName);
        }

        public void Subscribe<T>(IMessageHandler<T> handler, string channelName) where T : IBusEvent
        {
            _logger.Info($"Subscribing to channel {channelName}");
            SubscribeCallbackExt listenerSubscribeCallack = new SubscribeCallbackExt(
                (pubnubObj, message) =>
                {
                    try
                    {
                        _logger.Info($"Pubnub message received {message}");
                        var data = JsonConvert.DeserializeObject<T>(message.Message.ToString());
                        handler.Handle(data);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Error procesing message {message.ToString()} with exception {ex.Message}");
                    }
                },
                (pubnubObj, presence) => { },
                (pubnubObj, status) => { });

            _pubnub.AddListener(listenerSubscribeCallack);
            _pubnub.Subscribe<string>()
                .Channels(new[] {channelName})
                .Execute();
        }

        public void GrantRead(string authKey, string channel)
        {
            ManualResetEvent grantManualEvent = new ManualResetEvent(false);
            _logger.Info("Called GrantRead access.");
            _pubnub.Grant()
                .Channels(new[]
                {
                    channel
                })
                .AuthKeys(new[]
                {
                    authKey
                })
                .Write(false)
                .Read(true)
                .TTL(12337)
                .Async(new PNAccessManagerGrantResultExt(
                    (result, status) => {
                        if (status.Error)
                        {
                            _logger.Error($"Error granting adding access authkey={authKey} to channel={channel} error info {JsonConvertExt.SerializeObject(status.ErrorData)}");
                        }
                        else
                        {
                            _logger.Info($"Access Granted access authkey={authKey} to channel={channel} ");

                        }

                        grantManualEvent.Set();
                    }
                ));
                grantManualEvent.WaitOne();
        }

        public void GrantAll(string authKey, string channel)
        {
            ManualResetEvent grantManualEvent = new ManualResetEvent(false);
            _logger.Info("Called GrantAll access.");
            _pubnub.Grant()
                .Channels(new[]{
                    channel
                })
                .AuthKeys(new[] {
                    authKey
                })
                .Write(true)
                .Read(true)
                .TTL(12337)
                .Async(new PNAccessManagerGrantResultExt(
                    (result, status) => {
                        if (status.Error)
                        {
                            _logger.Error($"Error granting adding access authkey={authKey} to channel={channel} error info {JsonConvertExt.SerializeObject(status.ErrorData)}");
                        }
                        else
                        {
                            _logger.Info($"Access Granted access authkey={authKey} to channel={channel} ");

                        }
                        grantManualEvent.Set();
                    }
                ));
            grantManualEvent.WaitOne();
        }

        private string GetChannelNameFromType(Type busEventType, Guid ChannelId)
        {
            return $"{_enviroment}_{busEventType.FullName}{ChannelId.ToString()}";
        }

        public IPubnubMessageBus SetAuth(string authKey)
        {
            _config.AuthKey = authKey;
            return this;
        }

        public IPubnubMessageBus SetSecretKey()
        {
            var secretKey = System.Configuration.ConfigurationManager.AppSettings["REPLACE_WITH_YOUR_VALUE"];
            if (!string.IsNullOrEmpty(secretKey))
            {
                _config.SecretKey = secretKey;
            }
            return this;
        }
    }
}
