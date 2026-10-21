using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Auth.Domain.Services;

namespace DaysOn.Auth.Test.Application
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IUserManagerWrapper> _userManagerWrapper;
        private Mock<IResetPasswordService> _resetPasswordService;
        private Mock<INonActiveUsersService> _nonActiveUsersService;

        [SetUp]
        public void Setup()
        {
            _userManagerWrapper = new Mock<IUserManagerWrapper>();
            _resetPasswordService = new Mock<IResetPasswordService>();
            _nonActiveUsersService = new Mock<INonActiveUsersService>();
        }

        [Test]
        public void WhenICallRegisterUser_WithValidEmailAndPassword_ThenUserIsRegistered()
        {
            _userManagerWrapper.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Success));
            _resetPasswordService.Setup(x => x.CreateResetPasswordCodeAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
	        _userManagerWrapper
		        .Setup(x => x.AddClaimAsync(It.IsAny<string>(), It.IsAny<System.Security.Claims.Claim>(), It.IsAny<UserManager<User>>()))
		        .ReturnsAsync(IdentityResult.Success);
	        _userManagerWrapper
		        .Setup(x => x.AddToRoleAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserManager<User>>()))
		        .ReturnsAsync(IdentityResult.Success);

			
			var authService = new AuthService(_userManagerWrapper.Object, _resetPasswordService.Object, new UserManagerFactory(), _nonActiveUsersService.Object);

            var result = authService.RegisterUserAsync("user@example.invalid", "1234567").Result;

            Assert.That(result.Succeeded);
        }

        [Test]
        public void WhenICallRegisterUser_WithValidEmailAndPassword_AndRegistrationFails_ThenErrorIsReturned()
        {
            _userManagerWrapper.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(Task.FromResult(new IdentityResult("error message")));

            var authService = new AuthService(_userManagerWrapper.Object, _resetPasswordService.Object, new UserManagerFactory(), _nonActiveUsersService.Object);

            var result = authService.RegisterUserAsync("user@example.invalid", "1234567").Result;
            
            Assert.IsNotNullOrEmpty(result.Errors.Single(x => x == "error message"));
        }
    }
}
