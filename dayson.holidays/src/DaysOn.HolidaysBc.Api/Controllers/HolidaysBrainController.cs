using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.HolidaysBc.Models.Commands;
using log4net;
using DaysOn.Common.Events.FunctionsEvents;
using System;
using System.Linq;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Utilities.Extension;
using DaysOn.HolidaysBc.BoundedContext.Value;
using Itenso.TimePeriod;

namespace DaysOn.HolidaysBc.Api.Controllers
{
    [RoutePrefix("v1")]
    public class HolidaysBrainController : ApiControllerBase
    {
        private readonly IServiceBus _serviceBus;
        private readonly IEmployeeService _employeeService;
        private readonly IOpenClosedDatesService _openClosedDatesService;
        private readonly ICompanyGlobalRulesService _globalRulesService;
        private readonly IHolidayService _holidayService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public HolidaysBrainController(IEmployeeService employeeService, IServiceBus serviceBus, 
            IOpenClosedDatesService openClosedDatesService, ICompanyGlobalRulesService globalRulesService,
            IHolidayService holidayService)
        {
            _serviceBus = serviceBus;
            _employeeService = employeeService;
            _openClosedDatesService = openClosedDatesService;
            _globalRulesService = globalRulesService;
            _holidayService = holidayService;
        }

        [Route("brain/businessDays")]
        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> Get(GetBusinessDaysCommand command)
        {
            var employeeId = GetEmployeeId();
            var companyId = GetCompanyId();

            var employee = _employeeService.GetEmployee(employeeId, companyId);

            _logger.LogInf($"GetBusiness days called for employee {employeeId}");
            var globalRules = _globalRulesService.GetGlobalRules(companyId);
            var startDayOfTheYear = new DateTime(command.From.Year, globalRules.YearStartDay.Month, globalRules.YearStartDay.Day);

            //// We should be able to split this holiday request into two and tell show the result to the user.
            if (BetweenDates(startDayOfTheYear, command.From, command.To) || BetweenDates(startDayOfTheYear.AddYears(1), command.From, command.To))
            {
                return BadRequest("You cannot ask for holidays between years you will need to create two separate holiday request.");
            }

            var holidays = await _holidayService.GetHolidaysRequestsByYear(employeeId, command.From.Year);
            DateTime virtualTo = command.To.Hour == 0 ? command.To.AddDays(1) : command.To;
            TimeRange commandTimeRange = new TimeRange(command.From, virtualTo);

            if (holidays.Any(x => x.GetTimeRange().OverlapsWith(commandTimeRange)))
            {
                return BadRequest("You cannot ask for holidays that overlaps other holidays already requested.");
            }
            /// DO Validation and year start day here because I dont have the dlls yet.

            var openClosedDates = _openClosedDatesService.GetAllByYear(companyId, command.From.Year);

            var message = new CalculateBusinessDaysEvent
            {
                CorrelationId = command.CorrelationId,
                From = command.From,
                To = command.To,
                SubscriberId = command.SubscriberId,
                WorkingDaysOfTheWeek = employee.WorkingDaysOfTheWeek,
                NonWorkingDays = openClosedDates.ClosedDays,
                NonWorkingHalfDays = openClosedDates.ClosedHalfDays
            };

            _serviceBus.Publish(message);
            return Ok();
        }

        public bool BetweenDates(DateTime date, DateTime from, DateTime to)
        {
            return (date > from & date < to);
        }
    }
}
