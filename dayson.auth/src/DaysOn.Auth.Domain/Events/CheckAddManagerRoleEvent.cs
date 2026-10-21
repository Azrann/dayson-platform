using System;
using DaysOn.Common.Domain.DomainEvents;

namespace DaysOn.Auth.Domain.Events
{
    public class CheckAddManagerRoleEvent : IDomainEvent
    {
        public readonly Guid UserId;

        public CheckAddManagerRoleEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
