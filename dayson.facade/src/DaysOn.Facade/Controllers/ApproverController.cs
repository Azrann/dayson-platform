using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.Utilities.Extension;
using DaysOn.Facade.Common;
using DaysOn.HolidaysBc.Models.Commands;
using log4net;

namespace DaysOn.Facade.Controllers
{
    [RoutePrefix(("v1"))]
    public class ApproverController : ApiControllerBase 
    {
        private readonly ILog _logger;

        public ApproverController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [AllowAnonymous]
        [Route("company/approvers/all")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllApprovers()
        {
            _logger.LogInf("[GetAllApprovers] method called");
            return
                ResponseMessage(await ApproverClientWrapper.GetAllApproversAsync(Request.Headers.Authorization.Parameter));
        }

        [AllowAnonymous]
        [Route("company/approver/addEmployee")]
        [HttpPost]
        public async Task<IHttpActionResult> AddEmployeeToApprover(AddEmployeeToApproverCommand command)
        {
            _logger.LogInf("[AddEmployeeToApprover] method called");
            return
                ResponseMessage(await ApproverClientWrapper.AddEmployeeToApproverAsync(command, Request.Headers.Authorization.Parameter));
        }

        [AllowAnonymous]
        [Route("company/approver/removeEmployee")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveEmployeeFromApprover(AddEmployeeToApproverCommand command)
        {
            _logger.LogInf("[RemoveEmployeeFromApproverAsync] method called");
            return
                ResponseMessage(await ApproverClientWrapper.RemoveEmployeeFromApprover(command, Request.Headers.Authorization.Parameter));
        }

        [AllowAnonymous]
        [Route("company/approver/add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddApprover([FromBody] Guid approverId)
        {
            _logger.LogInf("[AddApprover] method called");
            return
                ResponseMessage(await ApproverClientWrapper.AddApprover(approverId, Request.Headers.Authorization.Parameter));
        }

        [AllowAnonymous]
        [Route("company/approver/remove")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveApprover([FromBody] Guid approverId)
        {
            _logger.LogInf("[RemoveApprover] method called");
            return
                ResponseMessage(await ApproverClientWrapper.RemoveApprover(approverId, Request.Headers.Authorization.Parameter));
        }

        [AllowAnonymous]
        [Route("company/approver/{approverId}/getemployees")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeesForApprover(Guid approverId)
        {
            _logger.LogInf("[GetEmployeesForApprover] method called");

            return
                ResponseMessage(await ApproverClientWrapper.GetEmployeesForApprover(approverId, Request.Headers.Authorization.Parameter));
        }
    }
}
