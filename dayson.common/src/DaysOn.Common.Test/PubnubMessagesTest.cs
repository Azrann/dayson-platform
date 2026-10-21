using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DaysOn.Common.Events.ResultEvents;
using DaysOn.Common.MessageHandlers;
using log4net;
using log4net.Config;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using PubnubApi;

namespace DaysOn.Common.Test
{
    [TestFixture]
    public class PubnubMessagesTest : EventTestBase
    {
        [Test]
        public void ICanSendAMessageToPubNub()
        {
            XmlConfigurator.Configure();
            ILog _logger = LogManager.GetLogger("Test CalculateBusinessDaysEventHandlerTests");

            IPubnubMessageBus bus = new PubnubMessageBus(_logger, ConfigurationManager.AppSettings["Enviroment"]);
            bus.SetSecretKey().Start();
            bus.GrantAll("test321", "Terremoto");

            IPubnubMessageBus bus2 = new PubnubMessageBus(_logger, ConfigurationManager.AppSettings["Enviroment"]);
            bus2.SetAuth("test321").Start();

            MockMessageReader _mockMessageReader = new MockMessageReader();
            bus2.Subscribe(_mockMessageReader, "Terremoto");
            int timeEllapsed;

            bus2.Publish(new StringResultEvent
            {
                Result = "Hello World",
                Ok = true,
                RetryCount = 0,
                CorrelationId = Guid.NewGuid(),
                Errors = null
            }, "Terremoto");

            var result = WaitFor(() => _mockMessageReader.numCalls == 1, 15000, out timeEllapsed);
            Assert.IsTrue(result);
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
            numCalls++;
            return true;
        }

        public void LogError(Exception ex)
        {
        }

        public void LogMessage(string message)
        {
        }
    }

    public class MockResultMessageReader : IMessageHandler<DoubleResultEvent>
    {
        public double Result { get; set; }

        public MockResultMessageReader()
        {
            Result = 0;
        }

        public bool Handle(DoubleResultEvent message)
        {
            Result = message.Result;
            return true;
        }

        public void LogError(Exception ex)
        {
        }

        public void LogMessage(string message)
        {
        }
    }

    public class PLog : IPubnubLog
    {
        private readonly ILog _logger;

        public PLog(ILog logger)
        {
            _logger = logger;
        }

        public void WriteToLog(string logText)
        {
            _logger.Info($"PUBNUB: {logText}");
        }
    }

    public class PlatformPubnubLog : IPubnubLog
    {
        private string logFilePath = "";

        public PlatformPubnubLog()
        {
            // Get folder path may vary based on environment
            string folder = System.IO.Directory.GetCurrentDirectory(); //For console
            //string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // For iOS
            System.Diagnostics.Debug.WriteLine(folder);
            logFilePath = System.IO.Path.Combine(folder, "pubnubmessaging.log");
            Trace.Listeners.Add(new TextWriterTraceListener(logFilePath));
        }

        public void WriteToLog(string log)
        {
            //Save to text file or DB or any storage
            Trace.WriteLine(log);
            Trace.Flush();
        }
    }
}
