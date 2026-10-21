using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Data;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Auth.Domain.Services;
using DaysOn.Common.SqlDataAccess.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using NUnit.Framework;

namespace DaysOn.Auth.Test.Domain.Services
{
    [TestFixture]
    public class ActivationServiceTest
    {
        [Test]
        public async void WhenIActivateAUser_ThenUserIsActive()
        {
            var userId = Guid.NewGuid();
            var activationCodeId = Guid.NewGuid();

            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());
            var activationCode = new ActivationCode(activationCodeId, userId);

            var mockActivationCodeRepo = new Mock<IRepository<ActivationCode>>();
            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockActivationCodeRepo.Setup(x => x.GetAll(It.IsAny<Expression<Func<ActivationCode, bool>>>())).Returns(new List<ActivationCode>() { activationCode });

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult(user));
            mockUserRepo.Setup(x => x.UpdateAsync(user, It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Success));

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var activationService = new ActivationService()
            {
                ActivationCodeRespository = mockActivationCodeRepo.Object,
                UserManagerWrapper = mockUserRepo.Object,
                UserManagerFactory = mockUserManagerFactory.Object
            };

            await activationService.ActivateAsync(activationCodeId);

            Assert.IsTrue(user.IsActive);
        }

        [Test]
        public async void WhenIActivateAUser_ThenUserEmailIsConfirmed()
        {
            var userId = Guid.NewGuid();
            var activationCodeId = Guid.NewGuid();

            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());
            var activationCode = new ActivationCode(activationCodeId, userId);

            var mockActivationCodeRepo = new Mock<IRepository<ActivationCode>>();
            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockActivationCodeRepo.Setup(x => x.GetAll(It.IsAny<Expression<Func<ActivationCode, bool>>>())).Returns(new List<ActivationCode>() { activationCode });

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult<User>(user));
            mockUserRepo.Setup(x => x.UpdateAsync(user, It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Success));

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var activationService = new ActivationService()
            {
                ActivationCodeRespository = mockActivationCodeRepo.Object,
                UserManagerWrapper = mockUserRepo.Object,
                UserManagerFactory = mockUserManagerFactory.Object
            };

            await activationService.ActivateAsync(activationCodeId);

            Assert.IsTrue(user.EmailConfirmed);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UserId 960609e8-af47-495c-81a5-7a11ba698f66 could not be found")]
        public async void WhenIActivateAUserAndUserDoesntExists_ThenExceptionIsThrown()
        {
            var userId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");
            var activationCodeId = Guid.NewGuid();

            var activationCode = new ActivationCode(activationCodeId, userId);

            var mockActivationCodeRepo = new Mock<IRepository<ActivationCode>>();
            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockActivationCodeRepo.Setup(x => x.GetAll(It.IsAny<Expression<Func<ActivationCode, bool>>>())).Returns(new List<ActivationCode>() { activationCode });

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>() ,It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult<User>(null));

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var activationService = new ActivationService()
            {
                ActivationCodeRespository = mockActivationCodeRepo.Object,
                UserManagerWrapper = mockUserRepo.Object,
                UserManagerFactory = mockUserManagerFactory.Object
            };

            await activationService.ActivateAsync(activationCodeId);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Activation code 960609e8-af47-495c-81a5-7a11ba698f66 could not be found")]
        public async void WhenIActivateAUserAndActivationCodeDoesntExists_ThenExceptionIsThrown()
        {
            var activationCodeId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");
            var userId = Guid.NewGuid();

            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            var mockActivationCodeRepo = new Mock<IRepository<ActivationCode>>();
            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockActivationCodeRepo.Setup(x => x.GetAll(It.IsAny<Expression<Func<ActivationCode, bool>>>())).Returns((IList<ActivationCode>)Enumerable.Empty<ActivationCode>());

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult(user));

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var activationService = new ActivationService()
            {
                ActivationCodeRespository = mockActivationCodeRepo.Object,
                UserManagerWrapper = mockUserRepo.Object,
                UserManagerFactory = mockUserManagerFactory.Object
            };

            await activationService.ActivateAsync(activationCodeId);
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "User 960609e8-af47-495c-81a5-7a11ba698f66 could not be activated")]
        public async void WhenIActivateAUserAndDatabaseFail_ThenExceptionIsThrown()
        {
            var userId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");
            var activationCodeId = Guid.NewGuid();

            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());
            var activationCode = new ActivationCode(activationCodeId, userId);

            var mockActivationCodeRepo = new Mock<IRepository<ActivationCode>>();
            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockActivationCodeRepo.Setup(x => x.GetAll(It.IsAny<Expression<Func<ActivationCode, bool>>>())).Returns(new List<ActivationCode>() { activationCode });
            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult(user));
            mockUserRepo.Setup(x => x.UpdateAsync(user, It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Failed("Error")));

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var activationService = new ActivationService()
            {
                ActivationCodeRespository = mockActivationCodeRepo.Object,
                UserManagerWrapper = mockUserRepo.Object,
                UserManagerFactory = mockUserManagerFactory.Object
            };

            await activationService.ActivateAsync(activationCodeId);
        }

        [Test]
        public async void WhenICreateActivationCode_ThenDataIsSaveInTheRepository()
        {
            var userId = Guid.NewGuid();
            var activationCodeId = Guid.NewGuid();

            var activationCode = new ActivationCode(activationCodeId, userId);

            var mockActivationCodeRepo = new Mock<IRepository<ActivationCode>>();

            mockActivationCodeRepo.Setup(x => x.Add(activationCode)).Verifiable("Activation code was not added to the repo");
            mockActivationCodeRepo.Setup(x => x.Save()).Verifiable("Activation code was not saved to the repo");

            var activatinonService = new ActivationService()
            {
                ActivationCodeRespository = mockActivationCodeRepo.Object,
                UserManagerWrapper = null
            };

            await activatinonService.CreateActivationCodeAsync(activationCode);

            mockActivationCodeRepo.VerifyAll();
        }
    }
}
