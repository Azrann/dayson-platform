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
    [RoutePrefix(("v1"))]
    public class HolidayController : ApiControllerBase 
    {
        private ILog _logger;

        public HolidayController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [Route("holidays/add")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> Create(AddHolidayRequestCommand command)
        {
            _logger.LogInf("AddHolidayRequestCommand method called");
            return
                ResponseMessage(
                    await HolidayClientWrapper.CreateHolidayAsync(command, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/cancel")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> Cancel(EmployeeHolidayCommand command)
        {
            _logger.LogInf("CancelHoliday method called");
            return
                ResponseMessage(
                    await HolidayClientWrapper.CancelHolidayAsync(command, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/{holidayId}/employee/{employeeId}")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(HolidayRequestDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid holidayId, Guid employeeId)
        {
            _logger.LogInf("GetHolidayAsync method called");
            return
                ResponseMessage(
                    await HolidayClientWrapper.GetHolidayAsync(employeeId, holidayId, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/employee/{employeeId}")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<HolidayRequestDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid employeeId)
        {
            _logger.LogInf("GetHolidaysAsync method called");
            return
                ResponseMessage(
                    await HolidayClientWrapper.GetHolidaysAsync(employeeId, Request.Headers.Authorization.Parameter));
        }

        [Route("holidays/employee/{employeeId}/year/{year}")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<HolidayRequestDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid employeeId, int year)
        {
            _logger.LogInf("GetHolidaysByYearAsync method called");
            return
                ResponseMessage(
                    await HolidayClientWrapper.GetHolidaysByYearAsync(employeeId, year, Request.Headers.Authorization.Parameter));
        }
    }
}
