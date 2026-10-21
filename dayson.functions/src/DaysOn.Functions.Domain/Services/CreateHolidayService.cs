using DaysOn.Functions.BoundedContext.Value;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using TimeRange = Itenso.TimePeriod.TimeRange;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Common.Events.ResultEvents;
using DaysOn.Common.Utilities.Extension;
using DaysOn.HolidaysBc.BoundedContext.Value;

namespace DaysOn.Functions.BoundedContext.Services
{
    [CastleComponent(typeof(ICreateHolidayService))]
    public class CreateHolidayService : ICreateHolidayService
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICompanyGlobalRulesService _globalRulesService;
        private readonly IHolidayService _holidayService;
        private readonly ICalculationService _calculationService;
        private readonly IOpenClosedDatesService _openClosedDatesService;

        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CreateHolidayService(IEmployeeService employeeService, IOpenClosedDatesService openClosedDatesService,
            ICompanyGlobalRulesService globalRulesService,
            IHolidayService holidayService, ICalculationService calculationService)
        {
            _employeeService = employeeService;
            _openClosedDatesService = openClosedDatesService;
            _globalRulesService = globalRulesService;
            _holidayService = holidayService;
            _calculationService = calculationService;
        }

        public async Task<ValidateAddHolidaysResponse> Validate(AddHolidayRequestEvent command)
        {
            var response = new ValidateAddHolidaysResponse();
            _logger.LogInf($"CreateHolidayService Validate called with message {JsonConvertExt.SerializeObject(command)}");

            Employee employee = _employeeService.GetEmployee(command.EmployeeId, command.CompanyId);

            var globalRules = _globalRulesService.GetGlobalRules(command.CompanyId);

            var startDayOfTheYear = new DateTime(command.From.Year, globalRules.YearStartDay.Month,
                globalRules.YearStartDay.Day);

            //TODO We should be able to split this holiday request into two and tell show the result to the user.
            if (BetweenDates(startDayOfTheYear,command.From, command.To) || BetweenDates(startDayOfTheYear.AddYears(1), command.From, command.To))
            {
                response.Errors = new List<string>
                {
                    "You cannot ask for holidays between years you will need to create two separate holiday request."
                };
                return response;
            }

            var holidays = await _holidayService.GetHolidaysRequestsByYear(command.EmployeeId, command.From.Year);
            DateTime virtualTo = command.To.Hour == 0 ? command.To.AddDays(1) : command.To;
            TimeRange commandTimeRange = new TimeRange(command.From, virtualTo);
            
            if (holidays.Any(x => TimePeriodExtensions.GetTimeRange(x).OverlapsWith(commandTimeRange)))
            {
                response.Errors = new List<string>
                {
                    "You cannot ask for holidays that overlaps other holidays already requested."
                };
                return response;
            } 

            var openClosedDates = _openClosedDatesService.GetAllByYear(command.CompanyId, command.From.Year);

            response.WorkingDaysOfTheWeek = employee.WorkingDaysOfTheWeek;
            response.NonWorkingDays = openClosedDates.ClosedDays;
            response.NonWorkingHalfDays = openClosedDates.ClosedHalfDays;
            response.YearStartDay = employee.YearStartDay;
            response.Ok = true;
            return response;
        }

        public bool BetweenDates(DateTime date, DateTime from, DateTime to)
        {
            return (date > from & date < to);
        }

        public GetBusinessDaysResponse GetBusinessDays(AddHolidayRequestEvent message,
            ValidateAddHolidaysResponse validationResponse)
        {
            _logger.LogInf(
                $"CreateHolidayService GetBusinessDays called with message {JsonConvertExt.SerializeObject(message)}");

            var nonWorkingDays = WorkingDaysHelper.GetTotalBankHolidaysBetweenDates(validationResponse.NonWorkingDays,
                message.From, message.To);
            var nonWorkingHalfDays =
                WorkingDaysHelper.GetTotalBankHolidaysBetweenDates(validationResponse.NonWorkingHalfDays, message.From,
                    message.To);

            nonWorkingDays = WorkingDaysHelper.FilterWorkingDaysOfTheWeek(validationResponse.WorkingDaysOfTheWeek,
                nonWorkingDays);
            nonWorkingHalfDays = WorkingDaysHelper.FilterWorkingDaysOfTheWeek(validationResponse.WorkingDaysOfTheWeek,
                nonWorkingHalfDays);

            var days =
                WorkingDaysHelper.GetWorkingDays(message.From, message.To, validationResponse.WorkingDaysOfTheWeek) -
                nonWorkingDays.Count - (decimal) (nonWorkingHalfDays.Count / 2.0);
            return new GetBusinessDaysResponse
            {
                Ok = true,
                Days = days
            };
        }

        public bool GetNonValidatedSystemRules(AddHolidayRequestEvent message, out BaseResultEvent response, decimal numDays)
        {
            var calculationResult =_calculationService.GetCalculationsByYearCreateIfNotExists(message.EmployeeId, message.From.Year);
            if (numDays > calculationResult.NumberOfDaysLeft)
            {
                response = new BaseResultEvent()
                {
                    Ok = false,
                    CorrelationId = message.CorrelationId,
                    Errors = new List<string> { $"You only have {calculationResult.NumberOfDaysLeft} days left and this request requires {numDays}" }
                };
                return false;
            }
            response = new BaseResultEvent
            {
                Ok = true
            };
            return true;
        }

        public bool GetNonValidatedCompanyRules(AddHolidayRequestEvent message, out BaseResultEvent response)
        {
            response = new BaseResultEvent
            {
                Ok = true
            };
            return true;
        }

        public bool GetNonValidatedTeamRules(AddHolidayRequestEvent message, out BaseResultEvent response)
        {
            response = new BaseResultEvent
            {
                Ok = true
            };
            return true;
        }

        public async Task<HolidayRequest> CreateHolidayRequest(AddHolidayRequestEvent command, decimal numDays, DateTime yearStartDay)
        {
            var dateNow = DateTime.Now;
            var holidayRequest = new HolidayRequest(command.Id)
            {
                CreatedOn = dateNow,
                LastUpdated = dateNow,
                EmployeeId = command.EmployeeId,
                From = command.From,
                To = command.To,
                Notes = command.Notes,
                NumberOfDays = numDays,
                Status = GetStatus(command.Status)
            };
            holidayRequest.SetYear(yearStartDay);
            await _holidayService.AddHolidayRequest(holidayRequest, command.EmployeeId);

            return holidayRequest;
        }

        private HolidaysBc.BoundedContext.Entities.Status GetStatus(Common.Events.Dtos.Status commandStatus)
        {
            HolidaysBc.BoundedContext.Entities.Status result = 0;
            if (Enum.TryParse(commandStatus.ToString(), true, out result))
            {
                return result;
            }

            _logger.LogErr($"Error parsing Status {commandStatus.ToString()} to Status");
            return HolidaysBc.BoundedContext.Entities.Status.Requested;
        }
        
        public BaseResultEvent UpdateCalculation(Guid employeeId, int year)
        {
            try
            {
                var totalHolidays = _holidayService.GetSumHolidaysPerYear(employeeId, year);
                _calculationService.UpdateNumberOfDaysLeft(employeeId, year, totalHolidays);
                return new BaseResultEvent()
                {
                    Ok = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResultEvent()
                {
                    Errors = new List<string> { ex.Message},
                    Ok = false
                };
            }
            
        }
    }
}
