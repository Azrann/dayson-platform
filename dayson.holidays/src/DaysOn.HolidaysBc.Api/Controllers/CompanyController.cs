using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CompanyController : ApiControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [Authorize(Roles = "MegaAdmin")]
        [Route("company/all")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            _logger.LogInf("MegaAdmin GetAll company method called");
            IEnumerable<Company> result = _companyService.GetAll();
            var companies = result.Select(AutoMapperBase.GetAutoMapperBase().Map<CompanyDto>);
            return Ok(companies);
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCompany()
        {
            _logger.LogInf("MegaAdmin GetCompany method called");
            Guid companyId = GetCompanyId();
            var company = _companyService.GetAll().FirstOrDefault(x => x.Id.ToString() == companyId.ToString());
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<CompanyDto>(company));
        }
    }
}
