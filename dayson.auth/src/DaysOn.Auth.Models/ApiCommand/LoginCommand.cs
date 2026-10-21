namespace DaysOn.Auth.Models.ApiCommand
{
    public class LoginCommand : ApiCommand 
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ClientId { get; set; }
    }
}