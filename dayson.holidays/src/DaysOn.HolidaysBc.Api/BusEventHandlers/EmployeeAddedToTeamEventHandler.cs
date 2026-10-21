using System;
using System.Reflection;
using Castle.Core;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;
using DaysOn.HolidaysBc.BoundedContext.Services;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.HolidaysBc.Api.BusEventHandlers
{
    [CastleComponent(typeof(IMessageHandler<EmployeeAddedToTeamEvent>))]
    public class EmployeeAddedToTeamEventHandler : IMessageHandler<EmployeeAddedToTeamEvent>
    {
        private readonly ITeamMembersService _teamMembersService;
        private readonly IEmployeeTeamService _employeeTeamService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public EmployeeAddedToTeamEventHandler(ITeamMembersService teamMembersService, IEmployeeTeamService employeeTeamService)
        {
            _teamMembersService = teamMembersService;
            _employeeTeamService = employeeTeamService;
        }

        public bool Handle(EmployeeAddedToTeamEvent message)
        {
            _logger.LogInf($"[EmployeeAddedToTeamEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(message)}]");

            try
            {
                _teamMembersService.AddEmployeeToATeam(message.EmployeeId, message.TeamId);
                _employeeTeamService.AddEmployeeToATeam(message.EmployeeId, message.TeamId);
                _logger.LogInf($"[EmployeeAddedToTeamEventHandler] completed with Model=[{JsonConvertExt.SerializeObject(message)}]");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogErr(
                    $"[EmployeeAddedToTeamEventHandler] Error during registration with Model=[{JsonConvertExt.SerializeObject(message)}]. Error=[{ex.Message}] .Stack=[{ex.StackTrace}]");
                return false;
            }
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"[EmployeeAddedToTeamEventHandler] ExceptionThrown=[{JsonConvertExt.SerializeObject(ex)}]");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}