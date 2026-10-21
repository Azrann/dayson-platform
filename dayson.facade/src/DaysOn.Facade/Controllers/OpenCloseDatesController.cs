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
    [RoutePrefix("v1")]
    public class OpenCloseDatesController : ApiControllerBase
    {
        private readonly ILog _logger;

        public OpenCloseDatesController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [Route("openCloseDates/year/{year}")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OpenClosedDatesDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetByYear(int year)
        {
            _logger.LogInf("GetByYear method called");

            return ResponseMessage(await OpenCloseDatesClientWrapper.GetByYear(year, Request.Headers.Authorization.Parameter));
        }

        [Route("openCloseDates/all/year/{year}")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OpenClosedDatesDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllByYear(int year)
        {
            _logger.LogInf("GetAllByYear method called");

            return ResponseMessage(await OpenCloseDatesClientWrapper.GetAllByYear(year, Request.Headers.Authorization.Parameter));
        }

        [Route("openCloseDates")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OpenClosedDatesDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetRecurrent()
        {
            _logger.LogInf("GetRecurrent method called");

            return ResponseMessage(await OpenCloseDatesClientWrapper.GetRecurrent(Request.Headers.Authorization.Parameter));
        }

        [Route("openCloseDates/addRecurrent/ClosedDay")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> AddRecurrentClosedDay(AddDateCommand command)
        {
            _logger.LogInf("AddRecurrentClosedDay method called");

            return ResponseMessage(await OpenCloseDatesClientWrapper.AddRecurrentClosedDay(command, Request.Headers.Authorization.Parameter));
        }

        [Route("openCloseDates/addRecurrent/ClosedHalfDay")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> AddRecurrentClosedHalfDay(AddDateCommand command)
        {
            _logger.LogInf("AddRecurrentClosedHalfDay method called");

            return ResponseMessage(await OpenCloseDatesClientWrapper.AddRecurrentClosedHalfDay(command, Request.Headers.Authorization.Parameter));
        }

        [Route("openCloseDates/add/ClosedHalfDay")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> AddClosedHalfDay(AddDateCommand command)
        {
            _logger.LogInf("AddClosedHalfDay method called");

            return ResponseMessage(await OpenCloseDatesClientWrapper.AddClosedHalfDay(command, Request.Headers.Authorization.Parameter));
        }

        [Route("openCloseDates/add/ClosedDay")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> AddClosedDay(AddDateCommand command)
        {
            _logger.LogInf("AddClosedDay method called");

            return ResponseMessage(await OpenCloseDatesClientWrapper.AddClosedDay(command, Request.Headers.Authorization.Parameter));
        }

        [Route("openCloseDates/add/ClosedDays")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> AddClosedDays(AddDatesCommand command)
        {
            _logger.LogInf("AddClosedDays method called");

            return ResponseMessage(await OpenCloseDatesClientWrapper.AddClosedDays(command, Request.Headers.Authorization.Parameter));
        }

        [Route("openCloseDates/addRecurrent/ClosedDays")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> AddRecurrentClosedDays(AddRecurrentDatesCommand command)
        {
            _logger.LogInf("AddRecurrentClosedDays method called");

            return ResponseMessage(await OpenCloseDatesClientWrapper.AddRecurrentClosedDays(command, Request.Headers.Authorization.Parameter));
        }
    }
}
