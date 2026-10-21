using System;
using System.Configuration;
using System.Net;
using System.Threading;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.Facade.Common;
using NUnit.Framework;
using DaysOn.Facade.Tests.Acceptance.Helper;
using Newtonsoft.Json;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class AuthControllerTests
    {
        [Test]
        public void WhenILogInWithUsernameAndValidPassowrd_ThenOkResponseReceivedWithBearerToken()
        {
            const string requestUri = "v1/oauth2/token";
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings[BaseUrl.FacadeServer.ToString()]);

            var baseUrl = ConfigurationManager.AppSettings[BaseUrl.FacadeServer.ToString()];

            var loginRequest = new
            {
                username = "REPLACE_WITH_YOUR_VALUE",
                password = "REPLACE_WITH_YOUR_VALUE"
            };

            Console.WriteLine("FullUrl={0}{1}", baseUrl, requestUri);

            var response = HttpClientHelper.PostAsync(loginRequest, baseUrl, requestUri).Result;

            string content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Content: {0}", content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(content, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void WhenILogInWithUsernameAndInValidPassowrd_ThenOkResponseReceivedWithBearerToken()
        {
            const string requestUri = "v1/oauth2/token";
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings[BaseUrl.FacadeServer.ToString()]);

            var baseUrl = ConfigurationManager.AppSettings[BaseUrl.FacadeServer.ToString()];

            var loginRequest = new
            {
                username = "REPLACE_WITH_YOUR_VALUE",
                password = "REPLACE_WITH_YOUR_VALUE"
            };

            Console.WriteLine("FullUrl={0}{1}", baseUrl, requestUri);

            var response = HttpClientHelper.PostAsync(loginRequest, baseUrl, requestUri).Result;

            string content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Content: {0}", content);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Test]
        public void WhenIRegisterUse_ThenICanRetrieveAToken()
        {
            Console.WriteLine("Facade={0}", ConfigurationManager.AppSettings["FacadeServer"]);
            var companyName = NameHelper.GenerateCompanyName();
            var ownerName = NameHelper.GenerateEmployeeName();
            var employeeEmail = NameHelper.CreateEmail(ownerName, companyName);
            CreateCompanyCommand command = new CreateCompanyCommand()
            {
                CompanyName = companyName,
                AdminEmailAddress = employeeEmail,
                FirstName = ownerName,
                LastName = "Bolson",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
                OwnerId = Guid.NewGuid(),
                CorrelationId = Guid.NewGuid()
            };
            Guid companyId = CompanyClientWrapper.CreateCompanyAsync(command).Result;

            var token = AuthClientWrapper.GetTokenAsync(employeeEmail, command.OwnerId.GetHashCode().ToString()).Result;

            var employeeUserName = NameHelper.GenerateEmployeeName();
            var employeeUserEmail = NameHelper.CreateEmail(employeeUserName, companyName);

            var userId = AuthClientWrapper.RegisterUser(employeeUserEmail, "bobthebuilder123!", token.access_token).Result;

            Assert.AreNotEqual(Guid.Empty, userId);

            var tokenUser = AuthClientWrapper.GetTokenAsync(employeeUserEmail, "bobthebuilder123!").Result;

            Assert.IsNotEmpty(tokenUser.access_token);
        }
    }
    public class ResetPasswordCodeCommandResult 
    {
        public string ResetPasswordCode { get; set; }
    }

    public class Wrapper
        {
            public string Message { get; set; }
        }
}
