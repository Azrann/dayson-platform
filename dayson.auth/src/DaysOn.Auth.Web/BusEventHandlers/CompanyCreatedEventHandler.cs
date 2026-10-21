using System;
using System.Linq;
using System.Reflection;
using Castle.Core;
using DaysOn.Auth.Domain.Application;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.Auth.Web.BusEventHandlers
{
    [CastleComponent(typeof(IMessageHandler<CompanyCreatedBusEvent>))]
    public class CompanyCreatedEventHandler : IMessageHandler<CompanyCreatedBusEvent>
    {
        private readonly IAuthService _authService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CompanyCreatedEventHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public bool Handle(CompanyCreatedBusEvent message)
        {
            _logger.LogInf($"[CompanyCreatedEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(message)}]");
            var result = _authService.RegisterUserAsync(message.AdminEmailAddress, message.OwnerId.GetHashCode().ToString(), message.CompanyId.ToString(), "Owner", message.OwnerId.ToString()).Result;

            if (result == null)
            {
                _logger.LogInf(
                    $"[CompanyCreatedEventHandler] Unknown error during registration with Model=[{JsonConvertExt.SerializeObject(message)}].");
                return false;
            }

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogInf(
                        $"[CompanyCreatedEventHandler] Error during registration with Model=[{JsonConvertExt.SerializeObject(message)}]. Error=[{error}]");
                }
                return false;
            }
            return true;
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"[CompanyCreatedEventHandler] ExceptionThrown=[{JsonConvertExt.SerializeObject(ex)}]");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}