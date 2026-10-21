using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;
using Thinktecture.IdentityModel.Http;

namespace DaysOn.Auth.Test.Helper
{
    public static class ClientPostHelper
    {
       // private static string TestResourceServerUrl = "http://localhost:58707";
        private static string TestResourceServerUrl = "http://dayson-companybc.gear.host/";

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
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<TokenResponse>(responseString);
        }

        public static string AccessResource(TokenResponse token)
        {
            string ResourceUrl = $"{TestResourceServerUrl}/company/create";

            var client = new HttpClient();
            client.SetBearerToken(token.access_token);
            return client.GetStringAsync(new Uri(ResourceUrl)).Result;
        }
        
        //TODO Create user llamandon a api/audience/Register?  estamos locos?
        public static string CreateUser(string email, string pass)
        {
            var authServerUrl = ConfigurationManager.AppSettings["AuthServer"];
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{authServerUrl}/api/audience/Register");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    Email = email,
                    Password = pass,
                    ConfirmPassword = pass,
                    Claims = new List<object> {
                         new
                             {
                                 Key = "Admin",
                                 Value = "true"
                             }   
                        },
                    Roles = new List<string> ()
                });

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string result;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        // TODO cuantas veces he utilizado este metodo?s
        public static HttpResponseMessage RegisterAudience(string name)
        {
            var authServerUrl = ConfigurationManager.AppSettings["AuthServer"];

            const string requestUrl = "/api/audience";
            var requestObject = new
            {
                Name = name
            };
            return HttpClientHelper.Post(requestObject, authServerUrl, requestUrl).Result;
        }
    }
}
