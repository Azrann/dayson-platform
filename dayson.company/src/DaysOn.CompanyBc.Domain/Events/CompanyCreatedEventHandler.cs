using System.Reflection;
using DaysOn.Common;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Events.Events;
using DaysOn.Common.Utilities.Extension;
using DaysOn.Common.Domain.DomainEvents;
using log4net;

namespace DaysOn.CompanyBc.BoundedContext.Events
{
    public class CompanyCreatedEventHandler : IEventHandler<CompanyCreatedEvent>
    {
        private readonly IServiceBus _bus;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CompanyCreatedEventHandler(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Handle(CompanyCreatedEvent domainEvent)
        {
            _logger.LogInf("CompanyCreatedDomainEventHandler Called.");
            _bus.Publish(new CompanyCreatedBusEvent(domainEvent.CompanyId, domainEvent.OwnerId, domainEvent.Name, domainEvent.AdminEmailAddress, domainEvent.FirstName, domainEvent.LastName));
        }
    }
}
