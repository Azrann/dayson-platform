using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Thinktecture.IdentityModel.Http;

namespace DaysOn.Functions.Test.Common
{
    public class HttpClientHelper
    {
        public async Task<HttpResponseMessage> PostAsync(object model, string baseUrl, string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                var serializedRequest = JsonConvert.SerializeObject(model,
                                                          new JsonSerializerSettings
                                                          {
                                                              ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                          });

                var request = new StringContent(serializedRequest);

                request.Headers.Clear();

                request.Headers.Add("Content-Type", "application/json");

                return await client.PostAsync(requestUri, request);
            }
        }

        public static HttpResponseMessage Post(object model, string baseUrl, string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                var serializedRequest = JsonConvert.SerializeObject(model);

                var request = new StringContent(serializedRequest);

                request.Headers.Clear();

                request.Headers.Add("Content-Type", "application/json");

                return client.PostAsync(requestUri, request).Result;
            }
        }


        public static async Task<HttpResponseMessage> GetAsync(string token, string baseUrl, string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.SetBearerToken(token);

                return await client.GetAsync(string.Format("{0}{1}", baseUrl, requestUri));
            }
        }
    }
}
