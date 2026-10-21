using System;
using System.Collections.Generic;
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
    [RoutePrefix(("v1/admin"))]
    public class HolidayAdminController : ApiControllerBase 
    {
        private ILog _logger;

        public HolidayAdminController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [Route("holidays/add")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> Create(AddHolidayCommand command)
        {
            _logger.LogInf("Admin CreateHoliday method called");
            return
                ResponseMessage(
                    await HolidayAdminClientWrapper.CreateHolidayAsync(command, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/{holidayId}/employee/{employeeId}")]
        [AllowAnonymous]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(HolidayRequestDto))]
        public async Task<IHttpActionResult> Get(Guid holidayId, Guid employeeId)
        {
            _logger.LogInf("Admin GetHolidayAsync method called");
            return
                ResponseMessage(
                    await HolidayAdminClientWrapper.GetHolidayAsync(employeeId, holidayId, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/employee/{employeeId}")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<HolidayRequestDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid employeeId)
        {
            _logger.LogInf("Admin GetHolidaysAsync method called");
            return
                ResponseMessage(
                    await HolidayAdminClientWrapper.GetHolidaysAsync(employeeId, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/employee/{employeeId}/year/{year}")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<HolidayRequestDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid employeeId, int year)
        {
            _logger.LogInf("Admin GetHolidaysByYearAsync method called");
            return
                ResponseMessage(
                    await HolidayAdminClientWrapper.GetHolidaysByYearAsync(employeeId, year, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/cancel")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> Cancel(EmployeeHolidayCommand command)
        {
            _logger.LogInf("Admin CancelHoliday method called");
            return
                ResponseMessage(
                    await HolidayAdminClientWrapper.CancelHolidayAsync(command, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/approve")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Guid))]
        [HttpPost]
        public async Task<IHttpActionResult> Approve(EmployeeHolidayCommand command)
        {
            _logger.LogInf("Admin ApproveHolidayAsync method called");
            return
                ResponseMessage(
                    await HolidayAdminClientWrapper.ApproveHolidayAsync(command, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/reject")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Guid))]
        [HttpPost]
        public async Task<IHttpActionResult> Reject(EmployeeHolidayCommand command)
        {
            _logger.LogInf("Admin RejectHolidayAsync method called");
            return
                ResponseMessage(
                    await HolidayAdminClientWrapper.RejectHolidayAsync(command, Request.Headers.Authorization.Parameter));
        }
    }
}
