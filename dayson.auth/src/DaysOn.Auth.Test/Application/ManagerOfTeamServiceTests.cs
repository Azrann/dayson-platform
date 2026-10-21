using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Data;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Common.SqlDataAccess.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using NUnit.Framework;

namespace DaysOn.Auth.Domain.Application.Tests
{
    [TestFixture()]
    public class ManagerOfTeamServiceTests
    {
        [Test]
        public async void WhenISetAUserAsManager_ThenUserHasAClaimManagerOf()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult(user));
            mockUserRepo.Setup(x => x.AddClaimAsync(userId.ToString(), It.Is<Claim>(obj => obj.Type == "ManagerOf"), It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Success)).Verifiable();

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var managerOfTeamService = new ManagerOfTeamService(mockUserRepo.Object, mockUserManagerFactory.Object);

            await managerOfTeamService.AddManager(userId, teamId);

            mockUserRepo.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UserId 960609e8-af47-495c-81a5-7a11ba698f66 could not be found")]
        public async void WhenIAddAManagerAndUserDoesntExists_ThenExceptionIsThrown()
        {
            var userId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");
            var teamId = Guid.NewGuid();

            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult<User>(null));

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var managerOfTeamService = new ManagerOfTeamService(mockUserRepo.Object, mockUserManagerFactory.Object);

            await managerOfTeamService.AddManager(userId, teamId);
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "User with id 960609e8-af47-495c-81a5-7a11ba698f66 could not be added as a Manager for the team 123409e8-af47-295c-11a5-ab1233698f66")]
        public async void WhenIAddAManagerAndDatabaseFail_ThenExceptionIsThrown()
        {
            var userId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");
            var teamId = new Guid("123409e8-af47-295c-11a5-ab1233698f66");

            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult(user));
            mockUserRepo.Setup(x => x.AddClaimAsync(userId.ToString(), It.Is<Claim>(obj => obj.Type == "ManagerOf"), It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Failed("Error")));

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var managerOfTeamService = new ManagerOfTeamService(mockUserRepo.Object, mockUserManagerFactory.Object);

            await managerOfTeamService.AddManager(userId, teamId);
        }

        [Test]
        public async void WhenIRemoveAManagerFromTheTeam_ThenUserHasNoManagerClaim()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var claims = new List<Claim>
            {
                new Claim("ManagerOf", teamId.ToString())

            };
            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult(user));
            mockUserRepo.Setup(x => x.RemoveClaimAsync(userId.ToString(), It.Is<Claim>(o => o.Type == "ManagerOf"), It.IsAny<UserManager<User>>())).ReturnsAsync(IdentityResult.Success).Verifiable();
            mockUserRepo.Setup(x => x.GetClaimsAsync(userId.ToString(), It.IsAny<UserManager<User>>())).ReturnsAsync(claims);

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var managerOfTeamService = new ManagerOfTeamService(mockUserRepo.Object, mockUserManagerFactory.Object);

            await managerOfTeamService.RemoveManagerFromTeam(userId, teamId);

            mockUserRepo.VerifyAll();
        }

        [Test]
        public async void WhenIRemoveAManagerFromTheTeamAndTheUserIsNoManager_ThenNothingHappen()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var claims = new List<Claim>
            {
                new Claim("ManagerOf", Guid.NewGuid().ToString())

            };
            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult(user));
            mockUserRepo.Setup(x => x.GetClaimsAsync(userId.ToString(), It.IsAny<UserManager<User>>())).ReturnsAsync(claims);

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var managerOfTeamService = new ManagerOfTeamService(mockUserRepo.Object, mockUserManagerFactory.Object);

            await managerOfTeamService.RemoveManagerFromTeam(userId, teamId);

            mockUserRepo.VerifyAll();
            mockUserRepo.Verify(
                x =>
                    x.RemoveClaimAsync(userId.ToString(), It.Is<Claim>(o => o.Type == "ManagerOf"),
                        It.IsAny<UserManager<User>>()), Times.Never);

        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UserId 960609e8-af47-495c-81a5-7a11ba698f66 could not be found")]
        public async void WhenIRemoveTheUserFromTheTeamAndUserDoesntExists_ThenExceptionIsThrown()
        {
            var userId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");

            var mockActivationCodeRepo = new Mock<IRepository<ActivationCode>>();
            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult<User>(null));

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var managerOfTeamService = new ManagerOfTeamService(mockUserRepo.Object, mockUserManagerFactory.Object);

            await managerOfTeamService.RemoveManagerFromTeam(userId, Guid.NewGuid());
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "User with id 960609e8-af47-495c-81a5-7a11ba698f66 could not be removed as Manager from the team 123409e8-af47-295c-11a5-ab1233698f66")]
        public async void WhenIRemoveTheUserFromTheTeamAndDatabaseFail_ThenExceptionIsThrown()
        {
            var userId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");
            var teamId = new Guid("123409e8-af47-295c-11a5-ab1233698f66");

            var claims = new List<Claim>
            {
                new Claim("ManagerOf", teamId.ToString())

            };
            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult(user));
            mockUserRepo.Setup(x => x.RemoveClaimAsync(userId.ToString(), It.Is<Claim>(obj => obj.Type == "ManagerOf"), It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Failed("Error")));
            mockUserRepo.Setup(x => x.GetClaimsAsync(userId.ToString(), It.IsAny<UserManager<User>>())).ReturnsAsync(claims);

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var managerOfTeamService = new ManagerOfTeamService(mockUserRepo.Object, mockUserManagerFactory.Object);

            await managerOfTeamService.RemoveManagerFromTeam(userId, teamId);
        }
    }
}