using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.Utilities.Extension;
using DaysOn.Facade.Common;
using DaysOn.HolidaysBc.Models.Commands;
using DaysOn.HolidaysBc.Models.Dtos;
using log4net;
using Swashbuckle.Swagger.Annotations;

namespace DaysOn.Facade.Controllers
{
    [RoutePrefix(("v1"))]
    public class YearlyHolidayCalculationController : ApiControllerBase
    {
        private readonly ILog _logger;

        public YearlyHolidayCalculationController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [Route("calculations/year/{year}")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(YearlyHolidayCalculationDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int year)
        {
            _logger.LogInf("YearlyHolidayCalculation get method called");
            return
                ResponseMessage(
                    await YearlyHolidayCalculationClientWrapper.GetYearlyHolidayCalculationByEmployee(year, Request.Headers.Authorization.Parameter));
        }

        [Route("admin/calculations/employee/{employeeId}/year/{year}")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(YearlyHolidayCalculationDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetCalculations(Guid employeeId, int year)
        {
            _logger.LogInf("Admin YearlyHolidayCalculation get method called");
            return
                ResponseMessage(
                    await YearlyHolidayCalculationClientWrapper.AdminGetYearlyHolidayCalculationByEmployee(employeeId, year, Request.Headers.Authorization.Parameter));
        }

        [Route("carryoverdays")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> CarryOver(CarryOverCommand command)
        {
            _logger.LogInf("CarryOver post method called");
            return
                ResponseMessage(
                    await YearlyHolidayCalculationClientWrapper.CarryOverDays(command, Request.Headers.Authorization.Parameter));
        }

        [Route("correctiondays")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> Correction(CorrectionDaysCommand command)
        {
            _logger.LogInf("Correction post method called");
            return
                ResponseMessage(
                    await YearlyHolidayCalculationClientWrapper.CorrectionDays(command, Request.Headers.Authorization.Parameter));
        }
    }
}
