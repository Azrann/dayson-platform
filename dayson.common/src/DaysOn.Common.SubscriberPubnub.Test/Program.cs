using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaysOn.Common.Events.ResultEvents;
using DaysOn.Common.MessageHandlers;
using log4net;
using Moq;
using Newtonsoft.Json;

namespace DaysOn.Common.SubscriberPubnub.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Mock<ILog> mockLog = new Mock<ILog>();

            IPubnubMessageBus bus = new PubnubMessageBus(mockLog.Object, ConfigurationManager.AppSettings["Enviroment"]);
            bus.SetSecretKey().Start();
            Console.WriteLine("Pubnub initialized.");
            MockMessageReader _mockMessageReader = new MockMessageReader();

            Console.WriteLine("Subscribe press Enter");
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                bus.Subscribe(_mockMessageReader, "Terremoto");
                Console.WriteLine("Pubnub subscribed.");
            }
            else
            {
                Console.WriteLine("Pubnub NOT subscribed.");
            }

            Console.WriteLine("Press Escape for exit any key for publish");
            key = Console.ReadKey();
            while (key.Key != ConsoleKey.Escape)
            { 
                bus.Publish(new StringResultEvent
                {
                    Result = $"Cabrones!!! {Guid.NewGuid()}",
                    Ok = true,
                    RetryCount = 0,
                    CorrelationId = Guid.NewGuid(),
                    Errors = null
                }, "Terremoto");
                Console.WriteLine("Message Published");
                key = Console.ReadKey();
            }
        }
    }

    public class MockMessageReader : IMessageHandler<StringResultEvent>
    {
        public int numCalls { get; set; }

        public MockMessageReader()
        {
            numCalls = 0;
        }

        public bool Handle(StringResultEvent message)
        {
            Console.WriteLine(JsonConvert.SerializeObject(message));
            return true;
        }

        public void LogError(Exception ex)
        {
            Console.WriteLine(JsonConvert.SerializeObject(ex));
        }

        public void LogMessage(string message)
        {
            Console.WriteLine(JsonConvert.SerializeObject(message));
        }
    }
}
