using System.Reflection;
using DaysOn.Common;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.Common.Events.Events;
using DaysOn.Common.Utilities.Extension;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.CompanyBc.BoundedContext.Events
{
    public class TeamRemovedEventHandler : IEventHandler<TeamRemovedEvent>
    {
        private readonly IServiceBus _bus;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TeamRemovedEventHandler(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Handle(TeamRemovedEvent domainEvent)
        {
            _logger.LogInf(
                $"TeamRemovedDomainEventHandler Called with model [{JsonConvertExt.SerializeObject(domainEvent)}].");

            _bus.Publish(new TeamRemovedBusEvent
            {
                CompanyId = domainEvent.CompanyId,
                CorrelationId = domainEvent.CorrelationId,
                TeamId = domainEvent.TeamId,
                ActionBy = domainEvent.ActionBy
            });
        }
    }
}
