using System;
using DaysOn.Common.Domain.DomainEvents;

namespace DaysOn.CompanyBc.BoundedContext.Events
{
    public class TeamUpdatedEvent : IDomainEvent
    {
        public readonly Guid CompanyId;
        public readonly Guid CorrelationId;
        public readonly Guid ActionBy;
        public readonly Guid TeamId;
        public readonly string Name;

        public TeamUpdatedEvent(Guid teamId, Guid companyId, string name, Guid correlationId, Guid actionBy)
        {
            TeamId = teamId;
            CompanyId = companyId;
            CorrelationId = correlationId;
            ActionBy = actionBy;
            Name = name;
        }
    }
}