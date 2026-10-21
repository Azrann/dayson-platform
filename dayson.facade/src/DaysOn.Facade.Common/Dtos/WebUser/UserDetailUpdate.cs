using System;
using DaysOn.Facade.Models.WebModels.User;

namespace DaysOn.Facade.Common.Dtos.WebUser
{
    public class UserDetailUpdate
    {
        public Guid TeamId { get; set; }
        public Guid? ApproverId { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PictureUrl { get; set; }

        public UserType Type { get; set; }
        public bool IsApprover { get; set; }

        public UserRules Rules { get; set; }
    }
}
