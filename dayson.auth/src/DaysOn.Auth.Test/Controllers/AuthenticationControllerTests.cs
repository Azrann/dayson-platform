using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Auth.Domain;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Services;
using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Auth.Models.CommandResult;
using DaysOn.Auth.Web.Controllers;
using DaysOn.Common;
using DaysOn.Common.AzureBus;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DaysOn.Auth.Test.Controllers
{
    [TestFixture]
    public class AuthenticationControllerTests
    {
        private Mock<IAuthService> _authService;
        private Mock<IActivationService> _activationService;
        private Mock<IServiceBus> _serviceBusMock;
        private Mock<IUserIdService> _userIdServiceMock;
        private Mock<IPubnubMessageBus> _pubnubMessageBusMock;
        [SetUp]
        public void Setup()
        {
            _authService = new Mock<IAuthService>();
            _activationService = new Mock<IActivationService>();
            _serviceBusMock = new Mock<IServiceBus>();
            _userIdServiceMock = new Mock<IUserIdService>();
            _pubnubMessageBusMock = new Mock<IPubnubMessageBus>();
        }

        [Test]
        public void GivenValidEmailAndPasswordAndLanguageCode_WhenSentToAuthController_ThenOkResponseReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new AuthenticationController(_authService.Object, null, _serviceBusMock.Object,null, _userIdServiceMock.Object, _pubnubMessageBusMock.Object)
                                 {
                                     Request = requestMessage
                                 };

            var userRegistration = CreateUserRegistrationRequest("user@example.invalid", "en-GB");

            _authService.Setup(x => x.RegisterUserAsync(userRegistration.Email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(Task.FromResult(new RegisterUserCommandResult()
                                                     {
                                                         Succeeded = true
                                                     }));

            var result = controller.Register(userRegistration).Result;

            Assert.AreEqual(HttpStatusCode.OK, result.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }

        [Test]
        public void GivenEmailAndPasswordAndLanguageCode_WhenRegistrationFails_ThenBadRequestReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new AuthenticationController(_authService.Object, null, _serviceBusMock.Object,null, _userIdServiceMock.Object, _pubnubMessageBusMock.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var userRegistration = CreateUserRegistrationRequest("user@example.invalid", "en-GB");

            _authService.Setup(x => x.RegisterUserAsync(userRegistration.Email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(Task.FromResult(new RegisterUserCommandResult()
                                                     {
                                                         Succeeded = false,
                                                         Errors = new List<string>() {"error message"}
                                                     }));

            var result = controller.Register(userRegistration).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }

        [Test]
        public void GivenEmailAndPasswordAndLanguageCode_WhenRegistrationFailsWithNullResponse_InternalServerErrorReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new AuthenticationController(_authService.Object, null, _serviceBusMock.Object, null, _userIdServiceMock.Object, _pubnubMessageBusMock.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var userRegistration = CreateUserRegistrationRequest("user@example.invalid", "en-GB");

            _authService.Setup(x => x.RegisterUserAsync(userRegistration.Email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult((RegisterUserCommandResult)null));

            var result = controller.Register(userRegistration).Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }

        [Test]
        public void GivenEmailAndPasswordAndLanguageCode_WhenRegistrationFailsWithNoErrors_BadRequestReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new AuthenticationController(_authService.Object, null, _serviceBusMock.Object, null, _userIdServiceMock.Object, _pubnubMessageBusMock.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var userRegistration = CreateUserRegistrationRequest("user@example.invalid", "en-GB");

            _authService.Setup(x => x.RegisterUserAsync(userRegistration.Email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(Task.FromResult(new RegisterUserCommandResult()
                                                     {
                                                         Succeeded = false
                                                     }));

            var result = controller.Register(userRegistration).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.ExecuteAsync(CancellationToken.None).Result.StatusCode);
        }

        [Test]
        public void GivenInvalidEmailIsSentToAuthController_ThenBadRequestResponseReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new AuthenticationController(_authService.Object, null, _serviceBusMock.Object, null, _userIdServiceMock.Object, _pubnubMessageBusMock.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var userRegistration = CreateUserRegistrationRequest("test@.com", "en-GB");

            var result = controller.Register(userRegistration).Result.ExecuteAsync(CancellationToken.None).Result;

            var userRegistrationResult = JsonConvert.DeserializeObject<RegisterUserCommandResult>(result.Content.ReadAsStringAsync().Result);
            
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(1, userRegistrationResult.Errors.Count());
            Assert.AreEqual(ApiErrors.EmailMustBeAValidEmailAddress, userRegistrationResult.Errors.First());
        }

        [Test]
        public void GivenEmailEmptyIsSentToAuthController_ThenBadRequestResponseReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new AuthenticationController(_authService.Object, null, _serviceBusMock.Object, null, _userIdServiceMock.Object, _pubnubMessageBusMock.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var request = CreateUserRegistrationRequest(string.Empty, "en-GB");

            var result = controller.Register(request).Result.ExecuteAsync(CancellationToken.None).Result;

            var userRegistrationResult = JsonConvert.DeserializeObject<RegisterUserCommandResult>(result.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(1, userRegistrationResult.Errors.Count());
            Assert.AreEqual(ApiErrors.EmailMustBeSupplied, userRegistrationResult.Errors.First());
        }

        [Test]
        public void GivenEmailAlreadyRegisteredIsSentToAuthController_ThenBadRequestResponseReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new AuthenticationController(_authService.Object, null, _serviceBusMock.Object, null, _userIdServiceMock.Object, _pubnubMessageBusMock.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var request = CreateUserRegistrationRequest(string.Empty, "en-GB");

            var result = controller.Register(request).Result.ExecuteAsync(CancellationToken.None).Result;

            var userRegistrationResult = JsonConvert.DeserializeObject<RegisterUserCommandResult>(result.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(1, userRegistrationResult.Errors.Count());
            Assert.AreEqual(ApiErrors.EmailMustBeSupplied, userRegistrationResult.Errors.First());
        }

        [Test]
        public void GivenValidActivationCodeIsSentToAuthController_ThenOkIsReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new AuthenticationController(_authService.Object, _activationService.Object, _serviceBusMock.Object, null, _userIdServiceMock.Object, _pubnubMessageBusMock.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var result = controller.Activate(new ActivateUserCommand()
                {
                    ActivationCode = Guid.NewGuid()
                }).Result.ExecuteAsync(CancellationToken.None).Result; ;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void GivenInValidActivationCodeIsSentToAuthController_ThenBadRequestIsReturned()
        {
            var requestMessage = new HttpRequestMessage();

            var controller = new AuthenticationController(_authService.Object, _activationService.Object, _serviceBusMock.Object, null, _userIdServiceMock.Object, _pubnubMessageBusMock.Object)
            {
                Request = requestMessage,
                Configuration = new HttpConfiguration()
            };

            var result = controller.Activate(new ActivateUserCommand()).Result.ExecuteAsync(CancellationToken.None).Result; ;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
         
        private RegisterUserCommand CreateUserRegistrationRequest(string email, string languageCode)
        {
            return new RegisterUserCommand()
            {
                Email = email,
                LanguageCode = languageCode
            };
        }
    }

    public class WebResponse
    {
        public string Message { get; set; }
        public List<string> Errors
        {
            get
            {
                return JsonConvert.DeserializeObject<List<string>>(Message);
            }
        }
    }
}
