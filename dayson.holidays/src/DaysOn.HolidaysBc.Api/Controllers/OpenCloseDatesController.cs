using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.HolidaysBc.Models.Commands;
using log4net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.Utilities.Extension;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.HolidaysBc.Api.Controllers
{
    [RoutePrefix("v1")]
    public class OpenCloseDatesController : ApiControllerBase
    {
        private readonly IOpenClosedDatesService _openCloseDatesService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public OpenCloseDatesController(IOpenClosedDatesService holidayService)
        {
            _openCloseDatesService = holidayService;
        }

        [Route("openCloseDates/year/{year}")]
        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> GetByYear(int year)
        {
            _logger.LogInf($"User GetByYear openCloseDates method called year={year}");
            var companyId = GetCompanyId();

            var result = _openCloseDatesService.GetByYear(companyId, year);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<OpenClosedDatesDto>(result));
        }

        [Route("openCloseDates/all/year/{year}")]
        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllByYear(int year)
        {
            _logger.LogInf($"User GetAllByYear openCloseDates method called year={year}");
            var companyId = GetCompanyId();

            var result = _openCloseDatesService.GetAllByYear(companyId, year);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<OpenClosedDatesDto>(result));
        }

        [Route("openCloseDates")]
        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> GetRecurrent()
        {
            _logger.LogInf($"User GetAllByYear GetRecurrent method called");
            var companyId = GetCompanyId();

            var result = _openCloseDatesService.GetRecurrent(companyId);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<OpenClosedDatesDto>(result));
        }

        [Route("openCloseDates/addRecurrent/ClosedDay")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<IHttpActionResult> AddRecurrentClosedDay(AddDateCommand command)
        {
            _logger.LogInf($"Admin,Owner AddRecurrentClosedDay method called Date={command.Date}");
            var companyId = GetCompanyId();

            _openCloseDatesService.AddRecurrentClosedDay(companyId, command.Date);
            return Ok();
        }

        [Route("openCloseDates/addRecurrent/ClosedHalfDay")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<IHttpActionResult> AddRecurrentClosedHalfDay(AddDateCommand command)
        {
            _logger.LogInf($"Admin,Owner AddRecurrentClosedHalfDay method called Date={command.Date}");
            var companyId = GetCompanyId();

            _openCloseDatesService.AddRecurrentClosedHalfDay(companyId, command.Date);
            return Ok();
        }

        [Route("openCloseDates/add/ClosedHalfDay")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<IHttpActionResult> AddClosedHalfDay(AddDateCommand command)
        {
            _logger.LogInf($"Admin,Owner AddClosedHalfDay method called Date={command.Date}");
            var companyId = GetCompanyId();

            _openCloseDatesService.AddClosedHalfDay(companyId, command.Date);
            return Ok();
        }

        [Route("openCloseDates/add/ClosedDay")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<IHttpActionResult> AddClosedDay(AddDateCommand command)
        {
            _logger.LogInf($"Admin,Owner AddClosedDay method called Date={command.Date}");
            var companyId = GetCompanyId();

            _openCloseDatesService.AddClosedDay(companyId, command.Date);
            return Ok();
        }

        [Route("openCloseDates/add/ClosedDays")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<IHttpActionResult> AddClosedDays(AddDatesCommand command)
        {
            _logger.LogInf($"Admin,Owner AddClosedDays method called CorrelationId={command.CorrelationId}");
            var companyId = GetCompanyId();

            var openClosedDates = AutoMapperBase.GetAutoMapperBase().Map<OpenClosedDates>(command);
            openClosedDates.CompanyId = companyId;
            _openCloseDatesService.AddClosedDays(openClosedDates, command.Year);
            return Ok();
        }

        [Route("openCloseDates/addRecurrent/ClosedDays")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<IHttpActionResult> AddRecurrentClosedDays(AddRecurrentDatesCommand command)
        {
            _logger.LogInf($"Admin,Owner AddRecurrentClosedDays method called CorrelationId={command.CorrelationId}");
            var companyId = GetCompanyId();

            var openClosedDates = AutoMapperBase.GetAutoMapperBase().Map<OpenClosedDates>(command);
            openClosedDates.CompanyId = companyId;
            _openCloseDatesService.AddRecurrentClosedDays(openClosedDates);
            return Ok();
        }
    }
}
