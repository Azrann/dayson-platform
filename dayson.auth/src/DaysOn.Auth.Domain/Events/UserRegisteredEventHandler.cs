using System.Reflection;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.Common.Events.Events;
using DaysOn.Common.Utilities.Extension;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.Auth.Domain.Events
{
    public class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
    {
        private readonly IServiceBus _serviceBus;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserRegisteredEventHandler(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public void Handle(UserRegisteredEvent domainEvent)
        {
            _logger.LogInf($"[UserRegisteredDomainEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(domainEvent)}]");

            _serviceBus.Publish(new SendEmailEvent
            {
                EmailTo = domainEvent.UserEmailAddress,
                Subject = "Your account is ready.",
                BodyHtml = $@"You have been created an account at HRPlan, set up your account in the following link <a href=""http://www.dayson-ui.gearhost.com/password/{domainEvent.Code}"">click here</a>"
            });

            _logger.LogInf($"[UserRegisteredDomainEventHandler] completed");

        }
    }
}