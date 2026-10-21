using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DaysOn.Common;
using DaysOn.Common.Events.ResultEvents;
using DaysOn.Common.MessageHandlers;
using DaysOn.Facade.Common;
using DaysOn.Facade.Tests.Acceptance.Helper;
using DaysOn.HolidaysBc.Models.Commands;
using DaysOn.HolidaysBc.Models.Dtos;
using log4net;
using log4net.Config;
using NUnit.Framework;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class FacadeBrainHolidayControllerTest : EventTestBase
    {
        [SetUp]
        public void Initialize()
        {
            TokenResponse tokenAdmin;
            var companyName = CompanyFactory.CreateCompany(out tokenAdmin);

            var newGlobalRules = new GlobalRulesDto
            {
                DefaultNumberOfHolidays = 5,
                MaxConsecutiveDaysOff = 5,
                MaxDaysCarriedOverNextYear = 2,
                MinDaysAdvanceNoticeHolidays = 3,
                YearStartDay = new DateTime(2020, 1, 1),
                WorkingDaysOfTheWeek = Factory.GetWorkingDays()
            };
            FacadeClientWrapper.UpdateCompanyGlobalRulesAsync(newGlobalRules, tokenAdmin.access_token)
                .Result.GetGlobalRuleDto();
            Thread.Sleep(150);
            var employeeUserEmail = EmployeeFactory.CreateEmployee(companyName, tokenAdmin, out _employeeId,"User");
            Thread.Sleep(150);
            Assert.AreNotEqual(Guid.Empty, _employeeId);

            _tokenUser = AuthClientWrapper.GetTokenAsync(employeeUserEmail, _employeeId.GetHashCode().ToString()).Result;

            var closedDates =
                FacadeOpenCloseDatesClientWrapper.AddClosedDays(Factory.GetAddDaysCommandJanuary(2020),
                    tokenAdmin.access_token).Result;
            var recurrentClosed =
                FacadeOpenCloseDatesClientWrapper.AddRecurrentClosedDays(Factory.GetRecurrentDaysCommandJanuary(),
                    tokenAdmin.access_token).Result;
            Thread.Sleep(100);
        }

        [TearDown]
        public void CleanUp()
        {
        }

        private Guid _employeeId;
        private TokenResponse _tokenUser;
        private MockMessageReader _mockMessageReader;
        private MockFunctionResponseMessageReader _mockFunctionResponseMessageReader;

        [Test]
        public async Task WhenIAddHolidayRequest_IReceiveResponse()
        {
            XmlConfigurator.Configure();
            var logger = LogManager.GetLogger("DaysOn.Facade AcceptanceTest AddHolidayRequestEventHandlerTests");

           _mockFunctionResponseMessageReader = new MockFunctionResponseMessageReader();

            var bus = new PubnubMessageBus(logger, ConfigurationManager.AppSettings["Enviroment"]).SetAuth(_tokenUser.access_token).Start();
            bus.Subscribe(_mockFunctionResponseMessageReader,_employeeId.ToString());

            var command = new AddHolidayRequestCommand
            {
                CorrelationId = Guid.NewGuid(),
                SubscriberId = _employeeId,
                From = new DateTime(2020,1, 1),
                To = new DateTime(2020,1, 11),
                Id = Guid.NewGuid(),
                Notes = "Test Notes"
            };

            var response = await FacadeClientWrapper.AddHolidayRequestCommandAsync(command, _tokenUser.access_token);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            int timeEllapsed;
            WaitFor(() => _mockFunctionResponseMessageReader.Errors != null, 10000, out timeEllapsed);

            Console.WriteLine($"time elapsed = {timeEllapsed} milliseconds");
            Assert.IsFalse(_mockFunctionResponseMessageReader.Ok);
            Assert.AreEqual("You only have 5.00 days left and this request requires 5.5",_mockFunctionResponseMessageReader.Errors.First());
            command.To = command.To.AddDays(-2);
            response = await FacadeClientWrapper.AddHolidayRequestCommandAsync(command, _tokenUser.access_token);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            WaitFor(() => _mockFunctionResponseMessageReader.Ok, 10000, out timeEllapsed);

            Console.WriteLine($"time elapsed = {timeEllapsed} milliseconds");
            Assert.IsTrue(_mockFunctionResponseMessageReader.Ok);
        }

        [Test]
        public async Task WhenIRequestNumberOfBusinessDays_IReceiveRightAsResponse()
        {
            XmlConfigurator.Configure();
            var logger = LogManager.GetLogger("DaysOn.Facade AcceptanceTest CalculateBusinessDaysEventHandlerTests");

            var subscriberIdString = "79e265ec-b842-481d-8c7b-0d35319cb786";
            var subscriberId = new Guid(subscriberIdString);
            const double numDays = 6.5;

            _mockMessageReader = new MockMessageReader();
            var bus = new PubnubMessageBus(logger, ConfigurationManager.AppSettings["Enviroment"]).SetAuth(_tokenUser.access_token).Start();
            bus.Subscribe(_mockMessageReader, _employeeId.ToString());

            var command = new GetBusinessDaysCommand
            {
                Id = Guid.NewGuid(),
                CorrelationId = Guid.NewGuid(),
                SubscriberId = subscriberId,
                From = new DateTime(2022, 1, 3),
                To = new DateTime(2022, 1, 12, 12, 0, 0)
            };

            var response = await FacadeClientWrapper.GetBusinessDaysCommandAsync(command, _tokenUser.access_token);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            int timeEllapsed;
            var result = WaitFor(() => _mockMessageReader.Result > 1, 10000, out timeEllapsed);

            Console.WriteLine($"time elapsed = {timeEllapsed} milliseconds");
            Assert.AreEqual(numDays, _mockMessageReader.Result);
        }
    }

    public class MockMessageReader : IMessageHandler<DoubleResultEvent>
    {
        public MockMessageReader()
        {
            Result = 0;
        }

        public double Result { get; set; }


        bool IMessageHandler<DoubleResultEvent>.Handle(DoubleResultEvent message)
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

    public class MockFunctionResponseMessageReader : IMessageHandler<BaseResultEvent>
    {
        public bool Ok { get; set; }
        public List<string> Errors { get; set; }
        public Guid CorrelationId { get; set; }

        public bool Handle(BaseResultEvent message)
        {
            Ok = message.Ok;
            Errors = message.Errors;
            CorrelationId = message.CorrelationId;
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