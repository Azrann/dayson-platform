using System;
using System.Collections.Generic;
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
    public class EmployeeController : ApiControllerBase 
    {
        private readonly ILog _logger;

        public EmployeeController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [AllowAnonymous]
        [Route("employee/{employeeId}/globalRules")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> SetEmployeeGlobalRules(Guid employeeId, GlobalRulesDto globalRules)
        {
            _logger.LogInf("SetEmployeeGlobalRules method called");
            return
                ResponseMessage(
                    await GlobalRulesClientWrapper.SetEmployeeGlobalRules(employeeId, globalRules, Request.Headers.Authorization.Parameter));
        }

        [AllowAnonymous]
        [Route("employee/{employeeId}/globalRules")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(HolidaysEmployeeDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeGlobalRules(Guid employeeId)
        {
            _logger.LogInf("GetEmployeeGlobalRules method called");
            return
                ResponseMessage(
                    await GlobalRulesClientWrapper.GetEmployeeGlobalRules(employeeId, Request.Headers.Authorization.Parameter));
        }

        [AllowAnonymous]
        [Route("employee/globalRules")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(HolidaysEmployeeDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeGlobalRules()
        {
            _logger.LogInf("GetMyGlobalRules method called");
            return
                ResponseMessage(
                    await GlobalRulesClientWrapper.GetMyGlobalRules(Request.Headers.Authorization.Parameter));
        }
    }

    public class HolidaysEmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public int HolidayEntitlement { get; set; }
        public int MaxDaysCarriedOverNextYear { get; set; }
        public int MinDaysAdvanceNoticeHolidays { get; set; }
        public int MaxConsecutiveDaysOff { get; set; }
        public DateTime YearStartDay { get; set; }
        public Guid CompanyId { get; set; }
        public Dictionary<DayOfWeek, bool> WorkingDaysOfTheWeek { get; set; }
    }
}
