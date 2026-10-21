using System;
using DaysOn.Common.Domain.DomainEvents;

namespace DaysOn.CompanyBc.BoundedContext.Events
{
    public class TeamRemovedEvent : IDomainEvent
    {
        public readonly Guid CompanyId;
        public readonly Guid CorrelationId;
        public readonly Guid ActionBy;
        public readonly Guid TeamId;

        public TeamRemovedEvent(Guid teamId,  Guid companyId, Guid correlationId, Guid actionBy)
        {
            TeamId = teamId;
            CompanyId = companyId;
            CorrelationId = correlationId;
            ActionBy = actionBy;
        }
    }
}
