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
using DaysOn.Common.Utilities.Extension;

namespace DaysOn.Auth.Web.BusEventHandlers
{
    [CastleComponent(typeof(IMessageHandler<TemporalManagerAddedToTeamEvent>))]
    public class TemporalManagerAddedToTeamEventHandler : IMessageHandler<TemporalManagerAddedToTeamEvent>
    {
        private readonly IManagerOfTeamService _managerOfTeamService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private TemporalManagerAddedToTeamEvent _message;

        public TemporalManagerAddedToTeamEventHandler(IManagerOfTeamService managerOfTeamService)
        {
            _managerOfTeamService = managerOfTeamService;
        }

        public bool Handle(TemporalManagerAddedToTeamEvent message)
        {
            _message = message;
            _logger.LogInf($"[TemporalManagerAddedToTeamEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(message)}]");

            var task = Task.Run(async () => { await _managerOfTeamService.AddManager(message.EmployeeId, message.TeamId); });
            task.Wait();

            _logger.LogInf($"[TemporalManagerAddedToTeamEventHandler] completed during registration with Model=[{message}].");
            return true;
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"[TemporalManagerAddedToTeamEventHandler] Model=[{_message}] ExceptionThrown=[{JsonConvertExt.SerializeObject(ex)}]");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}