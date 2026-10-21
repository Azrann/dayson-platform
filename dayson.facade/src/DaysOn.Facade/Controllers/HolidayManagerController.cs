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
    [RoutePrefix(("v1/manager"))]
    public class HolidayManagerController : ApiControllerBase 
    {
        private readonly ILog _logger;

        public HolidayManagerController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [Route("holidays/add")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> Create(AddHolidayCommand command)
        {
            _logger.LogInf("Manager CreateHoliday method called");
            return
                ResponseMessage(
                    await HolidayManagerClientWrapper.CreateHolidayAsync(command, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/cancel")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> Cancel(EmployeeHolidayCommand command)
        {
            _logger.LogInf("Manager CancelHoliday method called");
            return
                ResponseMessage(
                    await HolidayManagerClientWrapper.CancelHolidayAsync(command, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/{holidayId}/employee/{employeeId}")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(HolidayRequestDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid holidayId, Guid employeeId)
        {
            _logger.LogInf("Manager GetHolidayAsync method called");
            return
                ResponseMessage(
                    await HolidayManagerClientWrapper.GetHolidayAsync(employeeId, holidayId, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/employee/{employeeId}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<HolidayRequestDto>))]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid employeeId)
        {
            _logger.LogInf("Manager GetHolidaysAsync method called");
            return
                ResponseMessage(
                    await HolidayManagerClientWrapper.GetHolidaysAsync(employeeId, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/employee/{employeeId}/year/{year}")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<HolidayRequestDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid employeeId, int year)
        {
            _logger.LogInf("Manager GetHolidaysByYearAsync method called");
            return
                ResponseMessage(
                    await HolidayManagerClientWrapper.GetHolidaysByYearAsync(employeeId, year, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/approve")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Guid))]
        [HttpPost]
        public async Task<IHttpActionResult> Approve(EmployeeHolidayCommand command)
        {
            _logger.LogInf("Manager ApproveHolidayAsync method called");
            return
                ResponseMessage(
                    await HolidayManagerClientWrapper.ApproveHolidayAsync(command, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/reject")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Guid))]
        [HttpPost]
        public async Task<IHttpActionResult> Reject(EmployeeHolidayCommand command)
        {
            _logger.LogInf("Manager RejectHolidayAsync method called");
            return
                ResponseMessage(
                    await HolidayManagerClientWrapper.RejectHolidayAsync(command, Request.Headers.Authorization.Parameter));
        }
    }
}
