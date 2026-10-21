using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Thinktecture.IdentityModel.Http;

namespace DaysOn.CompanyBc.BoundedContext.Infrastructure.Helper
{
    public class HttpClientHelper
    {
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

        public async static Task<HttpResponseMessage> PostAsync(object model, string baseUrl, string requestUri, string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.SetBearerToken(token);

                var serializedRequest = JsonConvert.SerializeObject(model);

                var request = new StringContent(serializedRequest);

                request.Headers.Clear();

                request.Headers.Add("Content-Type", "application/json");

                return await client.PostAsync(requestUri, request);
            }
        }
    }
}
