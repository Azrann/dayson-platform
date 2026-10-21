using System;
using DaysOn.Common.Domain.DomainEvents;

namespace DaysOn.Auth.Domain.Events
{
    public class UserRegisteredEvent : IDomainEvent
    {
        public readonly string UserEmailAddress;
        public readonly Guid Code;

        public UserRegisteredEvent(string userEmailAddress, Guid code)
        {
            UserEmailAddress = userEmailAddress;
            Code = code;
        }
    }
}