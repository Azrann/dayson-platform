using System;
using System.Collections.Generic;
using DaysOn.Common;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Common.Events.ResultEvents;
using DaysOn.Functions.Api.BusEventHandlers;
using log4net.Config;
using Moq;
using NUnit.Framework;

namespace DaysOn.Functions.Test.Unit.BusEventHandlers
{
    [TestFixture()]
    public class CalculateBusinessDaysEventHandlerTests
    {
        Mock<IPubnubMessageBus> _bus;

        [SetUp]
        public void Initialize()
        {
            _bus = new Mock<IPubnubMessageBus>();
            _bus.Setup(x => x.SetSecretKey()).Returns(_bus.Object);
            _bus.Setup(x => x.Start()).Returns(_bus.Object);
        }

        [Test]
        public void PubNubWhenIExecuteCalculateBusinessDays_ThenIGetTheResultInMessageBus()
        {
            XmlConfigurator.Configure();

            CalculateBusinessDaysEventHandler handler = new CalculateBusinessDaysEventHandler(_bus.Object);
            handler.Handle(new CalculateBusinessDaysEvent()
            {
                From = new DateTime(2022,1,21),
                To = new DateTime(2022, 1, 23,12,0,0),
                SubscriberId = Guid.NewGuid(),
                NonWorkingDays = new List<DateTime>(),
                NonWorkingHalfDays = new List<DateTime>(),
                WorkingDaysOfTheWeek = GetWorkingDaysOfTheWeek()
            });
           
            _bus.Verify(x => x.Publish(It.Is<DoubleResultEvent>(y => y.Result == 1), It.IsAny<string>()));
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
    }
}