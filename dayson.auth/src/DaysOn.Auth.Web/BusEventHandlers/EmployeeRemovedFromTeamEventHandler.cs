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
    [CastleComponent(typeof(IMessageHandler<EmployeeRemovedFromTeamEvent>))]
    public class EmployeeRemovedFromTeamEventHandler : IMessageHandler<EmployeeRemovedFromTeamEvent>
    {
        private readonly ITeamService _teamService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private EmployeeRemovedFromTeamEvent _message;

        public EmployeeRemovedFromTeamEventHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public bool Handle(EmployeeRemovedFromTeamEvent message)
        {
            _message = message;
            _logger.LogInf($"[EmployeeRemovedFromTeamEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(message)}]");

            var task = Task.Run(async () => { await _teamService.RemoveTeamFromEmployee(message.EmployeeId); });
            task.Wait();

            _logger.LogInf($"[EmployeeRemovedFromTeamEventHandler] completed during registration with Model=[{JsonConvertExt.SerializeObject(message)}].");
            return true;
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"[EmployeeRemovedFromTeamEventHandler] Model=[{_message}] ExceptionThrown=[{JsonConvertExt.SerializeObject(ex)}]");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}