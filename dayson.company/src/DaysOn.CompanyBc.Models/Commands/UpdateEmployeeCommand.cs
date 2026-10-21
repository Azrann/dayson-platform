using System;

namespace DaysOn.CompanyBc.Models.Commands
{
    public class UpdateEmployeeCommand : ApiCommand
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public string JobTitle { get; set; }
        public string Role { get; set; }
        public string PictureUrl { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string OthersJson { get; set; }
    }
}
