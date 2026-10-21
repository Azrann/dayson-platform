namespace DaysOn.HolidaysBc.Test.Common
{
    public class TokenResponse
    {
        public string access_token { get; set; }

        public string expires_in { get; set; }

        public string token_type { get; set; }

        public string error { get; set; }

        public string error_description { get; set; }
    }
}
