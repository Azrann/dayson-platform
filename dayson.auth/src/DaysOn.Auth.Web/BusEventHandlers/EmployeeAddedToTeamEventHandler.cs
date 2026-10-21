using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Auth.Domain.Application;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.Auth.Web.BusEventHandlers
{
    [CastleComponent(typeof(IMessageHandler<EmployeeAddedToTeamEvent>))]
    public class EmployeeAddedToTeamEventHandler : IMessageHandler<EmployeeAddedToTeamEvent>
    {
        private readonly ITeamService _teamService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private EmployeeAddedToTeamEvent _message;

        public EmployeeAddedToTeamEventHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public bool Handle(EmployeeAddedToTeamEvent message)
        {
            _message = message;
            _logger.LogInf($"[EmployeeAddedToTeamEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(message)}]");

            var task = Task.Run(async () => { await _teamService.AddEmployee(message.EmployeeId, message.TeamId); });
            task.Wait();

            _logger.LogInf($"[EmployeeAddedToTeamEventHandler] completed during registration with Model=[{JsonConvertExt.SerializeObject(message)}].");
            return true;
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"[EmployeeAddedToTeamEventHandler] Model=[{_message}] ExceptionThrown=[{JsonConvertExt.SerializeObject(ex)}]");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}