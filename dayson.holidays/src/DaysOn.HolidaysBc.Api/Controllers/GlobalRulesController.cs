using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.Utilities.Extension;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.HolidaysBc.Models.Dtos;
using log4net;

namespace DaysOn.HolidaysBc.Api.Controllers
{
    [RoutePrefix("v1")]
    public class GlobalRulesController : ApiControllerBase
    {
        private readonly ICompanyGlobalRulesService _companyGlobalRulesService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public GlobalRulesController(ICompanyGlobalRulesService companyGlobalRulesService)
        {
            _companyGlobalRulesService = companyGlobalRulesService;
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("globalRules")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCompanyGlobalRules()
        {
            _logger.LogInf("Admin,Owner GetCompanyGlobalRules method called");
            Guid companyId = GetCompanyId();
            var company = _companyGlobalRulesService.GetGlobalRules(companyId);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<GlobalRulesDto>(company));
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("globalRules")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateCompanyGlobalRules(GlobalRulesDto globalRules)
        {
            _logger.LogInf("Admin,Owner UpdateCompanyGlobalRules method called");
            Guid companyId = GetCompanyId();
            var company = _companyGlobalRulesService.UpdateGlobalRules(companyId, AutoMapperBase.GetAutoMapperBase().Map<GlobalRules>(globalRules));
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<GlobalRulesDto>(company));
        }
    }
}
