using System;
using DaysOn.Common.Domain.DomainEvents;

namespace DaysOn.Auth.Domain.Events
{
    public class CheckRemoveManagerRoleEvent : IDomainEvent
    {
        public readonly Guid UserId;

        public CheckRemoveManagerRoleEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
