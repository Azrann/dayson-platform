namespace DaysOn.Facade.Common.Dtos
{
    public class RegisterUserCommand : ApiCommand
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string LanguageCode { get; set; }

        public override string ToString()
        {
            return $"REPLACE_WITH_YOUR_VALUE";
        }
    }
}