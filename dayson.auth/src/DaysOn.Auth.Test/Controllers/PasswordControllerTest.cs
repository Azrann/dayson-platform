using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using DaysOn.Auth.Domain.Services;
using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Auth.Models.CommandResult;
using DaysOn.Auth.Web.Controllers;
using DaysOn.Common.AzureBus;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json;

namespace DaysOn.Auth.Test.Controllers
{
    [TestFixture]
    public class PasswordControllerTest
    {
        private Mock<IResetPasswordService> _resetPasswordService;
        private Mock<IServiceBus> _serviceBusMock;
        private Mock<INonActiveUsersService> _nonActiveUsersService;

        [SetUp]
        public void Setup()
        {
            _resetPasswordService = new Mock<IResetPasswordService>();
            _serviceBusMock = new Mock<IServiceBus>();
            _nonActiveUsersService = new Mock<INonActiveUsersService>();
        }
        [Test]
        public void GivenValidResetPasswordCommand_OkResponseIsReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new PasswordController(_resetPasswordService.Object, _serviceBusMock.Object, _nonActiveUsersService.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var result = controller.ResetCode(new ResetPasswordCodeCommand() { Email = "user@example.invalid" }).Result.ExecuteAsync(CancellationToken.None).Result; ;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void GivenValidResetPasswordCommand_ResetPasswordCodeIsReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new PasswordController(_resetPasswordService.Object, _serviceBusMock.Object, _nonActiveUsersService.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var result = controller.ResetCode(new ResetPasswordCodeCommand() { Email = "user@example.invalid" }).Result.ExecuteAsync(CancellationToken.None).Result; ;

            Assert.AreEqual(HttpStatusCode.OK,result.StatusCode);
        }

        [Test]
        public void GivenInValidResetPasswordCommand_BadRequestIsReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new PasswordController(_resetPasswordService.Object, _serviceBusMock.Object, _nonActiveUsersService.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var result = controller.ResetCode(new ResetPasswordCodeCommand() { Email = "testtest.com" }).Result.ExecuteAsync(CancellationToken.None).Result; ;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void GivenInValidResetPasswordCommand_ErrorMessageIsReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new PasswordController(_resetPasswordService.Object, _serviceBusMock.Object, _nonActiveUsersService.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var result = controller.ResetCode(new ResetPasswordCodeCommand() { Email = "testtest.com" }).Result.ExecuteAsync(CancellationToken.None).Result; ;
            var userRegistrationResult = JsonConvert.DeserializeObject<ResetPasswordCodeCommandResult>(result.Content.ReadAsStringAsync().Result);

            Assert.IsFalse(userRegistrationResult.Succeeded);
            Assert.AreEqual("'Email' is not a valid email address.", userRegistrationResult.Errors.ToList()[0]);
        }

        [Test]
        public void GivenInValidCodeForResetPasswordCommand_ErrorMessageIsReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new PasswordController(_resetPasswordService.Object, _serviceBusMock.Object, _nonActiveUsersService.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var result = controller.Reset(new ResetPasswordCommand() { Password = "REPLACE_WITH_YOUR_VALUE", ConfirmPassword = "REPLACE_WITH_YOUR_VALUE" }).Result.ExecuteAsync(CancellationToken.None).Result; ;
            var userRegistrationResult = JsonConvert.DeserializeObject<CommandResult>(result.Content.ReadAsStringAsync().Result);

            Assert.IsFalse(userRegistrationResult.Succeeded);
            Assert.AreEqual("Reset password token is empty or invalid", userRegistrationResult.Errors.ToList()[0]);
        }

        [Test]
        public void GivenValidCodeForResetPasswordCommand_ResetServiceIsCalled()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new PasswordController(_resetPasswordService.Object, _serviceBusMock.Object, _nonActiveUsersService.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var result = controller.Reset(new ResetPasswordCommand() { Password = "REPLACE_WITH_YOUR_VALUE", ConfirmPassword = "REPLACE_WITH_YOUR_VALUE", ResetPasswordToken = Guid.NewGuid() }).Result.ExecuteAsync(CancellationToken.None).Result; ;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            _resetPasswordService.Verify(x => x.ResetPasswordAsync(It.IsAny<ResetPasswordCommand>()));
        }
    }
}

