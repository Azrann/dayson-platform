using System;
using DaysOn.Common.Domain.DomainEvents;

namespace DaysOn.CompanyBc.BoundedContext.Events
{
    public class CompanyCreatedEvent : IDomainEvent
    {
        public readonly string Name;
        public readonly string AdminEmailAddress;
        public readonly Guid CompanyId;
        public readonly Guid OwnerId;
        public readonly string FirstName;
        public readonly string LastName;

        public CompanyCreatedEvent(Guid companyId, string name, string adminEmailAddress, Guid ownerId, string firstName, string lastName)
        {
            CompanyId = companyId;
            Name = name;
            AdminEmailAddress = adminEmailAddress;
            OwnerId = ownerId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}