namespace DaysOn.Auth.Models.ApiCommand
{
    public class ResetPasswordCodeCommand : ApiCommand
    {
        public string Email { get; set; }
    }
}
