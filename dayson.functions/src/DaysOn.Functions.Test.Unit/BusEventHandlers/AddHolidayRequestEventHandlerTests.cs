using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DaysOn.Common;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Common.Events.ResultEvents;
using DaysOn.Functions.Api.BusEventHandlers;
using DaysOn.Functions.BoundedContext.Services;
using DaysOn.Functions.BoundedContext.Value;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using log4net.Config;
using Moq;
using NUnit.Framework;

namespace DaysOn.Functions.Test.Unit.BusEventHandlers
{
    [TestFixture()]
    public class AddHolidayRequestEventHandlerTests
    {
        private Mock<IPubnubMessageBus> _bus;
        private MockCreateHolidayService _createHolidayService;
        [SetUp]
        public void Initialize()
        {
            _bus = new Mock<IPubnubMessageBus>();
            _bus.Setup(x => x.SetSecretKey()).Returns(_bus.Object);
            _bus.Setup(x => x.Start()).Returns(_bus.Object);
        }

        [Test]
        public void WhenAddingHolidayRequestWithInvalidData_ThenIGetTheErrornMessageBus()
        {
            XmlConfigurator.Configure();
            var correlationId = Guid.NewGuid();
            _createHolidayService = new MockCreateHolidayService(true, false, true, true)
            {
                CorrelationId = correlationId
            };
            var errorMessage = "errorValidation";
            AddHolidayRequestEventHandler handler = new AddHolidayRequestEventHandler(_createHolidayService,_bus.Object);

            var subscriberId = Guid.NewGuid();
            handler.Handle(new AddHolidayRequestEvent()
            {
                From = new DateTime(2022, 1, 21),
                To = new DateTime(2022, 1, 23, 12, 0, 0),
                SubscriberId = subscriberId,
                EmployeeId = Guid.NewGuid(),
                CorrelationId = correlationId,
                CompanyId = Guid.NewGuid(),
                Notes = "Test notes"
            }).Wait();

            _bus.Verify(x => x.Publish(It.IsAny<BaseResultEvent>(),It.IsAny<string>()));
        }

        [Test]
        [TestCase(true, true, true, "errorCompanyRules")]
        [TestCase(true, false, true, "errorCompanyRules")]
        [TestCase(true, true, false, "errorCompanyRules")]
        [TestCase(false, true, false, "errorTeamRules")]
        [TestCase(false, true, true, "errorTeamRules")]
        [TestCase(false, false, true, "errorSystemRules")]
        public void WhenAddingHolidayRequestWithInvalidCompanyRules_ThenIGetTheErrornMessageBus(bool companyError, bool teamError, bool systemError, string expectedMessage)
        {
            _bus.ResetCalls();
            XmlConfigurator.Configure();
            var correlationId = Guid.NewGuid();
            _createHolidayService = new MockCreateHolidayService(false, companyError, teamError, systemError)
            {
                CorrelationId = correlationId
            };

            AddHolidayRequestEventHandler handler = new AddHolidayRequestEventHandler(_createHolidayService, _bus.Object);

            var subscriberId = Guid.NewGuid();
            handler.Handle(new AddHolidayRequestEvent()
            {
                From = new DateTime(2022, 1, 21),
                To = new DateTime(2022, 1, 23, 12, 0, 0),
                SubscriberId = subscriberId,
                EmployeeId = Guid.NewGuid(),
                CorrelationId = correlationId,
                CompanyId = Guid.NewGuid(),
                Notes = "Test notes"
            }).Wait();

            _bus.Verify(x => x.Publish(It.Is<BaseResultEvent>(y => !y.Ok && y.CorrelationId == correlationId && y.Errors[0] == expectedMessage), subscriberId.ToString()));
        }

        [Test]
        public void WhenAddingHolidayRequestWithValidArguments_ThenIGetMessageBus()
        {
            _bus.ResetCalls();
            XmlConfigurator.Configure();
            var correlationId = Guid.NewGuid();
            _createHolidayService = new MockCreateHolidayService(false, false, false, false)
            {
                CorrelationId = correlationId
            };

            AddHolidayRequestEventHandler handler = new AddHolidayRequestEventHandler(_createHolidayService, _bus.Object);

            var subscriberId = Guid.NewGuid();
            handler.Handle(new AddHolidayRequestEvent()
            {
                From = new DateTime(2022, 1, 21),
                To = new DateTime(2022, 1, 23, 12, 0, 0),
                SubscriberId = subscriberId,
                EmployeeId = Guid.NewGuid(),
                CorrelationId = correlationId,
                CompanyId = Guid.NewGuid(),
                Notes = "Test notes"
            }).Wait();

            _bus.Verify(x => x.Publish(It.Is<BaseResultEvent>(y => y.Ok && y.CorrelationId == correlationId && y.Errors == null), subscriberId.ToString()));
        }
    }

    public class MockCreateHolidayService : ICreateHolidayService
    {
        private readonly bool _validationError;
        private readonly bool _errorSystemRules;
        private readonly bool _errorCompanyRules;
        private readonly bool _errorTeamRules;
        public Guid CorrelationId { get; set; }

        private BaseResultEvent OkFunctionResponse { get ; set; }

        public MockCreateHolidayService(bool validationError, bool errorCompanyRules, bool errorTeamRules, bool errorSystemRules)
        {
            _validationError = validationError;
            _errorSystemRules = errorSystemRules;
            _errorCompanyRules = errorCompanyRules;
            _errorTeamRules = errorTeamRules;
            OkFunctionResponse = new BaseResultEvent() {Ok = true};
        }
    

        public Task<ValidateAddHolidaysResponse> Validate(AddHolidayRequestEvent command)
        {
            if (_validationError)
            {
                return Task.FromResult(new ValidateAddHolidaysResponse
                {
                    Ok = false,
                    Errors = new List<string>() { "errorValidation" },
                    CorrelationId = CorrelationId
                });
            }
            return Task.FromResult(new ValidateAddHolidaysResponse
            {
                Ok = true
            });

        }

        public GetBusinessDaysResponse GetBusinessDays(AddHolidayRequestEvent message, ValidateAddHolidaysResponse validationResponse)
        {
            return new GetBusinessDaysResponse() {Ok = true, Days = 2, CorrelationId = CorrelationId};
        }

        public bool GetNonValidatedCompanyRules(AddHolidayRequestEvent message, out BaseResultEvent response)
        {
            response = null;
            if (_errorCompanyRules)
            {
                response = new BaseResultEvent()
                {
                    Ok = false,
                    Errors = new List<string>() { "errorCompanyRules" },
                    CorrelationId = CorrelationId
                };
                return false;
            }

            return true;
        }

        public bool GetNonValidatedTeamRules(AddHolidayRequestEvent message, out BaseResultEvent response)
        {
            response = null;
            if (_errorTeamRules)
            {
                response = new BaseResultEvent()
                {
                    Ok = false,
                    Errors = new List<string>() { "errorTeamRules" },
                    CorrelationId = CorrelationId
                };
                return false;
            }

            return true;
        }

        public BaseResultEvent UpdateCalculation(Guid employeeId, int year)
        {
            return OkFunctionResponse;
        }

        public bool GetNonValidatedSystemRules(AddHolidayRequestEvent message, out BaseResultEvent response, decimal numDays)
        {
            response = null;
            if (_errorSystemRules)
            {
                response = new BaseResultEvent()
                {
                    Ok = false,
                    Errors = new List<string>() { "errorSystemRules" },
                    CorrelationId = CorrelationId
                };
                return false;
            }

            return true;
        }

        public Task<HolidayRequest> CreateHolidayRequest(AddHolidayRequestEvent command, decimal numDays, DateTime yearStartDay)
        {
            return Task.FromResult(new HolidayRequest(Guid.NewGuid())
            {
                Year = 2015
            });
        }
    }
}