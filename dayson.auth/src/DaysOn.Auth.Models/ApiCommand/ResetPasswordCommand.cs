using System;

namespace DaysOn.Auth.Models.ApiCommand
{
    public class ResetPasswordCommand : ApiCommand
    {
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public Guid ResetPasswordToken { get; set; }
    }
}
