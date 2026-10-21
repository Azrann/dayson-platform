using System;

namespace DaysOn.Auth.Models.ApiCommand
{
    public class RegisterUserCommand : ApiCommand
    {
        public string Email { get; set; }

        public string LanguageCode { get; set; }

        public Guid EmployeeId { get; set; }

        public string Role { get; set; }
    }
}
