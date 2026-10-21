using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web.Script.Serialization;
using DaysOn.Auth.Acceptance;
using DaysOn.Auth.Models.ApiCommand;

namespace DaysOn.Auth.Web.Helper
{
    public static class ClientPostHelper
    {
        public static TokenResponse GetToken(LoginCommand command)
        {
            var authServerUrl = ConfigurationManager.AppSettings["AuthServer"];

            string tokenUrlProvider = $"{authServerUrl}/oauth2/token";

            string responseString;

            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                    {
                        {"client_id", command.ClientId},
                        {"username", command.Username},
                        {"email", command.Username},
                        {"password", command.Password},
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
                    return new TokenResponse
                    {
                        error = ex.ToString()
                    };
                }
            }
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<TokenResponse>(responseString);
        }
    }
}