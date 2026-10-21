using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using System;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Auth.Domain.Services;

namespace DaysOn.Auth.Test.Domain.Services
{
    [TestFixture]
    public class AuthServiceTest
    {
        private Mock<IUserManagerWrapper> _mockUserManager;
        private IAuthService _authService;
        private Mock<IResetPasswordService> _resetPasswordService;
        private Mock<INonActiveUsersService> _nonActiveUsersService;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _mockUserManager = new Mock<IUserManagerWrapper>();
            _resetPasswordService = new Mock<IResetPasswordService>();
            _nonActiveUsersService = new Mock<INonActiveUsersService>();
        }

        [Test]
        public void WhenICallCreateOwnerUserThenCreateUserAndAddRoleAreCalled()
        {
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), "cat123", It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            _mockUserManager.Setup(x => x.AddClaimAsync(It.IsAny<string>(), It.IsAny<Claim>(), It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<string>(), "Owner", It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            _resetPasswordService.Setup(x => x.CreateResetPasswordCodeAsync(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            _authService = new AuthService(_mockUserManager.Object, _resetPasswordService.Object, new UserManagerFactory(), _nonActiveUsersService.Object);

            _authService.RegisterUserAsync("user@example.invalid", "cat123", Guid.NewGuid().ToString(), "Owner","123123");

            _mockUserManager.VerifyAll();
        }
    }
}
