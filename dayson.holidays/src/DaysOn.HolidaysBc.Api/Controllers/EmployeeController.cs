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
    public class EmployeeController : ApiControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("employee/{employeeId}/globalRules")]
        [HttpPost]
        public async Task<IHttpActionResult> SetEmployeeGlobalRules(Guid employeeId, GlobalRulesDto globalRules)
        {
            _logger.LogInf("Admin,Owner SetEmployeeGlobalRules method called");
            Guid companyId = GetCompanyId();
            _employeeService.SetGlobalRules(employeeId, companyId, AutoMapperBase.GetAutoMapperBase().Map<GlobalRules>(globalRules));
            return Ok();
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("employee/{employeeId}/globalRules")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeGlobalRules(Guid employeeId)
        {
            _logger.LogInf("Admin,Owner GetEmployeeGlobalRules method called");
            Guid companyId = GetCompanyId();
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<EmployeeDto>(_employeeService.GetEmployeeWith(employeeId, "Approver")));
        }

        [Authorize]
        [Route("employee/globalRules")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMyGlobalRules()
        {
            _logger.LogInf("Admin,Owner GetEmployeeGlobalRules method called");
            Guid companyId = GetCompanyId();
            Guid employeeId = GetEmployeeId();
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<EmployeeDto>(_employeeService.GetEmployee(employeeId, companyId)));
        }
    }
}
