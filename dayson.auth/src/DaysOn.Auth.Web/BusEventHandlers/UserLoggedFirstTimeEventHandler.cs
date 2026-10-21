using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Auth.Domain.Services;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.Auth.Web.BusEventHandlers
{
    [CastleComponent(typeof(IMessageHandler<UserLoggedFirstTimeEvent>))]
    public class UserLoggedFirstTimeEventHandler : IMessageHandler<UserLoggedFirstTimeEvent>
    {
        private readonly IResetPasswordService _resetPasswordService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserLoggedFirstTimeEventHandler(IResetPasswordService resetPasswordService)
        {
            _resetPasswordService = resetPasswordService;
        }

        public bool Handle(UserLoggedFirstTimeEvent message)
        {
            _logger.LogInf($"[UserLoggedFirstTimeEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(message)}]");
            return true;
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"[UserLoggedFirstTimeEventHandler] ExceptionThrown=[{JsonConvertExt.SerializeObject(ex)}]");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}