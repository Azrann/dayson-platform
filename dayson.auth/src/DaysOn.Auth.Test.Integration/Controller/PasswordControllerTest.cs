using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Castle.MicroKernel.Registration;
using DaysOn.Auth.Domain;
using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Auth.Models.CommandResult;
using DaysOn.Auth.Web;
using DaysOn.Common.AzureBus;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using DaysOn.Common.Domain.Ioc;
using Moq;

namespace DaysOn.Auth.Test.Integration.Controller
{
    [TestFixture]
    public class PasswordControllerTests
    {
        [Test]
        public void WhenIRequestPasswordResetCode_ThenCodeIsReturned()
        {
            var emailAddress = Guid.NewGuid() + "@test.com";
            using (var server = TestServer.Create<Startup>())
            {
                ContainerManager.Container.Register(Component.For<IServiceBus>().Instance(new Mock<IServiceBus>().Object).IsDefault().LifestyleTransient());

                var registerCommand = new RegisterAdminUserCommand
                {
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

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                var resetPasswordCodeCommand = new
                {
                    Email = emailAddress
                };

                request = JsonConvert.SerializeObject(resetPasswordCodeCommand,
                                                          new JsonSerializerSettings
                                                          {
                                                              ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                          });

                response = server.CreateRequest("v1/user/password/resetcode").And(r =>
                {
                    r.Content = new StringContent(request, Encoding.UTF8, "application/json");
                }).PostAsync().Result;

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Test]
        public void WhenIRequestPasswordResetCodeAndUserDoesntExists_ThenBadRequestReturned()
        {
            using (var server = TestServer.Create<Startup>())
            {
                var resetPasswordCodeCommand = new
                {
                    Email = "user@example.invalid"
                };

                var request = JsonConvert.SerializeObject(resetPasswordCodeCommand,
                                                          new JsonSerializerSettings
                                                          {
                                                              ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                          });

                var response = server.CreateRequest("v1/user/password/resetcode").And(r =>
                {
                    r.Content = new StringContent(request, Encoding.UTF8, "application/json");
                }).PostAsync().Result;

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Test]
        public void WhenIResetPasswordAndCodeDoesntExists_ThenBadRequestReturned()
        {
            using (var server = TestServer.Create<Startup>())
            {
                var resetPasswordCommand = new
                {
                    ResetPasswordToken = Guid.NewGuid().ToString(),
                    ConfirmPassword = "REPLACE_WITH_YOUR_VALUE",
                    Password = "REPLACE_WITH_YOUR_VALUE",
                };

                var request = JsonConvert.SerializeObject(resetPasswordCommand,
                                                          new JsonSerializerSettings
                                                          {
                                                              ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                          });

                var response = server.CreateRequest("v1/user/password/reset").And(r =>
                {
                    r.Content = new StringContent(request, Encoding.UTF8, "application/json");
                }).PostAsync().Result;

                var result = response.Content.ReadAsStringAsync().Result;
                var resetPasswordCodeCommandResult = JsonConvert.DeserializeObject<CommandResult>(result);

                Console.WriteLine("Status code:{0}", response.StatusCode);
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                Console.WriteLine("Command result:{0}", resetPasswordCodeCommandResult.Succeeded);
                Assert.IsFalse(resetPasswordCodeCommandResult.Succeeded);
                Console.WriteLine("Error message:{0}", resetPasswordCodeCommandResult.Errors.ToList()[0]);
                Assert.AreEqual(string.Format(ApiErrors.InvalidResetPasswordToken, resetPasswordCommand.ResetPasswordToken), resetPasswordCodeCommandResult.Errors.ToList()[0]);
            }
        }

        [TearDown]
        public void TearDown()
        {
            ContainerManager.ReleaseAll();
        }
    }
}
