using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using DaysOn.Common.Utilities.Extension;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.HolidaysBc.Models.Commands;
using DaysOn.HolidaysBc.Models.Dtos;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.HolidaysBc.Api.Controllers
{
    [RoutePrefix("v1")]
    public class ApproverController : ApiControllerBase
    {
        private readonly IApproverService _approverService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ApproverController(IApproverService approverService)
        {
            _approverService = approverService;
        }

        [Authorize]
        [Route("company/approvers/all")]
        [HttpGet]
        public IHttpActionResult GetAllApprovers()
        {
            _logger.LogInf("[GetAllApprovers] teams method called");
            Guid companyId = GetCompanyId();
            var result = _approverService.GetApprovers(companyId);

            return Ok(result ?? new List<IdNameDto>());
        }

        [Authorize(Roles = "Admin,Owner,Manager")]
        [Route("company/approver/addEmployee")]
        [HttpPost]
        public IHttpActionResult AddEmployeeToApprover(AddEmployeeToApproverCommand command)
        {
            _logger.LogInf("[AddEmployeeToApprover] method called");
            Guid companyId = GetCompanyId();

            if (_approverService.AddApproverToAnEmployee(companyId, command.ApproverId, command.EmployeeId))
            {
                return Ok();
            }
            
            return NotFound();
        }

        [Authorize(Roles = "Admin,Owner,Manager")]
        [Route("company/approver/removeEmployee")]
        [HttpPost]
        public IHttpActionResult RemoveEmployeeFromApprover(RemoveApproverToAnEmployeeCommand command)
        {
            _logger.LogInf($"[RemoveEmployeeFromApprover] method called {JsonConvertExt.SerializeObject(command)}");
            Guid companyId = GetCompanyId();

            if (_approverService.RemoveApproverToAnEmployee(companyId, command.EmployeeId))
            {
                return Ok();
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/approver/add")]
        [HttpPost]
        public IHttpActionResult AddApprover([FromBody] Guid approverId)
        {
            _logger.LogInf("[AddApprover] method called");
            Guid companyId = GetCompanyId();

            if (_approverService.AddApprover(companyId,approverId))
            {
                return Ok();
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/approver/remove")]
        [HttpPost]
        public IHttpActionResult RemoveApprover([FromBody] Guid approverId)
        {
            _logger.LogInf("[RemoveApprover] method called");
            Guid companyId = GetCompanyId();

            if (_approverService.RemoveApprover(companyId, approverId))
            {
                return Ok();
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/approver/{approverId}/getemployees")]
        [HttpGet]
        public IHttpActionResult GetEmployeesForApprover(Guid approverId)
        {
            _logger.LogInf("[GetEmployeesForApprover] method called");
            Guid companyId = GetCompanyId();

            return Ok(_approverService.GetEmployeesForApprover(companyId, approverId));
        }
    }
}
