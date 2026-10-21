using System.Reflection;
using DaysOn.Common;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Events.Events;
using DaysOn.Common.Utilities.Extension;
using DaysOn.Common.Domain.DomainEvents;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.CompanyBc.BoundedContext.Events
{
    public class TeamCreatedEventHandler : IEventHandler<TeamCreatedEvent>
    {
        private readonly IServiceBus _bus;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TeamCreatedEventHandler(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Handle(TeamCreatedEvent domainEvent)
        {
            _logger.LogInf(
                $"TeamCreatedDomainEventHandler Called with model [{JsonConvertExt.SerializeObject(domainEvent)}].");

            _bus.Publish(new TeamCreatedBusEvent
            {
                CompanyId = domainEvent.CompanyId,
                CorrelationId = domainEvent.CorrelationId,
                TeamId = domainEvent.TeamId,
                Name = domainEvent.Name,
                ActionBy = domainEvent.ActionBy
            });
        }
    }
}
