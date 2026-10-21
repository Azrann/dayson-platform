using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;

namespace DaysOn.Auth.Acceptance.Helper
{
    public class UserHelper
    {
        public static HttpResponseMessage RegisterNormalUser(out string userId, string email, string token)
        {
            const string requestUri = "v1/user/register";
            var baseUrl = ConfigurationManager.AppSettings["TestEndpoint"];

            Console.WriteLine("TestEndpoint={0}", baseUrl);

            var registrationRequest = new
            {
                CorrelationId = Guid.NewGuid(),
                Email = email,
                LanguageCode = "en-GB"
            };

            HttpResponseMessage response = new HttpClientHelper().PostAsyncWithToken(token,registrationRequest, baseUrl, requestUri).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;
            dynamic result = JsonConvert.DeserializeObject(responseString);

            userId = result.UserId?.ToString();

            return response;
        }

        public static TokenResponse RegisterAdmin(string email)
        {
            const string requestUri = "v1/user/registerAdmin";
            var baseUrl = ConfigurationManager.AppSettings["TestEndpoint"];

            Console.WriteLine("TestEndpoint={0}", baseUrl);

            var registrationRequest = new
            {
                CorrelationId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid().ToString(),
                Email = email,
                LanguageCode = "en-GB"
            };

            HttpResponseMessage response = new HttpClientHelper().PostAsync(registrationRequest, baseUrl, requestUri).Result;

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(response.Content.ReadAsStringAsync().Result);

            return token;
        }

        public static HttpResponseMessage Activate(string activationCode)
        {
            const string activateUri = "v1/user/activate";
            var baseUrl = ConfigurationManager.AppSettings["TestEndpoint"];

            Console.WriteLine("TestEndpoint={0}", baseUrl);
            Console.WriteLine("FullUrl={0}{1}", baseUrl, activateUri);

            var activationRequest = new
            {
                CorrelationId = Guid.NewGuid().ToString(),
                ActivationCode = activationCode
            };

            HttpResponseMessage activationResponse = new HttpClientHelper().PostAsync(activationRequest, baseUrl, activateUri).Result;

            string content = activationResponse.Content.ReadAsStringAsync().Result;

            Console.WriteLine("Content: {0}", content);
            return activationResponse;
        }

        public static HttpResponseMessage ResetPassword(string resetPasswordCode, string newPassword)
        {
            const string requestUri = "v1/user/password/reset";
            var baseUrl = ConfigurationManager.AppSettings["TestEndpoint"];

            Console.WriteLine("TestEndpoint={0}", baseUrl);

            var resetPasswordRequest = new
            {
                correlationId = Guid.NewGuid(),
                password = newPassword,
                confirmPassword = newPassword,
                ResetPasswordToken = resetPasswordCode
            };

            HttpResponseMessage response = new HttpClientHelper().PostAsync(resetPasswordRequest, baseUrl, requestUri).Result;

            return response;
        }

        public static TokenResponse GetToken(string username, string pass, string clientId = "REPLACE_WITH_YOUR_VALUE")
        {
            var authServerUrl = ConfigurationManager.AppSettings["AuthServer"];

            string tokenUrlProvider = $"{authServerUrl}/oauth2/token";

            string responseString;

            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                    {
                        {"client_id", clientId},
                        {"username", username},
                        {"email", username},
                        {"password", pass},
                        {"grant_type", "password"}
                    };

                var content = new FormUrlEncodedContent(values);

                try
                {
                    var response = client.PostAsync(tokenUrlProvider, content).Result;
                    responseString = response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception ex)
                {
                    return new TokenResponse()
                    {
                        error = ex.ToString()
                    };
                }
            }
            return JsonConvert.DeserializeObject<TokenResponse>(responseString);
        }
    }
}
