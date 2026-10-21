using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Castle.MicroKernel.Registration;
using DaysOn.Auth.Acceptance;
using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Auth.Models.CommandResult;
using DaysOn.Auth.Test.Integration.Helper;
using DaysOn.Auth.Web;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Domain.Ioc;
using Microsoft.Owin.Testing;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DaysOn.Auth.Test.Integration.Controller
{
        [TestFixture]
        public class AuthenticatonControllerTests
        {
            [Test]
            public void WhenIRegisterAUserWithAnEmailThatAlreadyExists_ThenOkResponseisReturned()
            {
                var emailAddress = Guid.NewGuid() + "@test.com";
                using (var server = TestServer.Create<Startup>())
                {
                    ContainerManager.Container.Register(Component.For<IServiceBus>().Instance(new Mock<IServiceBus>().Object).IsDefault().LifestyleTransient());

                    var registerCommand = new RegisterAdminUserCommand()
                    {
                        CorrelationId = Guid.NewGuid(),
                        Email = emailAddress,
                        LanguageCode = "en-GB",
                        CompanyId = Guid.NewGuid().ToString()
                    };

                    var request = JsonConvert.SerializeObject(registerCommand,
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });

                    var response = server.CreateRequest("v1/user/registerAdmin").And(r =>
                    {
                        r.Content = new StringContent(request, Encoding.UTF8, "application/json");
                    }).PostAsync().Result;

                    var result = response.Content.ReadAsStringAsync().Result;
                    var userRegistrationResult = JsonConvert.DeserializeObject<RegisterUserCommandResult>(result);

                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                    response = server.CreateRequest("v1/user/registerAdmin").And(r =>
                    {
                        r.Content = new StringContent(request, Encoding.UTF8, "application/json");
                    }).PostAsync().Result;

                    result = response.Content.ReadAsStringAsync().Result;
                    userRegistrationResult = JsonConvert.DeserializeObject<RegisterUserCommandResult>(result);
                    var errors = userRegistrationResult.Errors as List<string>;
                    Assert.AreEqual($"Name {emailAddress} is already taken.", errors[0]);
                }
            }

            [Test]
            public void Version()
            {
                using (var server = TestServer.Create<Startup>())
                {
                    var response = server.CreateRequest("version").GetAsync().Result;

                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                }
            }

            [Test]
            public void WhenIActivateUserWithInValidActivationCode_ThenErrorResponseisReturned()
            {
                using (var server = TestServer.Create<Startup>())
                {
                    var registerCommand = new
                    {
                    };

                    var request = JsonConvert.SerializeObject(registerCommand,
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });

                    var response = server.CreateRequest("v1/user/activate").And(r =>
                    {
                        r.Content = new StringContent(request, Encoding.UTF8, "application/json");
                    }).PostAsync().Result;

                    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                }
            }

            [Test]
            public void WhenIActivateUserThatDoesNotExist_ThenErrorResponseisReturned()
            {
                using (var server = TestServer.Create<Startup>())
                {
                    var registerCommand = new
                    {
                        ActivationCode = Guid.NewGuid()
                    };

                    var request = JsonConvert.SerializeObject(registerCommand,
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });

                    var response = server.CreateRequest("v1/user/activate").And(r =>
                    {
                        r.Content = new StringContent(request, Encoding.UTF8, "application/json");
                    }).PostAsync().Result;

                    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                }
            }

            [Test]
            public void WhenIRequestATokenWithValidUsernameAndPassword_ThenTokenIsReturned()
            {
                using (var server = TestServer.Create<Startup>())
                {
                    var token = ClientPostHelper.GetToken("REPLACE_WITH_YOUR_VALUE", "REPLACE_WITH_YOUR_VALUE");

                    Assert.IsNotNullOrEmpty(token.access_token);
                }
            }

            [Test]
            public void WhenIRequestATokenWithValidUsernameAndPasswordUsingController_ThenTokenIsReturned()
            {
                using (var server = TestServer.Create<Startup>())
                {
                    var getTokenCommand = new
                    {
                        username = "REPLACE_WITH_YOUR_VALUE",
                        password = "REPLACE_WITH_YOUR_VALUE",
                        clientId = "REPLACE_WITH_YOUR_VALUE"
                    };

                    var request = JsonConvert.SerializeObject(getTokenCommand,
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });

                    var response = server.CreateRequest("v1/oauth").And(r =>
                    {
                        r.Content = new StringContent(request, Encoding.UTF8, "application/json");
                    }).PostAsync().Result;

                    var token = JsonConvert.DeserializeObject<TokenResponse>(response.Content.ReadAsStringAsync().Result);
                    
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                    Assert.IsNotNullOrEmpty(token.access_token);
                }
            }

            [TearDown]
            public void TearDown()
            {
                ContainerManager.ReleaseAll();
            }
    }
}
