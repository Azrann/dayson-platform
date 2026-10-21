using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using DaysOn.Auth.Models.ApiCommand;
using Newtonsoft.Json;

namespace DaysOn.Facade.Common
{
    public static class AuthClientWrapper
    {
        private const string GetTokenRequestUri = "v1/oauth";
        private const string GetResetPasswordToken = "v1/user/password/resetcode";
        private const string PostResetPassword = "v1/user/password/reset";
        private const string GetUsersActivationStatusUri = "v1/user/activationStatus/all";
        private const string GetNonActiveUsersRequest = "v1/user/nonactive/all";

        public static async Task<TokenResponse> GetTokenAsync(string username, string pass, string clientIdentity = "REPLACE_WITH_YOUR_VALUE")
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["AuthServer"]);

            var baseUrl = ConfigurationManager.AppSettings["AuthServer"];

            var loginRequest = new
            {
                username,
                password = pass,
                clientId = clientIdentity
            };

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetTokenRequestUri);

            var response = await HttpClientHelper.PostAsync(loginRequest, baseUrl, GetTokenRequestUri);

            string responseString = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<TokenResponse>(responseString);
        }

        public static async Task<HttpResponseMessage> GetUsersActivationStatus(string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["AuthServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, GetUsersActivationStatusUri, token);

            return response;
        }

        public static async Task<TokenResponse> GetTokenAsync(string clientIdentity = "REPLACE_WITH_YOUR_VALUE")
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["AuthServer"]);

            var baseUrl = ConfigurationManager.AppSettings["AuthServer"];

            var loginRequest = new
            {
                username = "REPLACE_WITH_YOUR_VALUE",
                password = "REPLACE_WITH_YOUR_VALUE",
                clientId = clientIdentity
            };

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetTokenRequestUri);

            var response = await HttpClientHelper.PostAsync(loginRequest, baseUrl, GetTokenRequestUri);

            string responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenResponse>(responseString);
        }

        public static async Task<HttpResponseMessage> RegisterUser(string emailAddress, string pass, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["AuthServer"]);

            var baseUrl = ConfigurationManager.AppSettings["AuthServer"];
            var requestUri = "v1/user/register";

            var registrationRequest = new
            {
                correlationId = Guid.NewGuid(),
                email = emailAddress,
                password = pass,
                confirmPassword = pass,
                languageCode = "en-GB"
            };
            Console.WriteLine("FullUrl={0}{1}", baseUrl, requestUri);
             
            var response = await HttpClientHelper.PostAsync(registrationRequest, baseUrl, requestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> GetPasswordResetCode(string email)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["AuthServer"]);

            var baseUrl = ConfigurationManager.AppSettings["AuthServer"];
            
            ResetPasswordCodeCommand request = new ResetPasswordCodeCommand
            {
                CorrelationId = Guid.NewGuid(),
                Email = email
            };
            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetResetPasswordToken);

            var response = await HttpClientHelper.PostAsync(request, baseUrl, GetResetPasswordToken);

            return response;
        }

        public static async Task<HttpResponseMessage> ResetPassword(string newPass, string code)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["AuthServer"]);

            var baseUrl = ConfigurationManager.AppSettings["AuthServer"];

            ResetPasswordCommand request = new ResetPasswordCommand
            {
                CorrelationId = Guid.NewGuid(),
                Password = newPass,
                ConfirmPassword = newPass,
                ResetPasswordToken = new Guid(code)
            };
            Console.WriteLine("FullUrl={0}{1}", baseUrl, PostResetPassword);

            var response = await HttpClientHelper.PostAsync(request, baseUrl, PostResetPassword);

            return response;
        }

        public static async Task<HttpResponseMessage> GetNonActiveUsers(string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["AuthServer"];
            var response = await HttpClientHelper.GetAsync(baseUrl, GetNonActiveUsersRequest, token);

            return response;
        }

        
    }
}
