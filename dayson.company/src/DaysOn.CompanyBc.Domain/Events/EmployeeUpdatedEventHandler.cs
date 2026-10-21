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
    public class EmployeeUpdatedEventHandler : IEventHandler<EmployeeUpdatedEvent>
    {
        private readonly IServiceBus _bus;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public EmployeeUpdatedEventHandler(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Handle(EmployeeUpdatedEvent domainEvent)
        {
            _logger.LogInf(
                $"EmployeeUpdatedDomainEventHandler Called with model [{JsonConvertExt.SerializeObject(domainEvent)}].");

            _bus.Publish(new EmployeeUpdatedBusEvent
            {
                EmployeeEmailAddress = domainEvent.AdminEmailAddress,
                CompanyId = domainEvent.CompanyId,
                CorrelationId = domainEvent.CorrelationId,
                EmployeeId = domainEvent.EmployeeId,
                Role = domainEvent.Role
            });
        }
    }
}
