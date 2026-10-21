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
    [CastleComponent(typeof(IMessageHandler<TeamRemovedBusEvent>))]
    public class TeamRemovedBusEventHandler : IMessageHandler<TeamRemovedBusEvent>
    {
        private readonly ITeamService _teamService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TeamRemovedBusEventHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public bool Handle(TeamRemovedBusEvent message)
        {
            _logger.LogInf($"[TeamRemovedBusEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(message)}]");

            try
            {
                _teamService.RemoveTeam(message.TeamId);
                _logger.LogInf($"[TeamRemovedBusEventHandler] completed with Model=[{JsonConvertExt.SerializeObject(message)}]");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogErr(
                    $"[TeamRemovedBusEventHandler] Error during registration with Model=[{JsonConvertExt.SerializeObject(message)}]. Error=[{ex.Message}] .Stack=[{ex.StackTrace}]");
                return false;
            }
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"[TeamRemovedBusEventHandler] ExceptionThrown=[{JsonConvertExt.SerializeObject(ex)}]");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}