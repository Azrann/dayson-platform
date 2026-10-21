using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using DaysOn.Auth.Acceptance;
using DaysOn.Auth.Acceptance.Helper;
using DaysOn.CompanyBc.Test.Common;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DaysOn.Auth.Test
{
    [TestFixture]
    public class AuthenticationClientTest
    {
        private const string TheUserNameOrPasswordIsIncorrect = "The user name or password is incorrect.";

        private HttpClientHelper httpClientHelper;

        [SetUp]
        public void Initialise()
        {
            httpClientHelper = new HttpClientHelper();
        }

        [Test]
        public void WhenIRegisterWithValidEmailAndPassword_ThenOkResponseReceived()
        {
            var email = Guid.NewGuid() + "@test.com";

            TokenResponse response = UserHelper.RegisterAdmin(email);
            Assert.IsNotNullOrEmpty(response.access_token);
        }

        [Test]
        public void Given_Username_Password_I_Can_Retrieve_A_Token()
        {
            var response = UserHelper.GetToken("REPLACE_WITH_YOUR_VALUE", "REPLACE_WITH_YOUR_VALUE");
            Assert.IsNull(response.error);
            Assert.IsNotNull(response.access_token);
        }

        [Test]
        public void GivenUsernamePassword_ThenICanRetrieveATokenUsingAuthController()
        {
            const string requestUrl = "/v1/oauth";
            var requestObject = new 
            {
                 username = "REPLACE_WITH_YOUR_VALUE",
                 password = "REPLACE_WITH_YOUR_VALUE",
                 clientId = "REPLACE_WITH_YOUR_VALUE"
            };
            var baseUrl = ConfigurationManager.AppSettings["AuthServer"];
            var responseString = httpClientHelper.PostAsync(requestObject, baseUrl, requestUrl).Result.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<TokenResponse>(responseString);

            Assert.IsNotNull(result.access_token);
        }

        [Test]
        public void Given_Username_Wrong_Password_I_Cannot_Retrieve_A_Token()
        {
            var response = UserHelper.GetToken("REPLACE_WITH_YOUR_VALUE", "Alberto2");
            Assert.IsNotNull(response.error);
            Assert.AreEqual("invalid_grant", response.error);
            Assert.IsNull(response.access_token);
        }

        [Test]
        public void Given_A_Username_Password_I_Can_Create_A_Normal_User()
        {
            string company = NameHelper.GenerateCompanyName();
            string email = NameHelper.CreateEmail(NameHelper.GenerateEmployeeName(), company);
            string userEmail = NameHelper.CreateEmail(NameHelper.GenerateEmployeeName(), company);
            string userId;
            var token = UserHelper.RegisterAdmin(email);

            var response = UserHelper.RegisterNormalUser(out userId, userEmail, token.access_token);
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNullOrEmpty(userId);
        }
    }
}
