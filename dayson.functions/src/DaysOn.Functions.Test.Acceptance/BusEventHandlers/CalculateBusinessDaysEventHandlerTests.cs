using System;
using System.Collections.Generic;
using System.Configuration;
using DaysOn.Common;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Common.Events.ResultEvents;
using DaysOn.Common.MessageHandlers;
using DaysOn.Functions.Api.BusEventHandlers;
using log4net;
using log4net.Config;
using NUnit.Framework;

namespace DaysOn.Functions.Test.Acceptance.BusEventHandlers
{
    [TestFixture]
    public class CalculateBusinessDaysEventHandlerTests : EventTestBase
    {
        MockMessageReader _mockMessageReader;
        MockResultMessageReader _mockResultMessageReader;

        [Test]
        public void PubNubWhenIExecuteCalculateBusinessDays_ThenIGetTheResultInMessageBus()
        {
            XmlConfigurator.Configure();
            ILog _logger = LogManager.GetLogger("Test CalculateBusinessDaysEventHandlerTests");

            Guid channelId = new Guid("99365ba9-c0cc-4988-9aa4-b8ace058544a");
            const int numDays = 5;

            _mockMessageReader = new MockMessageReader();
            IPubnubMessageBus bus = new PubnubMessageBus(_logger, ConfigurationManager.AppSettings["Enviroment"]).Start();
            bus.Subscribe(_mockMessageReader, channelId);

            CalculateBusinessDaysEventHandler handler = new CalculateBusinessDaysEventHandler(bus);
            handler.Handle(new CalculateBusinessDaysEvent()
            {
                From = DateTime.Now,
                To = DateTime.Now.AddDays(numDays),
                SubscriberId = channelId,
                NonWorkingDays = new List<DateTime>(),
                NonWorkingHalfDays = new List<DateTime>(),
                WorkingDaysOfTheWeek = new Dictionary<DayOfWeek, bool>()
                {
                    {DayOfWeek.Monday, true},
                    {DayOfWeek.Tuesday, true},
                    {DayOfWeek.Wednesday, true},
                    {DayOfWeek.Thursday, true},
                    {DayOfWeek.Friday, true},
                    {DayOfWeek.Saturday, false},
                    {DayOfWeek.Sunday, false},
                }
            });

            int timeEllapsed;
            var result = WaitFor(() => _mockMessageReader.numCalls == 1, 10000, out timeEllapsed);

            Console.WriteLine($"time elapsed = {timeEllapsed} milliseconds");
            Assert.IsTrue(result);
        }

        [Test]
        public void PubNubWhenIExecuteCalculateBusinessDays_ThenIGetTheRightResultInMessageBus()
        {
            XmlConfigurator.Configure();
            ILog _logger = LogManager.GetLogger("Test CalculateBusinessDaysEventHandlerTests with Data");

            Guid channelId = new Guid("99365ba9-c0cc-4988-9aa4-b8ace058544a");
            const int numDays = 5;

            _mockResultMessageReader = new MockResultMessageReader();
            IPubnubMessageBus bus = new PubnubMessageBus(_logger, ConfigurationManager.AppSettings["Enviroment"]);
            bus.Subscribe(_mockResultMessageReader, channelId);

            CalculateBusinessDaysEventHandler handler = new CalculateBusinessDaysEventHandler(bus);
            handler.Handle(new CalculateBusinessDaysEvent()
            {
                From = new DateTime(2018, 3, 5),
                To = new DateTime(2018, 3, 15),
                SubscriberId = channelId,
                NonWorkingDays = GetNonWorkingDays(),
                NonWorkingHalfDays = GetNonWorkingHalfDays(),
                WorkingDaysOfTheWeek = GetWorkingDaysOfTheWeek()
            });

            int timeEllapsed;
            var result = WaitFor(() => _mockResultMessageReader.Result == 6.5, 10000, out timeEllapsed);

            Console.WriteLine($"time elapsed = {timeEllapsed} milliseconds");
            Assert.IsTrue(result);
        }

        private Dictionary<DayOfWeek, bool> GetWorkingDaysOfTheWeek()
        {
            var result = new Dictionary<DayOfWeek, bool>();
            result.Add(DayOfWeek.Monday, true);
            result.Add(DayOfWeek.Tuesday, true);
            result.Add(DayOfWeek.Wednesday, true);
            result.Add(DayOfWeek.Thursday, true);
            result.Add(DayOfWeek.Friday, true);
            result.Add(DayOfWeek.Saturday, false);
            result.Add(DayOfWeek.Sunday, false);
            return result;
        }

        private List<DateTime> GetNonWorkingHalfDays()
        {
            return new List<DateTime>
            {
                new DateTime(2018,1, 10),
                new DateTime(2018,3, 15),
                new DateTime(2018,4, 7),
                new DateTime(2018,10, 10),
            };
        }

        private List<DateTime> GetNonWorkingDays()
        {
            return new List<DateTime>
            {
                new DateTime(2018,2, 28),
                new DateTime(2018,3, 5),
                new DateTime(2018,3, 9),
                new DateTime(2018,3, 10),
                new DateTime(2018,4, 1),
                new DateTime(2018,9, 10),
            };
        }
    }

    public class MockMessageReader : IMessageHandler<DoubleResultEvent>
    {
        public int numCalls { get; set; }

        public MockMessageReader()
        {
            numCalls = 0;
        }

        public bool Handle(DoubleResultEvent message)
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
}