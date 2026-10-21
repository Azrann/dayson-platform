using System;

namespace DaysOn.Auth.Models.ApiCommand
{
    public class ActivateUserCommand : ApiCommand
    {
        public Guid ActivationCode { get; set; }
    }
}
