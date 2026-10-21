using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DaysOn.Auth.Test.Helper
{
    public class HttpClientHelper
    {
        public static async Task<HttpResponseMessage> Post(object model, string baseUrl, string requestUri)
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

        public static async Task<HttpResponseMessage> Get(string baseUrl, string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                return await client.GetAsync(requestUri);
            }
        }
    }
}
