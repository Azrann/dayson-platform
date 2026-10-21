using System;

namespace DaysOn.Facade.Models
{
    public class EmployeeOverviewDto
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string Role { get; set; }
        public string PictureUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string OthersJson { get; set; }
        public bool HasApprovalRights { get; set; }
        public EmployeeStatus Status { get; set; }
    }

    public enum EmployeeStatus
    {
        Inactive = 0,
        InviteSent = 1,
        Active = 2
    }
}
