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
    [CastleComponent(typeof(IMessageHandler<ManagerRemovedFromTeamEvent>))]
    public class ManagerRemovedFromTeamEventHandler : IMessageHandler<ManagerRemovedFromTeamEvent>
    {
        private readonly IManagerOfTeamService _managerOfTeamService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ManagerRemovedFromTeamEvent _message;

        public ManagerRemovedFromTeamEventHandler(IManagerOfTeamService managerOfTeamService)
        {
            _managerOfTeamService = managerOfTeamService;
        }

        public bool Handle(ManagerRemovedFromTeamEvent message)
        {
            _message = message;
            _logger.LogInf($"[ManagerRemovedFromTeamEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(message)}]");

            var task = Task.Run(async () => { await _managerOfTeamService.RemoveManagerFromTeam(message.EmployeeId, message.TeamId); });
            task.Wait();

            _logger.LogInf($"[ManagerRemovedFromTeamEventHandler] completed during registration with Model=[{message}].");
            return true;
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"[ManagerRemovedFromTeamEventHandler] Model=[{_message}] ExceptionThrown=[{JsonConvertExt.SerializeObject(ex)}]");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}