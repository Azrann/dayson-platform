using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using DaysOn.Common.Events;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using JustEat.StatsD;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace DaysOn.Common.AzureBus
{
    public class AzureServiceBus : IServiceBus
    {
        private const string RetryCountKey = "RetryCount";
        private readonly string _enviroment;
        private readonly string _feature;
        private readonly IStatsDPublisher _statsDPublisher;

        public AzureServiceBus(string enviroment,string feature, IStatsDPublisher statsDPublisher)
        {
            _enviroment = enviroment;
            _feature = feature;
            _statsDPublisher = statsDPublisher;
        }

        public void RegisterEvent(Type busEventType)
        {
            if (!typeof(IBusEvent).IsAssignableFrom(busEventType))
            {
                throw new Exception("Type must inherit from IBusEvent");
            }

            var topicName = GetTopicNameFromType(busEventType);

            CreateTopicIfNotExists(topicName);
        }

        public void Publish<T>(T busEvent) where T : IBusEvent
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

            var topicName = GetTopicNameFromType(busEvent.GetType());

            var client = TopicClient.CreateFromConnectionString(connectionString, topicName);
           
            Console.WriteLine("Sending message : {0}", busEvent);

            var message = new BrokeredMessage(busEvent);

            client.Send(message);
        }

        public void Subscribe<T>(IMessageHandler<T> handler) where T : class, IBusEvent
        {
            if (!typeof(IBusEvent).IsAssignableFrom(typeof(T)))
            {
                throw new Exception("Type must inherit from IBusEvent");
            }

            CreateSubscription(handler);

            StartListeningTo<T>(handler);
        }

        public void Subscribe<T>(IMessageHandlerAsync<T> handler) where T : class, IBusEvent
        {
            if (!typeof(IBusEvent).IsAssignableFrom(typeof(T)))
            {
                throw new Exception("Type must inherit from IBusEvent");
            }

            CreateSubscriptionAsync(handler);

            StartListeningToAsync<T>(handler);
        }

        private string GetTopicNameFromType(Type busEventType)
        {
            return $"{_enviroment}_{busEventType.FullName}";
        }

        private static void CreateTopicIfNotExists(string topicName)
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            namespaceManager.Settings.RetryPolicy = new RetryExponential(minBackoff: TimeSpan.FromSeconds(0.1),
                maxBackoff: TimeSpan.FromSeconds(10),
                maxRetryCount: 3);

            if (!namespaceManager.TopicExists(topicName))
            {
                namespaceManager.CreateTopic(topicName);
            }
        }

        private void StartListeningToAsync<T>(IMessageHandlerAsync<T> handler) where T : class, IBusEvent
        {
            var topicName = GetTopicNameFromType(typeof(T));
            var subscriptionName = GetSubscriptionNameFromType(handler);

            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

            SubscriptionClient client = SubscriptionClient.CreateFromConnectionString(connectionString, topicName, subscriptionName);
           
            OnMessageOptions options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(5)
            };

            client.OnMessageAsync(async message =>
            {
                T body = null;
                var watch = Stopwatch.StartNew();
                try
                {
                    body = message.GetBody<T>();
                    var result = await handler.Handle(body);
                    if (result)
                    {
                        handler.LogMessage($"Bus: message id {body?.CorrelationId.ToString() ?? string.Empty} completed.");
                        await message.CompleteAsync();
                        LogStats(handler, watch, "completed");
                    }
                    else
                    {
                        await ReenqueMessageAsync<T>(handler, message, connectionString, topicName, body.CorrelationId.ToString());
                        LogStats(handler, watch, "reenqued");
                    }
                }
                catch (Exception ex)
                {

                    var correlationId = body?.CorrelationId.ToString() ?? string.Empty;
                    await ReenqueMessageAsync<T>(handler, message, connectionString, topicName, correlationId, ex);
                    LogStats(handler, watch, "exceptionreenqued");
                }
            },options);
        }

        private void StartListeningTo<T>(IMessageHandler<T> handler) where T : class, IBusEvent
        {
            var topicName = GetTopicNameFromType(typeof(T));
            var subscriptionName = GetSubscriptionNameFromType(handler);

            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

            SubscriptionClient client = SubscriptionClient.CreateFromConnectionString(connectionString, topicName, subscriptionName);
            OnMessageOptions options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(5)
            };

            client.OnMessage((message) =>
            {
                T body = null;
                var watch = Stopwatch.StartNew();
                try
                {
                    body = message.GetBody<T>();
                    var result = handler.Handle(body);
                    if (result)
                    {
                        handler.LogMessage($"Bus: message id {body?.CorrelationId.ToString() ?? string.Empty} completed.");
                        message.Complete();
                        LogStats(handler, watch, "completed");
                    }
                    else
                    {
                        ReenqueMessage(handler, message, connectionString, topicName, body.CorrelationId.ToString());
                        LogStats(handler, watch, "reenqued");
                        LogStats(handler, watch, "exceptionreenqued");
                    }
                }
                catch (Exception ex)
                {
                    var correlationId = body?.CorrelationId.ToString() ?? string.Empty;
                    ReenqueMessage(handler, message, connectionString, topicName, correlationId, ex);
                }
            }, options);
        }

        private void ReenqueMessage<T>(IMessageHandler<T> handler, BrokeredMessage message, string connectionString,
            string topicName, string correlationId, Exception ex = null) where T : IBusEvent
        {
            var retryCount = IncrementRetryCount(message);
            
            if (retryCount > 5)
            {
                handler.LogMessage($"Bus: message id {correlationId} retry {retryCount} DeadLetter.");
                message.DeadLetter("Failed after 5 retries", ex?.Message ?? "Message reenqueed");
            }
            else
            {
                LogReenqueMessage(handler, correlationId, ex, retryCount);

                var messageClone = message.Clone();
                message.Complete();
                messageClone.ScheduledEnqueueTimeUtc = GetEnqueueTime(retryCount);
                var topicClient = TopicClient.CreateFromConnectionString(connectionString, topicName);
                topicClient.Send(messageClone);
            }
        }

        private async Task ReenqueMessageAsync<T>(IMessageHandlerAsync<T> handler, BrokeredMessage message, string connectionString,
            string topicName, string correlationId, Exception ex = null) where T : IBusEvent
        {
            var retryCount = IncrementRetryCount(message);

            if (retryCount > 5)
            {
                handler.LogMessage($"Bus: message id {correlationId} retry {retryCount} DeadLetter.");
                message.DeadLetter("Failed after 6 retries", ex?.Message ?? "Message reenqueed");
            }
            else
            {
                LogReenqueMessageAsync<T>(handler, correlationId, ex, retryCount);

                var messageClone = message.Clone();
                await message.CompleteAsync();
                messageClone.ScheduledEnqueueTimeUtc = GetEnqueueTime(retryCount);
                var topicClient = TopicClient.CreateFromConnectionString(connectionString, topicName);
                await topicClient.SendAsync(messageClone);
            }
        }

        private void LogReenqueMessage<T>(IMessageHandler<T> handler, string correlationId, Exception ex, int retryCount)
            where T : IBusEvent
        {
            if (ex != null)
            {
                handler.LogError(ex);
            }
            handler.LogMessage($"Bus: message id {correlationId} retry {retryCount} reenqued.");
        }

        private void LogReenqueMessageAsync<T>(IMessageHandlerAsync<T> handler, string correlationId, Exception ex, int retryCount)
            where T : IBusEvent
        {
            if (ex != null)
            {
                handler.LogError(ex);
            }
            handler.LogMessage($"Bus: message id {correlationId} retry {retryCount} reenqued.");
        }

        private DateTime GetEnqueueTime(int retryCount)
        {
            var miliseconds = 250 * Math.Pow(2,retryCount);
            return DateTime.UtcNow.AddMilliseconds(miliseconds);
        }

        private int IncrementRetryCount(BrokeredMessage message)
        {
            if (!message.Properties.ContainsKey(RetryCountKey))
            {
                message.Properties.Add(RetryCountKey, 0);
            }
            message.Properties[RetryCountKey] = (int)message.Properties[RetryCountKey] + 1;
            return (int) message.Properties[RetryCountKey];
        }

        private void CreateSubscription<T>(IMessageHandler<T> handler) where T : IBusEvent
        {
            var topicName = GetTopicNameFromType(typeof(T));
            var subscriptionName = GetSubscriptionNameFromType(handler);

            CreateSubscription<T>(topicName, subscriptionName);
        }

        private void CreateSubscriptionAsync<T>(IMessageHandlerAsync<T> handler) where T : IBusEvent
        {
            var topicName = GetTopicNameFromType(typeof(T));
            var subscriptionName = GetSubscriptionNameFromType(handler);

            CreateSubscription<T>(topicName, subscriptionName);
        }

        private static void CreateSubscription<T>(string topicName, string subscriptionName) where T : IBusEvent
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.SubscriptionExists(topicName, subscriptionName))
            {
                namespaceManager.CreateSubscription(topicName, subscriptionName);
            }
        }

        private string GetSubscriptionNameFromType<T>(IMessageHandler<T> handler) where T : IBusEvent
        {
            var name = $"s_{typeof(T).Name}_{handler.GetType().FullName}";

            if (name.Length > 50)
            {
                name = name.Substring(0, 50);
            }

            return name;
        }

        private string GetSubscriptionNameFromType<T>(IMessageHandlerAsync<T> handler) where T : IBusEvent
        {
            var name = $"s_{typeof(T).Name}_{handler.GetType().FullName}";

            if (name.Length > 50)
            {
                name = name.Substring(0, 50);
            }

            return name;
        }

        private void LogStats<T>(IMessageHandlerAsync<T> handler, Stopwatch watch, string result) where T : class, IBusEvent
        {
            watch.Stop();
            var bucketName = GetBucketName<T>(handler.GetType().Name, result);
            _statsDPublisher.Timing(watch.Elapsed, bucketName);
        }

        private void LogStats<T>(IMessageHandler<T> handler, Stopwatch watch, string result) where T : class, IBusEvent
        {
            watch.Stop();
            var bucketName = GetBucketName<T>(handler.GetType().Name, result);
            _statsDPublisher.Timing(watch.Elapsed, bucketName);
        }

        private string GetBucketName<T>(string handlerName,string result)
        {
            try
            {
                return $"{_enviroment}.{_feature}.handlers.{typeof(T).Name}.{handlerName}.{result}".ToLower();
            }
            catch (Exception)
            {
                return "exception-publishing-stat";
            }
        }
    }
}