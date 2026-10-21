namespace DaysOn.Auth.Models.ApiCommand
{
    public class RegisterAdminUserCommand : ApiCommand
    {
        public string Email { get; set; }

        public string LanguageCode { get; set; }

        public string CompanyId { get; set; }
    }
}
