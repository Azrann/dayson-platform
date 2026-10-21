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
    public class EmployeeCreatedEventHandler : IEventHandler<EmployeeCreatedEvent>
    {
        private readonly IServiceBus _bus;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public EmployeeCreatedEventHandler(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Handle(EmployeeCreatedEvent domainEvent)
        {
            _logger.LogInf(
                $"EmployeeCreatedDomainEventHandler Called with model [{JsonConvertExt.SerializeObject(domainEvent)}].");

            _bus.Publish(new EmployeeCreatedBusEvent
            {
                EmployeeEmailAddress = domainEvent.AdminEmailAddress,
                CompanyId = domainEvent.CompanyId,
                CorrelationId = domainEvent.CorrelationId,
                EmployeeId = domainEvent.EmployeeId,
                Role = domainEvent.Role,
                FirstName = domainEvent.FirstName,
                LastName = domainEvent.LastName
            });
        }
    }
}
