using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace DaysOn.Common.AzureBus
{
    public interface IServiceBusExplorer
    {
        List<TopicDto> GetTopicsWithSubscriptions();
        bool CleanSubscriptionQueue(string topicPath, string subscriptionName);
        bool CleanSubscriptionErrorQueue(string topicPath, string subscriptionName);
    }

    public class TopicDto 
    {
        public TopicDto()
        {
            Subscriptions = new List<SubscriptionDto>();
        }

        public string Name { get; set; }
        public long NumDeadLetterMessages { get; set; }
        public long NumSubscriptions { get; set; }
        public List<SubscriptionDto> Subscriptions { get; set; }
}

    public class SubscriptionDto
    {
        public string Name { get; set; }
        public long NumDeadLetterMessages { get; set; }
        public long NumMessages { get; set; }
        public string TopicPath { get; set; }
    }

    public class ServiceBusExplorer : IServiceBusExplorer
    {
        private readonly NamespaceManager _namespaceManager;
        public ServiceBusExplorer()
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            _namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);           
        }

        public List<TopicDto> GetTopicsWithSubscriptions()
        {
            List<TopicDto> result = new List<TopicDto>();
            var topics = _namespaceManager.GetTopics();
            foreach (var topic in topics)
            {
                var topicDto = new TopicDto()
                {
                    Name = topic.Path,
                    NumDeadLetterMessages = topic.MessageCountDetails.DeadLetterMessageCount,
                    NumSubscriptions = topic.SubscriptionCount
                };

                var subscriptionDescriptions = _namespaceManager.GetSubscriptions(topicDto.Name);
                foreach (var subscriptionDescription in subscriptionDescriptions)
                {
                    SubscriptionDto subscription = new SubscriptionDto()
                    {
                        NumDeadLetterMessages = subscriptionDescription.MessageCountDetails.DeadLetterMessageCount,
                        Name = subscriptionDescription.Name,
                        NumMessages = subscriptionDescription.MessageCountDetails.ActiveMessageCount,
                        TopicPath = topicDto.Name
                    };
                    topicDto.Subscriptions.Add(subscription);
                    Console.WriteLine(subscription.Name);
                    Console.WriteLine(JsonConvert.SerializeObject(subscription));
                    Console.WriteLine("\n");
                }

                result.Add(topicDto);
            }
            return result;
        }

        public bool CleanSubscriptionQueue(string topicPath, string subscriptionName)
        {
            if (!_namespaceManager.SubscriptionExists(topicPath, subscriptionName))
            {
                Console.WriteLine($"Subscription {subscriptionName} no existe!");
                return false;
            }

            var subscriptionPath = SubscriptionClient.FormatSubscriptionPath(topicPath, subscriptionName);

            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var client = SubscriptionClient.CreateFromConnectionString(
                connectionString, subscriptionPath, subscriptionName, ReceiveMode.ReceiveAndDelete);

            int count = 0;
            var receivedMessage = client.Receive();
            while (receivedMessage != null)
            {
                count++;
                Console.Clear();
                Console.WriteLine($"Deleting message {count} with id {receivedMessage.MessageId}");
                receivedMessage = client.Receive(TimeSpan.FromSeconds(2));
            }
            return true;
        }

        public bool CleanSubscriptionErrorQueue(string topicPath, string subscriptionName)
        {
            if (!_namespaceManager.SubscriptionExists(topicPath, subscriptionName))
            {
                Console.WriteLine($"Subscription {subscriptionName} no existe!");
                return false;
            }
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(connectionString);
            var deadLetterPath = SubscriptionClient.FormatDeadLetterPath(topicPath, subscriptionName);
            var dlqReceiver = factory.CreateMessageReceiver(deadLetterPath, ReceiveMode.ReceiveAndDelete);

            int count = 0;
            var receivedMessage = dlqReceiver.Receive();
            while (receivedMessage != null)
            {
                count++;
                Console.Clear();
                Console.WriteLine($"Deleting message {count} with id {receivedMessage.MessageId}");
                receivedMessage = dlqReceiver.Receive(TimeSpan.FromSeconds(2));
            }
            return true;
        }
    }
}
