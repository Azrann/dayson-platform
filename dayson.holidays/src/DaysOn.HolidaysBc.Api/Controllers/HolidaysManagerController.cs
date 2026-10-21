using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Common.Utilities.Extension;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.HolidaysBc.Models.Commands;
using DaysOn.HolidaysBc.Models.Dtos;
using log4net;
using DaysOn.HolidaysBc.Models.Errors;

namespace DaysOn.HolidaysBc.Api.Controllers
{
    [RoutePrefix("v1/manager")]
    public class HolidaysManagerController : ApiControllerBase
    {
        private readonly IHolidayService _holidayService;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeTeamService _employeeTeamService;
        private readonly IServiceBus _serviceBus;

        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public HolidaysManagerController(IHolidayService holidayService, IEmployeeService employeeService, IEmployeeTeamService employeeTeamService
            ,IServiceBus serviceBus)
        {
            _holidayService = holidayService;
            _employeeService = employeeService;
            _employeeTeamService = employeeTeamService;
            _serviceBus = serviceBus;
        }

        [Route("holidays/add")]
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(AddHolidayCommand command)
        {
            _logger.LogInf("Manager CreateHoliday method called");
           
            var employeeId = command.EmployeeId;
            var companyId = GetCompanyId();
            var employee = _employeeService.GetEmployee(employeeId);
            if (!EmployeeBelongsToMyCompany(employee))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, employeeId));
            }

            if (!EmployeeBelongsToMyTeam(employee.Id))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotInMyTeam, employeeId));
            }

            var addHolidayRequestEvent = AutoMapperBase.GetAutoMapperBase().Map<AddHolidayRequestEvent>(command);
            addHolidayRequestEvent.EmployeeId = employeeId;
            addHolidayRequestEvent.CompanyId = companyId;
            addHolidayRequestEvent.CorrelationId = command.CorrelationId;

            _serviceBus.Publish(addHolidayRequestEvent);
            return Ok();
        }

        [Route("holidays/cancel")]
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IHttpActionResult> Cancel(EmployeeHolidayCommand command)
        {
            _logger.LogInf("Manager CancelHoliday method called");
            var employee = _employeeService.GetEmployee(command.EmployeeId);
            if (!EmployeeBelongsToMyCompany(employee))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, command.EmployeeId));
            }

            if (!EmployeeBelongsToMyTeam(employee.Id))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotInMyTeam, command.EmployeeId));
            }

            await _holidayService.CancelHolidayRequest(command.HolidayId, command.EmployeeId);
            return Ok();
        }

        [Route("holidays/approve")]
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IHttpActionResult> Approve(EmployeeHolidayCommand command)
        {
            _logger.LogInf("Manager Approve holiday for employee method called");
            var employee = _employeeService.GetEmployee(command.EmployeeId);
            if (!EmployeeBelongsToMyCompany(employee))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, command.EmployeeId));
            }

            if (!EmployeeBelongsToMyTeam(employee.Id))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotInMyTeam, command.EmployeeId));
            }

            await _holidayService.ApprovedHolidayRequest(command.HolidayId, command.EmployeeId);
            return Ok(command.HolidayId);
        }

        [Route("holidays/reject")]
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IHttpActionResult> Reject(EmployeeHolidayCommand command)
        {
            _logger.LogInf("Manager Reject holiday for employee method called");
            var employee = _employeeService.GetEmployee(command.EmployeeId);
            if (!EmployeeBelongsToMyCompany(employee))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, command.EmployeeId));
            }

            if (!EmployeeBelongsToMyTeam(employee.Id))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotInMyTeam, command.EmployeeId));
            }

            await _holidayService.RejectHolidayRequest(command.HolidayId, command.EmployeeId);
            return Ok(command.HolidayId);
        }

        [Route("holidays/{holidayId}/employee/{employeeId}")]
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid holidayId, Guid employeeId)
        {
            _logger.LogInf("Manager Get holiday for employee method called");
            var employee = _employeeService.GetEmployee(employeeId);
            if (!EmployeeBelongsToMyCompany(employee))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, employeeId));
            }

            if (!EmployeeBelongsToMyTeam(employee.Id))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotInMyTeam, employeeId));
            }

            var result = await _holidayService.GetHolidayRequest(holidayId, employeeId);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<HolidayRequestDto>(result));
        }

        [Route("holidays/employee/{employeeId}")]
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid employeeId)
        {
            _logger.LogInf("Manager GetHolidaysAsync method called");
            var employee = _employeeService.GetEmployee(employeeId);
            if (!EmployeeBelongsToMyCompany(employee))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, employeeId));
            }

            if (!EmployeeBelongsToMyTeam(employee.Id))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotInMyTeam, employeeId));
            }

            var result = await _holidayService.GetAllHolidayRequests(employeeId);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<List<HolidayRequestDto>>(result));
        }

        [Route("holidays/employee/{employeeId}/year/{year}")]
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid employeeId, int year)
        {
            _logger.LogInf("Manager GetHolidaysByYearAsync method called");
            var employee = _employeeService.GetEmployee(employeeId);
            if (!EmployeeBelongsToMyCompany(employee))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotFound, employeeId));
            }

            if (!EmployeeBelongsToMyTeam(employee.Id))
            {
                return BadRequestResult(string.Format(HolidayApiErrors.EmployeeNotInMyTeam, employeeId));
            }

            var result = await _holidayService.GetHolidaysRequestsByYear(employeeId, year);
            return Ok(AutoMapperBase.GetAutoMapperBase().Map<List<HolidayRequestDto>>(result));
        }

        private bool EmployeeBelongsToMyCompany(Employee employee)
        {
            return employee.CompanyId == GetCompanyId();
        }

        private bool EmployeeBelongsToMyTeam(Guid employeeId)
        {
            var teamId = _employeeTeamService.GetTeamBy(employeeId);
            return GetMyTeams().Contains(teamId);
        }
    }
}
