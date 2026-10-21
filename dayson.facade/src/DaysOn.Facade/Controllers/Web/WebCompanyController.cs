using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.Utilities.Extension;
using DaysOn.Facade.Common;
using DaysOn.Facade.Common.Dtos;
using DaysOn.Facade.Common.Dtos.WebCompany;
using DaysOn.Facade.Models;
using DaysOn.Facade.Models.WebModels.Company;
using DaysOn.HolidaysBc.Models.Dtos;
using log4net;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;

namespace DaysOn.Facade.Controllers.Web
{
    [RoutePrefix("v1")]
    public class WebCompanyController : ApiControllerBase
    {
        private readonly ILog _logger;
        private string GetMyCompanyUri = "v1/company/mycompany";

        public WebCompanyController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [AllowAnonymous]
        [Route("WebCompany/Ping")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> Ping()
        {
            _logger.LogDeb("WebCompany.Ping invoked");

            return await Task.FromResult<IHttpActionResult>(Ok("WebCompany service is up and running!"));
        }

        [AllowAnonymous]
        [Route("WebCompany/GetCompanyRules")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GetCompanyRulesResponse))]
        [HttpGet]
        public async Task<IHttpActionResult> GetCompanyRules([FromUri] GetCompanyRulesRequest request)
        {
#if OFFLINE
            return Ok(await GetCompanyRulesOffline.GetCompanyRules(request));
#else
            if (!string.IsNullOrEmpty(Request.Headers.Authorization?.Parameter))
            {
                var responseString = await
                    (await GlobalRulesClientWrapper.GetCompanyGlobalRulesAsync(Request.Headers.Authorization.Parameter))
                    .Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<GlobalRulesDto>(responseString);

                return await Task.FromResult(Ok(new GetCompanyRulesResponse
                {
                    Rules = WebCompanyMapper.GetCompanyRules(response)
                }));
            }
            _logger.Warn("GlobalRules get method called with Authorization parameter missing.");
            return BadRequest("Auth parameter missing");
#endif
        }

        [AllowAnonymous]
        [Route("WebCompany/GetCompanyOverview")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GetCompanyOverviewResponse))]
        [HttpGet]
        public async Task<IHttpActionResult> GetCompanyOverview([FromUri] GetCompanyOverviewRequest request)
        {
#if OFFLINE
            return Ok(await GetCompanyOverviewOffline.GetCompanyOverview(request));
#else
            var baseUrl = ConfigurationManager.AppSettings[BaseUrl.CompanyServer.ToString()];
            var result = await HttpClientHelper.GetAsync(baseUrl, GetMyCompanyUri, GetTokenFromRequest());
            var resultString = await result.Content.ReadAsStringAsync();

            var companyOverview = JsonConvert.DeserializeObject<CompanyOverview>(resultString);
            return await Task.FromResult(Ok(new GetCompanyOverviewResponse
            {
                Company = companyOverview
            }));
#endif
        }

        [AllowAnonymous]
        [Route("WebCompany/UpdateCompanyRules")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UpdateCompanyRulesResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateCompanyRules([FromBody] UpdateCompanyRulesRequest request)
        {
#if OFFLINE
            return Ok(await UpdateCompanyRulesOffline.UpdateCompanyRules(request));
#else
            GlobalRulesDto command = WebCompanyMapper.GetGlobalRulesDto(request.Rules);

            if (!string.IsNullOrEmpty(Request.Headers.Authorization?.Parameter))
            {
                var result = await 
                    (await (GlobalRulesClientWrapper.UpdateCompanyGlobalRulesAsync(command,
                        Request.Headers.Authorization.Parameter))).GetGlobalRuleDtoAsync();
                return await Task.FromResult(Ok(WebCompanyMapper.GetCompanyRules(result)));
            }

            _logger.Warn("GlobalRules update method called with Authorization parameter missing.");
            return BadRequest("Auth parameter missing");
#endif
        }
    }
}
