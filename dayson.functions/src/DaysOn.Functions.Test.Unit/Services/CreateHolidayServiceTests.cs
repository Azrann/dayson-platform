using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Functions.BoundedContext.Services;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using Moq;
using NUnit.Framework;

namespace DaysOn.Functions.Test.Unit.Services
{
    [TestFixture]
    public class HolidaysBrainControllerTests 
    {
        CreateHolidayService _createHolidayService;
        Mock<IEmployeeService> _employeeService;
        Mock<IOpenClosedDatesService> _openClosedDatesService;
        Mock<ICompanyGlobalRulesService> _globalRulesService;
        Mock<IHolidayService> _holidayService;
        private Mock<ICalculationService> _calculationService;
        private Guid _employeeId;
        private Guid _companyId;

        [SetUp]
        public void Initialize()
        {
            _employeeId = Guid.NewGuid();
            _companyId = Guid.NewGuid();
            _employeeService = new Mock<IEmployeeService>();
            _openClosedDatesService = new Mock<IOpenClosedDatesService>();
            _globalRulesService = new Mock<ICompanyGlobalRulesService>();
            _holidayService = new Mock<IHolidayService>();
            _calculationService = new Mock<ICalculationService>();
            _createHolidayService = new CreateHolidayService(_employeeService.Object, _openClosedDatesService.Object,
            _globalRulesService.Object, _holidayService.Object, _calculationService.Object);
        }

        [Test]
        public async Task WhenIValidateHolidayRequestBetween2Years_IReceiveAnErrorAsResponse()
        {
            _globalRulesService.Setup(x => x.GetGlobalRules(_companyId)).Returns(GetGlobalRules(_companyId));
            var command = new AddHolidayRequestEvent()
            {
                EmployeeId = _employeeId,
                CompanyId = _companyId,
                CorrelationId = Guid.NewGuid(),
                SubscriberId = Guid.NewGuid(),
                From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(50),
                To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(75),
            };

            var response = await _createHolidayService.Validate(command);
            Assert.AreEqual("You cannot ask for holidays between years you will need to create two separate holiday request.", response.Errors.First());
        }

        [TestCase(29, 30)]
        [TestCase(30, 31)]
        [TestCase(30, 35)]
        [TestCase(39, 40)]
        [TestCase(40, 41)]
        public async Task WhenIValidateHolidayRequestThatOverlapsOtherHolidays_IReceiveAnErrorAsResponse(int from, int to)
        {
            var command = new AddHolidayRequestEvent()
            {
                EmployeeId = _employeeId,
                CompanyId = _companyId,
                CorrelationId = Guid.NewGuid(),
                SubscriberId = Guid.NewGuid(),
                From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(30),
                To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(40),
            };

            _globalRulesService.Setup(x => x.GetGlobalRules(_companyId)).Returns(GetGlobalRules(_companyId));
            _holidayService.Setup(x => x.GetHolidaysRequestsByYear(_employeeId, command.From.Year)).ReturnsAsync(GetHolidaysRequest(from, to));


            var response = await _createHolidayService.Validate(command);
            Assert.AreEqual("You cannot ask for holidays that overlaps other holidays already requested.", response.Errors.First());
        }

        [TestCase(41, 42)]
        [TestCase(28, 29)]
        public async Task WhenIValidateHolidayRequest_IReturnOpenClosedDates(int from, int to)
        {
            var command = new AddHolidayRequestEvent()
            {
                EmployeeId = _employeeId,
                CompanyId = _companyId,
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

            _globalRulesService.Setup(x => x.GetGlobalRules(_companyId)).Returns(GetGlobalRules(_companyId));
            _holidayService.Setup(x => x.GetHolidaysRequestsByYear(_employeeId, command.From.Year)).ReturnsAsync(GetHolidaysRequest(from, to));
            _openClosedDatesService.Setup(x => x.GetAllByYear(_companyId, command.From.Year)).Returns(openClosedDates);
            _employeeService.Setup(x => x.GetEmployee(_employeeId, _companyId)).Returns(new Employee(_employeeId)
            {
            });

            var response = await _createHolidayService.Validate(command);
            Assert.AreEqual(openClosedDates.ClosedDays, response.NonWorkingDays);
            Assert.AreEqual(openClosedDates.ClosedHalfDays, response.NonWorkingHalfDays);
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

        private GlobalRules GetGlobalRules(Guid companyId)
        {
            GlobalRules rules = new GlobalRules(Guid.NewGuid(), companyId);
            var workingDays = rules.WorkingDaysOfTheWeek;
            rules.YearStartDay = DateTime.Now.AddMonths(2);
            return rules;
        }
    }
}