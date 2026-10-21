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
using DaysOn.HolidaysBc.Models.Errors;
using Newtonsoft.Json;

namespace DaysOn.HolidaysBc.Api.Controllers
{
    [RoutePrefix("v1/admin")]
    public class HolidaysAdminController : ApiControllerBase
    {
        private readonly IHolidayService _holidayService;
        private readonly IEmployeeService _employeeService;
        private readonly IServiceBus _serviceBus;

        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public HolidaysAdminController(IHolidayService holidayService, IEmployeeService employeeService, IServiceBus serviceBus)
        {
            _holidayService = holidayService;
            _employeeService = employeeService;
            _serviceBus = serviceBus;
        }

        [Route("holidays/add")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(AddHolidayCommand command)
        {
            _logger.LogInf($"Admin CreateHoliday method called {JsonConvertExt.SerializeObject(command)}");
            if (!EmployeeBelongsToMyCompany(command.EmployeeId))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound,command.EmployeeId));
            }

            var employeeId = command.EmployeeId;
            var companyId = GetCompanyId();

            var addHolidayRequestEvent = AutoMapperBase.GetAutoMapperBase().Map<AddHolidayRequestEvent>(command);
            addHolidayRequestEvent.EmployeeId = employeeId;
            addHolidayRequestEvent.CompanyId = companyId;
            addHolidayRequestEvent.Status = command.Status;
            addHolidayRequestEvent.CorrelationId = command.CorrelationId;
            addHolidayRequestEvent.ActionBy = GetEmployeeId();
            if (addHolidayRequestEvent.Id == null || addHolidayRequestEvent.Id == Guid.Empty)
            {
                addHolidayRequestEvent.Id = Guid.NewGuid();
            }
            _serviceBus.Publish(addHolidayRequestEvent);
            return Ok(addHolidayRequestEvent);
        }

        [Route("holidays/{holidayId}/employee/{employeeId}")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid holidayId, Guid employeeId)
        {
            _logger.LogInf($"Admin GetHolidayRequest method called for employeeId: {employeeId}");
            if (!EmployeeBelongsToMyCompany(employeeId))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, employeeId));
            }

            var result = await _holidayService.GetHolidayRequest(holidayId, employeeId);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<HolidayRequestDto>(result));
        }

        [Route("holidays/employee/{employeeId}")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid employeeId)
        {
            _logger.LogInf($"Admin GetAllHolidayRequests method called for employeeId: {employeeId}");
            if (!EmployeeBelongsToMyCompany(employeeId))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, employeeId));
            }

            var result = await _holidayService.GetAllHolidayRequests(employeeId);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<List<HolidayRequestDto>>(result));
        }

        [Route("holidays/employee/{employeeId}/year/{year}")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid employeeId, int year)
        {
            _logger.LogInf($"Admin GetHolidaysByYearAsync method called {JsonConvertExt.SerializeObject(employeeId)}");
            if (!EmployeeBelongsToMyCompany(employeeId))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, employeeId));
            }

            var result = await _holidayService.GetHolidaysRequestsByYear(employeeId, year);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<List<HolidayRequestDto>>(result));
        }

        [Route("holidays/cancel")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<IHttpActionResult> Cancel(EmployeeHolidayCommand command)
        {
            _logger.LogInf($"Admin CancelHoliday method called {JsonConvertExt.SerializeObject(command)}");
            if (!EmployeeBelongsToMyCompany(command.EmployeeId))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, command.EmployeeId));
            }

            await _holidayService.CancelHolidayRequest(command.HolidayId, command.EmployeeId);
            return Ok();
        }

        [Route("holidays/approve")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<IHttpActionResult> Approve(EmployeeHolidayCommand command)
        {
            _logger.LogInf($"Admin Approve holiday for employee method called {JsonConvertExt.SerializeObject(command)}");
            if (!EmployeeBelongsToMyCompany(command.EmployeeId))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, command.EmployeeId));
            }

            await _holidayService.ApprovedHolidayRequest(command.HolidayId, command.EmployeeId);
            return Ok(command.HolidayId);
        }

        [Route("holidays/reject")]
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<IHttpActionResult> Reject(EmployeeHolidayCommand command)
        {
            _logger.LogInf($"Admin Reject holiday for employee method called {JsonConvertExt.SerializeObject(command)}");
            if (!EmployeeBelongsToMyCompany(command.EmployeeId))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, command.EmployeeId));
            }

            await _holidayService.RejectHolidayRequest(command.HolidayId, command.EmployeeId);
            return Ok(command.HolidayId);
        }

        private bool EmployeeBelongsToMyCompany(Guid employeeId)
        {
            var employee = _employeeService.GetEmployee(employeeId);
            return employee.CompanyId == GetCompanyId();
        }
    }
}
