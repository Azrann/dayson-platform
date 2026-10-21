using System;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Http;

namespace DaysOn.Facade.Common
{
    public class HttpClientHelper
    {
        public static async Task<HttpResponseMessage> PostAsync(object model, string baseUrl, string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                var serializedRequest = JsonConvert.SerializeObject(model);

                var request = new StringContent(serializedRequest);

                request.Headers.Clear();

                request.Headers.Add("Content-Type", "application/json");

                return await client.PostAsync(requestUri, request);
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(object model, string baseUrl, string requestUri, string token = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(token))
                {
                    client.SetBearerToken(token);
                }

                var serializedRequest = JsonConvert.SerializeObject(model);

                var request = new StringContent(serializedRequest);

                request.Headers.Clear();

                request.Headers.Add("Content-Type", "application/json");

                return await client.PostAsync(requestUri, request);
            }
        }

        public static async Task<HttpResponseMessage> GetAsync(string baseUrl, string requestUri)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"{baseUrl}{requestUri}"),
                    Method = HttpMethod.Get,
                };
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return await client.SendAsync(request);
            }
        }

        public static async Task<HttpResponseMessage> GetAsync(string baseUrl, string requestUri, string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.SetBearerToken(token);

                return await client.GetAsync(requestUri);
            }
        }
    }
}