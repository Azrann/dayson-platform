using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Common.Utilities.Extension;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.HolidaysBc.Models.Commands;
using DaysOn.HolidaysBc.Models.Dtos;
using log4net;
using DaysOn.HolidaysBc.BoundedContext.Entities;

namespace DaysOn.HolidaysBc.Api.Controllers
{
    [RoutePrefix("v1")]
    public class HolidaysController : ApiControllerBase
    {
        private readonly IHolidayService _holidayService;
        private readonly IServiceBus _serviceBus;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public HolidaysController(IHolidayService holidayService, IServiceBus serviceBus)
        {
            _holidayService = holidayService;
            _serviceBus = serviceBus;
        }

        [Route("holidays/add")]
        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> Create(AddHolidayRequestCommand command)
        {
            var employeeId = GetEmployeeId();
            var companyId = GetCompanyId();
            _logger.LogInf($"CreateHoliday method called by employee {employeeId}");

            var addHolidayRequestEvent = AutoMapperBase.GetAutoMapperBase().Map<AddHolidayRequestEvent>(command);
            addHolidayRequestEvent.EmployeeId = employeeId;
            addHolidayRequestEvent.CompanyId = companyId;
            addHolidayRequestEvent.CorrelationId = command.CorrelationId;
            addHolidayRequestEvent.ActionBy = employeeId;
            if (addHolidayRequestEvent.Id == Guid.Empty)
            {
                addHolidayRequestEvent.Id = Guid.NewGuid();
            }
            _serviceBus.Publish(addHolidayRequestEvent);
            return Ok();
        }

        [Route("holidays/cancel")]
        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> Cancel(EmployeeHolidayCommand command)
        {
            _logger.LogInf($"User Cancel holidays method called holidayId={command.HolidayId} employeeId={command.EmployeeId}");
            if (command.EmployeeId != GetEmployeeId())
            {
                return BadRequestResult("You can only cancel your holidays.");
            }

            await _holidayService.CancelHolidayRequest(command.HolidayId, command.EmployeeId);
            return Ok();
        }

        [Route("holidays/changeHolidayStatus")]
        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> ChangeHolidayStatus(ChangeHolidayStatusRequest request)
        {
            var role = GetRole();
            _logger.LogInf($"{role} ChangeHolidayStatus method called holidayId={request.HolidayId}");
            var requesterId = GetEmployeeId();
            var companyId = GetCompanyId();
            HolidayRequestDto response;
            Status status = (Status) Enum.Parse(typeof(Status), request.Status.ToString());

            if (role == Roles.Admin.ToString() || role == Roles.Owner.ToString())
            {
                response = await _holidayService.AdminChangeHolidayStatus(request.HolidayId, status, requesterId, companyId);
            }
            else
            {
                response = await _holidayService.ChangeHolidayStatus(request.HolidayId, status, requesterId, companyId);
            }

            return Ok(response);
        }

        [Route("holidays/{holidayId}/employee/{employeeId}")]
        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid holidayId, Guid employeeId)
        {
            _logger.LogInf($"User Get holidays method called holidayId={holidayId} employeeId={employeeId}");
            if (employeeId != GetEmployeeId())
            {
                return BadRequestResult("You can only view your holiday requests");
            }

            var result = await _holidayService.GetHolidayRequest(holidayId, employeeId);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<HolidayRequestDto>(result));
        }

        [Route("holidays/employee/{employeeId}")]
        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid employeeId)
        {
            _logger.LogInf($"User Get holidays method called employeeId={employeeId}");
            if (employeeId != GetEmployeeId())
            {
                return BadRequestResult("You can only view your holiday requests");
            }

            var result = await _holidayService.GetAllHolidayRequests(employeeId);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<List<HolidayRequestDto>>(result));
        }

        [Route("holidays/employee/{employeeId}/year/{year}")]
        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid employeeId, int year)
        {
            _logger.LogInf($"User Get holidays by year method called employeeId={employeeId} year={year}");
            if (employeeId != GetEmployeeId())
            {
                return BadRequestResult("You can only view your holiday requests");
            }

            var result = await _holidayService.GetHolidaysRequestsByYear(employeeId, year);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<List<HolidayRequestDto>>(result));
        }

    }
}
