using System;
using System.Reflection;
using Castle.Core;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.HolidaysBc.Api.BusEventHandlers
{
    [CastleComponent(typeof(IMessageHandler<TeamCreatedBusEvent>))]
    public class TeamAddedBusEventHandler : IMessageHandler<TeamCreatedBusEvent>
    {
        private readonly ITeamService _teamService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TeamAddedBusEventHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public bool Handle(TeamCreatedBusEvent message)
        {
            _logger.LogInf($"[TeamAddedBusEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(message)}]");

            try
            {
                _teamService.Add(new Team(message.TeamId)
                {
                    CompanyId = message.CompanyId,
                    CreatedOn = message.ActionTime.DateTime,
                    Name = message.Name,
                    LastUpdated = message.ActionTime.DateTime,
                });
                _logger.LogInf($"[TeamAddedBusEventHandler] completed with Model=[{JsonConvertExt.SerializeObject(message)}]");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogErr(
                    $"[TeamAddedBusEventHandler] Error during registration with Model=[{JsonConvertExt.SerializeObject(message)}]. Error=[{ex.Message}] .Stack=[{ex.StackTrace}]");
                return false;
            }
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"[TeamAddedBusEventHandler] ExceptionThrown=[{JsonConvertExt.SerializeObject(ex)}]");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}