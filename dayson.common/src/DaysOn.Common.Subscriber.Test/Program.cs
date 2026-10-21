using DaysOn.Common.Events;
using log4net;
using log4net.Config;
using System;
using System.Linq;
using System.Threading;
using DaysOn.Common.AzureBus;
using DaysOn.Common.CloudSites;
using DaysOn.Common.MessageHandlers;
using Moq;
using Newtonsoft.Json;
using JustEat.StatsD;

namespace DaysOn.Common.Subscriber.Test
{
    class Program
    {
        static void Main(string[] args)
        {

             XmlConfigurator.Configure();
             ILog _logger = LogManager.GetLogger("Test Console Azure APP");
            TestReadingFromDeadLetterAndCleaningSubscriptionWithMoreMessages(_logger);
             /*LastPingEvent.AddGuid(Guid.NewGuid());
             var result = LastPingEvent.GetDate();
             LastPingEvent.SetDate(DateTime.Now);
             result = LastPingEvent.GetDate();
             TestFailOnHandlerAnFinallyDeadLetter(_logger);*/

            /*Console.WriteLine("Scaling up 2 sites MUVA!");

            DaysOnCloudService.ScaleUpSitesForTest().Wait();

            Console.WriteLine("Scaling up Completado!");*/
        }

        private static void TestReadingFromDeadLetter(ILog _logger)
        {
            IServiceBusExplorer _bus = new ServiceBusExplorer();
            _bus.GetTopicsWithSubscriptions();

            Console.WriteLine("Type q to exit");
            while (Console.ReadLine() != "q")
            {
            }
        }

        private static void TestReadingFromSubscriptionsAndCleaningSubscriptionWithMoreMessages(ILog _logger)
        {
            IServiceBusExplorer _bus = new ServiceBusExplorer();
            var Topics = _bus.GetTopicsWithSubscriptions();
            var subscriptions = Topics.SelectMany(x => x.Subscriptions).ToList().Where(y => y.NumMessages > 0).OrderByDescending(x => x.NumMessages);
            Console.ReadKey();
            var subscriptionDto = subscriptions.First();
            Console.Clear();
            Console.WriteLine("Press any key to delete all mesages from this subscription");
            Console.WriteLine(JsonConvert.SerializeObject(subscriptionDto));
            Console.ReadKey();
            _bus.CleanSubscriptionQueue(subscriptionDto.TopicPath, subscriptionDto.Name);
            Console.WriteLine("Deleted all messages");
            Console.ReadKey();
        }

        private static void TestReadingFromDeadLetterAndCleaningSubscriptionWithMoreMessages(ILog _logger)
        {
            IServiceBusExplorer _bus = new ServiceBusExplorer();
            var Topics = _bus.GetTopicsWithSubscriptions();
            var subscriptions = Topics.SelectMany(x => x.Subscriptions).ToList().Where(y => y.NumDeadLetterMessages > 0).OrderByDescending(x => x.NumDeadLetterMessages);
            Console.ReadKey();
            var subscriptionDto = subscriptions.First();
            Console.Clear();
            Console.WriteLine("Press any key to delete all mesages from this error queue subscription");
            Console.WriteLine(JsonConvert.SerializeObject(subscriptionDto));
            Console.ReadKey();
            _bus.CleanSubscriptionErrorQueue(subscriptionDto.TopicPath, subscriptionDto.Name);
            Console.WriteLine("Deleted all messages");
            Console.ReadKey();
        }

        private static void TestFailOnHandlerAnFinallyDeadLetter(ILog _logger)
        {
            Mock<IStatsDPublisher> mockStats = new Mock<IStatsDPublisher>();
            IServiceBus _bus = new AzureServiceBus("QA","Test", mockStats.Object);
            _bus.RegisterEvent(typeof(TestPingEvent));
            _bus.Subscribe(new TestPingEventHandler());
            _logger.Debug("Ping method called.");
            _bus.Publish(new TestPingEvent()
            {
                LegacyId = 67,
                CorrelationId = Guid.NewGuid(),
            });

            Console.WriteLine("Type q to exit");
            while (Console.ReadLine() != "q")
            {
                _bus.Publish(new TestPingEvent()
                {
                    LegacyId = 67,
                    CorrelationId = Guid.NewGuid(),
                });
            }
        }

        private static void TestScaleUp()
        {
            
        }
    }  
}
