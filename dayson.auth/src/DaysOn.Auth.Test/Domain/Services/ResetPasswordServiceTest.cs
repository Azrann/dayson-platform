using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DaysOn.Auth.Domain;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Auth.Domain.Services;
using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Common.SqlDataAccess.Repositories;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;

namespace DaysOn.Auth.Test.Domain.Services
{
    [TestFixture]
    public class ResetPasswordServiceTest
    {
        private readonly Guid _codeId;
        private readonly Guid _userId;
        private readonly User _user;
        private Mock<INonActiveUsersService> _nonActiveUsersService;

        public ResetPasswordServiceTest()
        {
            _codeId = Guid.NewGuid();
            _userId = Guid.NewGuid();
            _user = new User(_userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());
            _nonActiveUsersService= new Mock<INonActiveUsersService>();
        }

        [Test]
        public async void WhenICreateAResetPasswordCode__ThenDataIsSaveInTheRepository()
        {
            var email = $"{Guid.NewGuid()}@test.com";
            var resetPasswordCodeId = Guid.NewGuid();

            var mockResetPasswordRepo = new Mock<IRepository<ResetPasswordCode>>();
            var mockUserManagerWrapper = new Mock<IUserManagerWrapper>();

            mockResetPasswordRepo.Setup(x => x.Add(It.IsAny<ResetPasswordCode>())).Verifiable("Resetpassword code was not added to the repo");
            mockResetPasswordRepo.Setup(x => x.Save()).Verifiable("Resetpassword code was not saved to the repo");

            mockUserManagerWrapper.Setup(x => x.FindByEmailAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(Task.FromResult(new User(Guid.NewGuid().ToString(), "user@example.invalid", Guid.NewGuid().ToString())));

            var resetPasswordService = new ResetPasswordService(_nonActiveUsersService.Object)
            {
                UserManagerFactory = new Mock<IUserManagerFactory>().Object,
                ResetPasswordRepository = mockResetPasswordRepo.Object,
                UserManagerWrapper = mockUserManagerWrapper.Object
            };

            await resetPasswordService.CreateResetPasswordCodeAsync(resetPasswordCodeId, email);

            mockResetPasswordRepo.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Reset password code could not be generated. Email user@example.invalid is not register as user.")]
        public async void WhenICreateAResetPasswordCodeAndUserIsNotRegistered__ThenAExceptionIsThrown()
        {
            const string email = "user@example.invalid";
            var resetPasswordCodeId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");

            var mockResetPasswordRepo = new Mock<IRepository<ResetPasswordCode>>();
            var mockUserManagerWrapper = new Mock<IUserManagerWrapper>();

            mockUserManagerWrapper.Setup(x => x.FindByEmailAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(Task.FromResult<User>(null));

            var resetPasswordService = new ResetPasswordService(_nonActiveUsersService.Object)
            {
                UserManagerFactory = new Mock<IUserManagerFactory>().Object,
                ResetPasswordRepository = mockResetPasswordRepo.Object,
                UserManagerWrapper = mockUserManagerWrapper.Object
            };

            await resetPasswordService.CreateResetPasswordCodeAsync(resetPasswordCodeId, email);
        }

        [Test]
        public async void WhenIResetPasswordWithAValidCommand_ThenNoExceptionsAreThrown()
        {
            const string passwordHash = "REPLACE_WITH_YOUR_VALUE";
            var resetPasswordCodeId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");

            var mockResetPasswordRepo = new Mock<IRepository<ResetPasswordCode>>();
            var mockUserManagerWrapper = new Mock<IUserManagerWrapper>();
            var mockUserStoreWrapper = new Mock<IUserStoreWrapper>();

            var passwordCode = new ResetPasswordCode(_codeId, _userId.ToString());
            mockResetPasswordRepo.Setup(x => x.GetAll(It.IsAny<Expression<Func<ResetPasswordCode, bool>>>()))
                                 .Returns(new List<ResetPasswordCode>() { passwordCode });
            mockUserManagerWrapper.Setup(x => x.FindByIdAsync(_userId.ToString(), It.IsAny<UserManager<User>>())).Returns(Task.FromResult(_user));

            mockUserManagerWrapper.Setup(x => x.HashPassword(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(passwordHash);

            mockUserManagerWrapper.Setup(x => x.UpdateAsync(_user, It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Success));

            mockUserStoreWrapper.Setup(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.CompletedTask);


            var resetPasswordService = new ResetPasswordService(_nonActiveUsersService.Object)
            {
                UserManagerFactory = new Mock<IUserManagerFactory>().Object,
                ResetPasswordRepository = mockResetPasswordRepo.Object,
                UserManagerWrapper = mockUserManagerWrapper.Object,
                UserStoreWrapper = mockUserStoreWrapper.Object
            };

            await resetPasswordService.ResetPasswordAsync(new ResetPasswordCommand()
            {
                ResetPasswordToken = resetPasswordCodeId,
                ConfirmPassword = "REPLACE_WITH_YOUR_VALUE",
                Password = "REPLACE_WITH_YOUR_VALUE",
            });
        }

        [Test]
        public async void WhenIResetPasswordAndTheTokenDoesntExists_ThenNoExceptionsAreThrown()
        {
            var resetPasswordCodeId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");

            var mockResetPasswordRepo = new Mock<IRepository<ResetPasswordCode>>();
            var mockUserManagerWrapper = new Mock<IUserManagerWrapper>();


            mockResetPasswordRepo.Setup(x => x.GetAll(It.IsAny<Expression<Func<ResetPasswordCode, bool>>>()))
                                 .Returns(Enumerable.Empty<ResetPasswordCode>().ToList);

            var resetPasswordService = new ResetPasswordService(_nonActiveUsersService.Object)
            {
                UserManagerFactory = new Mock<IUserManagerFactory>().Object,
                ResetPasswordRepository = mockResetPasswordRepo.Object,
                UserManagerWrapper = mockUserManagerWrapper.Object
            };

            try
            {
                await resetPasswordService.ResetPasswordAsync(new ResetPasswordCommand()
                {
                    ResetPasswordToken = resetPasswordCodeId,
                    ConfirmPassword = "REPLACE_WITH_YOUR_VALUE",
                    Password = "REPLACE_WITH_YOUR_VALUE",
                });
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual(ex.Message, (string.Format(ApiErrors.InvalidResetPasswordToken, resetPasswordCodeId)));
                return;
            }
            Assert.Fail("Expected exception");
        }

        [Test]
        public async void WhenIResetPasswordAndTheTokenHasExpired_ThenNoExceptionsAreThrown()
        {
            var resetPasswordCodeId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");

            var mockResetPasswordRepo = new Mock<IRepository<ResetPasswordCode>>();
            var mockUserManagerWrapper = new Mock<IUserManagerWrapper>();

            var passwordCode = new ResetPasswordCode(_codeId, _userId.ToString(), -1);
            mockResetPasswordRepo.Setup(x => x.GetAll(It.IsAny<Expression<Func<ResetPasswordCode, bool>>>()))
                                 .Returns(Enumerable.Empty<ResetPasswordCode>().ToList);
            mockResetPasswordRepo.Setup(x => x.GetAll(It.IsAny<Expression<Func<ResetPasswordCode, bool>>>()))
                     .Returns(new List<ResetPasswordCode>() { passwordCode });

            var resetPasswordService = new ResetPasswordService(_nonActiveUsersService.Object)
            {
                UserManagerFactory = new Mock<IUserManagerFactory>().Object,
                ResetPasswordRepository = mockResetPasswordRepo.Object,
                UserManagerWrapper = mockUserManagerWrapper.Object
            };

            try
            {
               await resetPasswordService.ResetPasswordAsync(new ResetPasswordCommand()
                {
                    ResetPasswordToken = resetPasswordCodeId,
                    ConfirmPassword = "REPLACE_WITH_YOUR_VALUE",
                    Password = "REPLACE_WITH_YOUR_VALUE",
                });
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual(ex.Message, (string.Format(ApiErrors.ResetPasswordTokenHasExpired, resetPasswordCodeId)));
                return;
            }
            Assert.Fail("Expected exception");
        }

        [Test]
        public async void WhenIResetPasswordAndTheUserAsociatedDoesNotExists_ThenNoExceptionsAreThrown()
        {
            var resetPasswordCodeId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");

            var mockResetPasswordRepo = new Mock<IRepository<ResetPasswordCode>>();
            var mockUserManagerWrapper = new Mock<IUserManagerWrapper>();

            var passwordCode = new ResetPasswordCode(_codeId, _userId.ToString());
            mockResetPasswordRepo.Setup(x => x.GetAll(It.IsAny<Expression<Func<ResetPasswordCode, bool>>>()))
                                 .Returns(Enumerable.Empty<ResetPasswordCode>().ToList);
            mockResetPasswordRepo.Setup(x => x.GetAll(It.IsAny<Expression<Func<ResetPasswordCode, bool>>>()))
                     .Returns(new List<ResetPasswordCode>() { passwordCode });
            mockUserManagerWrapper.Setup(x => x.FindByIdAsync(_userId.ToString(), It.IsAny<UserManager<User>>())).Returns(Task.FromResult<User>(null));

            var resetPasswordService = new ResetPasswordService(_nonActiveUsersService.Object)
            {
                UserManagerFactory = new Mock<IUserManagerFactory>().Object,
                ResetPasswordRepository = mockResetPasswordRepo.Object,
                UserManagerWrapper = mockUserManagerWrapper.Object
            };

            try
            {
                await resetPasswordService.ResetPasswordAsync(new ResetPasswordCommand()
                {
                    ResetPasswordToken = resetPasswordCodeId,
                    ConfirmPassword = "REPLACE_WITH_YOUR_VALUE",
                    Password = "REPLACE_WITH_YOUR_VALUE",
                });
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, (string.Format(ApiErrors.UserCouldNotBeFound, _userId.ToString())));
                return;
            }
            Assert.Fail("Expected exception");
        }
    }
}
