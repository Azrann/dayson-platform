using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.Utilities.Extension;
using DaysOn.Facade.Common;
using DaysOn.HolidaysBc.Models.Dtos;
using log4net;
using Swashbuckle.Swagger.Annotations;

namespace DaysOn.Facade.Controllers
{
    [RoutePrefix(("v1"))]
    public class CompanyGlobalRulesController : ApiControllerBase 
    {
        private readonly ILog _logger;

        public CompanyGlobalRulesController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [Route("globalRules")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GlobalRulesDto))]
        [HttpPost]
        public async Task<IHttpActionResult> Update(GlobalRulesDto command)
        {
            _logger.LogInf("GlobalRules update method called");

            if (Request.Headers.Authorization != null && !string.IsNullOrEmpty(Request.Headers.Authorization.Parameter))
            {
                return
                ResponseMessage(
                    await GlobalRulesClientWrapper.UpdateCompanyGlobalRulesAsync(command, Request.Headers.Authorization.Parameter));
            }

            _logger.Warn("GlobalRules update method called with Authorization parameter missing.");
            return BadRequest("Auth parameter missing");
        }

        [Route("globalRules")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GlobalRulesDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            _logger.LogInf("GlobalRules get method called");

            if (!string.IsNullOrEmpty(Request.Headers.Authorization?.Parameter))
            {
                return
                    ResponseMessage(
                        await GlobalRulesClientWrapper.GetCompanyGlobalRulesAsync(Request.Headers.Authorization.Parameter));
            }

            _logger.Warn("GlobalRules get method called with Authorization parameter missing.");
            return BadRequest("Auth parameter missing");
        }
    }
}
