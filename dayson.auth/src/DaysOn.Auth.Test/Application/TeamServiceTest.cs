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

namespace DaysOn.Auth.Test.Application
{
    [TestFixture]
    public class TeamServiceTest
    {
        [Test]
        public async void WhenISetTheTeam_ThenUserIsAssignedToTheTeam()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult(user));
            mockUserRepo.Setup(x => x.AddClaimAsync(userId.ToString(),It.Is<Claim>(obj => obj.Type == "Team"), It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Success)).Verifiable();

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var teamService = new TeamService()
            {
                UserManagerWrapper = mockUserRepo.Object,
                UserManagerFactory = mockUserManagerFactory.Object
            };

            await teamService.AddEmployee(userId, teamId);
            
            mockUserRepo.VerifyAll();

        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "UserId 960609e8-af47-495c-81a5-7a11ba698f66 could not be found")]
        public async void WhenISetTheTeamAndUserDoesntExists_ThenExceptionIsThrown()
        {
            var userId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");
            var teamId = Guid.NewGuid();

            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult<User>(null));

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var teamService = new TeamService()
            {
                UserManagerWrapper = mockUserRepo.Object,
                UserManagerFactory = mockUserManagerFactory.Object
            };

            await teamService.AddEmployee(userId, teamId);
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "User with id 960609e8-af47-495c-81a5-7a11ba698f66 could not be added to the team 123409e8-af47-295c-11a5-ab1233698f66")]
        public async void WhenISetTheTeamAndDatabaseFail_ThenExceptionIsThrown()
        {
            var userId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");
            var teamId = new Guid("123409e8-af47-295c-11a5-ab1233698f66");

            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult(user));
            mockUserRepo.Setup(x => x.AddClaimAsync(userId.ToString(), It.Is<Claim>(obj => obj.Type == "Team"), It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Failed("Error")));
            
            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var teamService = new TeamService()
            {
                UserManagerWrapper = mockUserRepo.Object,
                UserManagerFactory = mockUserManagerFactory.Object
            };

            await teamService.AddEmployee(userId, teamId);
        }

        [Test]
        public async void WhenIRemoveTheUserFromTheTeam_ThenUserHasNoTeam()
        {
            var userId = Guid.NewGuid();
            var claims = new List<Claim>
            {
                new Claim("Team", Guid.NewGuid().ToString())

            };
            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult(user));
            mockUserRepo.Setup(x => x.RemoveClaimAsync(userId.ToString(), It.Is<Claim>(o => o.Type == "Team"), It.IsAny<UserManager<User>>())).ReturnsAsync(IdentityResult.Success).Verifiable();
            mockUserRepo.Setup(x => x.GetClaimsAsync(userId.ToString(), It.IsAny<UserManager<User>>())).ReturnsAsync(claims);

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var teamService = new TeamService()
            {
                UserManagerWrapper = mockUserRepo.Object,
                UserManagerFactory = mockUserManagerFactory.Object
            };

            await teamService.RemoveTeamFromEmployee(userId);

            mockUserRepo.VerifyAll();
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

            var teamService = new TeamService()
            {
                UserManagerWrapper = mockUserRepo.Object,
                UserManagerFactory = mockUserManagerFactory.Object
            };

            await teamService.RemoveTeamFromEmployee(userId);
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "User with id 960609e8-af47-495c-81a5-7a11ba698f66 could not be removed from the team")]
        public async void WhenIRemoveTheUserFromTheTeamAndDatabaseFail_ThenExceptionIsThrown()
        {
            var userId = new Guid("960609e8-af47-495c-81a5-7a11ba698f66");
            var claims = new List<Claim>
            {
                new Claim("Team", Guid.NewGuid().ToString())

            };
            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            var mockUserRepo = new Mock<IUserManagerWrapper>();

            mockUserRepo.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<UserManager<User>>())).Returns(() => Task.FromResult(user)); 
            mockUserRepo.Setup(x => x.RemoveClaimAsync(userId.ToString(), It.Is<Claim>(obj => obj.Type == "Team"), It.IsAny<UserManager<User>>())).Returns(Task.FromResult(IdentityResult.Failed("Error")));
            mockUserRepo.Setup(x => x.GetClaimsAsync(userId.ToString(), It.IsAny<UserManager<User>>())).ReturnsAsync(claims);

            var mockUserManagerFactory = new Mock<IUserManagerFactory>();
            mockUserManagerFactory.Setup(x => x.CreateUserManager()).Returns(new UserManager<User>(new UserStore<User>(new AuthContext())));

            var teamService = new TeamService()
            {
                UserManagerWrapper = mockUserRepo.Object,
                UserManagerFactory = mockUserManagerFactory.Object
            };

            await teamService.RemoveTeamFromEmployee(userId);
        }
    }
}
