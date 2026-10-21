using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.Utilities.Extension;
using DaysOn.Facade.Common;
using DaysOn.HolidaysBc.Models.Commands;
using log4net;
using Swashbuckle.Swagger.Annotations;

namespace DaysOn.Facade.Controllers
{
    [RoutePrefix("v1")]
    public class HolidaysBrainController : ApiControllerBase
    {
        private readonly ILog _logger;

        public HolidaysBrainController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [Route("brain/businessDays")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> Get(GetBusinessDaysCommand command)
        {
            _logger.LogInf("GetBusinessDays method called");
            return
                ResponseMessage(
                    await HolidayBrainClientWrapper.GetBusinessDaysCommandAsync(command, Request.Headers.Authorization.Parameter));
        }
    }
}
