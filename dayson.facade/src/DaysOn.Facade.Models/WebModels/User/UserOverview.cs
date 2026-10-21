using System;
using System.Linq;

namespace DaysOn.Facade.Models.WebModels.User
{
    public class UserOverview
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid TeamId { get; set; }
        public Guid? ApproverId { get; set; }

        // user data
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PictureUrl { get; set; }

        public UserType Type { get; set; }
        public bool IsApprover { get; set; }

        // from team table, necessary for searches and stuff
        public string TeamName { get; set; }

        public string FullName => string.Join(" ", new [] { FirstName, MiddleName, LastName  }.Where(s => !string.IsNullOrEmpty(s)));
    }
}
