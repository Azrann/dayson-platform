using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.HolidaysBc.Models.Commands;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Common.AzureBus;

namespace DaysOn.HolidaysBc.Api.Controllers.Tests
{
    [TestFixture]
    public class HolidaysBrainControllerTests : BaseControllerTest
    {
        HolidaysBrainController _controller;
        Mock<IServiceBus> _serviceBus;
        Mock<IEmployeeService> _employeeService;
        Mock<IOpenClosedDatesService> _openClosedDatesService;
        Mock<ICompanyGlobalRulesService> _globalRulesService;
        Mock<IHolidayService> _holidayService;

        [SetUp]
        public void Initialize()
        {
            _serviceBus = new Mock<IServiceBus>();
            _employeeService = new Mock<IEmployeeService>();
            _openClosedDatesService = new Mock<IOpenClosedDatesService>();
            _globalRulesService = new Mock<ICompanyGlobalRulesService>();
            _holidayService = new Mock<IHolidayService>();
            _controller = new HolidaysBrainController(_employeeService.Object, _serviceBus.Object,
                _openClosedDatesService.Object,
                _globalRulesService.Object, _holidayService.Object) {User = _principal.Object};
        }

        [Test]
        public async Task WhenIRequestHolidayBetween2Years_IReceiveAnErrorAsResponse()
        {
            _globalRulesService.Setup(x => x.GetGlobalRules(_companyId)).Returns(GetGlobalRules());
            var command = new GetBusinessDaysCommand()
            {
                Id = Guid.NewGuid(),
                CorrelationId = Guid.NewGuid(),
                SubscriberId = Guid.NewGuid(),
                From = DateTime.Now.AddDays(50),
                To = DateTime.Now.AddDays(75)
            };

            var response = await _controller.Get(command);
            var contentResult = response as System.Web.Http.Results.BadRequestErrorMessageResult;
            Assert.AreEqual("You cannot ask for holidays between years you will need to create two separate holiday request.", contentResult.Message);
        }

        [TestCase(29, 30)]
        [TestCase(30, 31)]
        [TestCase(30, 35)]
        [TestCase(39, 40)]
        [TestCase(40, 41)]
        public async Task WhenIRequestHolidayThatOverlapsOtherHolidays_IReceiveAnErrorAsResponse(int from, int to)
        {
            var command = new GetBusinessDaysCommand()
            {
                Id = Guid.NewGuid(),
                CorrelationId = Guid.NewGuid(),
                SubscriberId = Guid.NewGuid(),
                From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(30),
                To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(40),
            };

            _globalRulesService.Setup(x => x.GetGlobalRules(_companyId)).Returns(GetGlobalRules());
            _holidayService.Setup(x => x.GetHolidaysRequestsByYear(_employeeId, command.From.Year)).ReturnsAsync(GetHolidaysRequest(from, to));


            var response = await _controller.Get(command);
            var contentResult = response as System.Web.Http.Results.BadRequestErrorMessageResult;
            Assert.AreEqual("You cannot ask for holidays that overlaps other holidays already requested.", contentResult.Message);
        }

        [TestCase(41, 42)]
        [TestCase(28, 29)]
        public async Task WhenIRequestHoliday_ISentAMessageWithOpenClosedDates(int from, int to)
        {
            var command = new GetBusinessDaysCommand()
            {
                Id = Guid.NewGuid(),
                CorrelationId = Guid.NewGuid(),
                SubscriberId = Guid.NewGuid(),
                From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(30),
                To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(40),
            };
            var openClosedDates = new OpenClosedDates
            {
                ClosedDays = new List<DateTime> { DateTime.Now },
                ClosedHalfDays = new List<DateTime> { DateTime.Now.AddDays(2) }
            };

            _globalRulesService.Setup(x => x.GetGlobalRules(_companyId)).Returns(GetGlobalRules());
            _holidayService.Setup(x => x.GetHolidaysRequestsByYear(_employeeId, command.From.Year)).ReturnsAsync(GetHolidaysRequest(from, to));
            _openClosedDatesService.Setup(x => x.GetAllByYear(_companyId, command.From.Year)).Returns(openClosedDates);
            _employeeService.Setup(x => x.GetEmployee(_employeeId, _companyId)).Returns(new Employee(_employeeId)
            {
            });

            var response = await _controller.Get(command);
            _serviceBus.Verify(y => y.Publish(It.Is<CalculateBusinessDaysEvent>(
                x => x.NonWorkingDays == openClosedDates.ClosedDays &&
                     x.NonWorkingHalfDays == openClosedDates.ClosedHalfDays)));
        }

        private AddHolidayRequestCommand GetAddHolidayRequest()
        {
            return new AddHolidayRequestCommand
            {
                CorrelationId = Guid.NewGuid(),
                SubscriberId = Guid.NewGuid(),
                From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(10),
                To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(15),
                Id = Guid.NewGuid(),
                Notes = "Test Notes"
            };
        }

        private List<HolidayRequest> GetHolidaysRequest(int from, int to)
        {
            return new List<HolidayRequest> {
                new HolidayRequest(Guid.NewGuid()) {
                     From = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day).AddDays(from),
                     To =  new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day).AddDays(to)
                }
            };
        }

        private GlobalRules GetGlobalRules()
        {
            GlobalRules rules = new GlobalRules(Guid.NewGuid(), _companyId);
            var workingDays = rules.WorkingDaysOfTheWeek;
            rules.YearStartDay = DateTime.Now.AddMonths(2);
            return rules;
        }
    }

    internal class ErrorResult
    {
        public string Message { get; set; }
    }
}