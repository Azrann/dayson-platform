using System;

namespace DaysOn.CompanyBc.Models.Commands
{
    public class CreateCompanyCommand : ApiCommand
    {
        public string AdminEmailAddress { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid CompanyId { get; set; }
        public Guid OwnerId { get; set; }
    }
}
