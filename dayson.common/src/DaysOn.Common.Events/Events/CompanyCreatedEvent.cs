using System;

namespace DaysOn.Common.Events.Events
{
    public class CompanyCreatedBusEvent : BaseEvent
    {
        public string AdminEmailAddress { get; set; }
        public Guid CompanyId { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public CompanyCreatedBusEvent()
        {
        }

        public CompanyCreatedBusEvent(Guid companyId, Guid ownerId, string name, string adminEmailAddress, string firstName, string lastName)
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
