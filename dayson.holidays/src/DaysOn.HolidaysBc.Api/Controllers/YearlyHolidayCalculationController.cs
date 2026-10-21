using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.HolidaysBc.Models.Commands;
using DaysOn.HolidaysBc.Models.Dtos;
using DaysOn.HolidaysBc.Models.Errors;
using log4net;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.Utilities.Extension;

namespace DaysOn.HolidaysBc.Api.Controllers
{
    [RoutePrefix("v1")]
    public class YearlyHolidayCalculationController : ApiControllerBase
    {
        private readonly ICalculationService _calculationService;
        private readonly IEmployeeService _employeeService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public YearlyHolidayCalculationController(ICalculationService calculationService, IEmployeeService employeeService)
        {
            _calculationService = calculationService;
            _employeeService = employeeService;
        }

        [Route("calculations/year/{year}")]
        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int year)
        {
            _logger.LogInf("$YearlyHolidayCalculation get method called year={year}");
            var employeeId = GetEmployeeId();

            var yearlyHolidayCalculation = _calculationService.GetCalculationsByYearCreateIfNotExists(employeeId, year);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<YearlyHolidayCalculationDto>(yearlyHolidayCalculation));
        }

        [Route("admin/calculations/employee/{employeeId}/year/{year}")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCalculations(Guid employeeId, int year)
        {
            _logger.LogInf($"Admin,Owner YearlyHolidayCalculation get method called year={year} employeeId={employeeId}");
            if (!EmployeeBelongsToMyCompany(employeeId))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, employeeId));
            }

            var yearlyHolidayCalculation = _calculationService.GetCalculationsByYearCreateIfNotExists(employeeId, year);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<YearlyHolidayCalculationDto>(yearlyHolidayCalculation));
        }

        [Route("carryoverdays")]
        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> CarryOver(CarryOverCommand command)
        {
            _logger.LogInf($"User CarryOver post method called CorrelationId={command.CorrelationId}");
            if (command.Days < 1)
            {
                return BadRequestResult("Number of days must be bigger than 0");
            }

            _calculationService.CarryOverDays(GetEmployeeId(), command.Year, command.Days);
            return Ok();
        }

        [Route("correctiondays")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<IHttpActionResult> Correction(CorrectionDaysCommand command)
        {
            _logger.LogInf("Admin,Owner Correction post method called CorrelationId={command.CorrelationId}");

            if (!EmployeeBelongsToMyCompany(command.EmployeeId))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, command.EmployeeId));
            }

            _calculationService.SetCorrection(command.EmployeeId, command.Year, command.Days);
            return Ok();
        }

        private bool EmployeeBelongsToMyCompany(Guid employeeId)
        {
            var employee = _employeeService.GetEmployee(employeeId);
            return employee.CompanyId == GetCompanyId();
        }

    }
}
