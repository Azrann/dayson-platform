using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using DaysOn.Facade.Tests.Acceptance.Helper;
using NUnit.Framework;
using Newtonsoft.Json;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class VersionControllerTests
    {
        [Test]
        public void WhenICallTheVersionMethod_ThenOkResponseReceived()
        {
            using (var client = new HttpClient())
            {
                Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings[BaseUrl.FacadeServer.ToString()]);

                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[BaseUrl.FacadeServer.ToString()]);

                var request = new StringContent(string.Empty);

                request.Headers.Clear();

                request.Headers.Add("Content-Type", "application/json");

                var requestUri = "version";

                Console.WriteLine("FullUrl={0}{1}", ConfigurationManager.AppSettings[BaseUrl.FacadeServer.ToString()], requestUri);

                var response = client.GetAsync(requestUri).Result;

                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Content: {0}", content);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        public class Wrapper
        {
            public string Message { get; set; }
        }
    }
}
