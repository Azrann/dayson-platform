using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Castle.Windsor;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Auth.Web;
using NUnit.Framework;

namespace DaysOn.Auth.Test.Integration.Services
{
    [TestFixture]
    public class ManagerOfTeamServiceTest
    {
        private IWindsorContainer _container;

        [SetUp]
        public void Initialize()
        {
            _container = new WindsorContainer();
            WindsorConfiguration.Configure(_container);
        }

        [Test]
        public async void WhenIAddAManagerToATeamICanRetrieveClaims()
        {
            IManagerOfTeamService managerOfTeamService = _container.Resolve<IManagerOfTeamService>();
            var userManagerWrapper = _container.Resolve<IUserManagerWrapper>();
            var userManagerFactory = _container.Resolve<IUserManagerFactory>();

            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            User user = new User(userId.ToString(), Guid.NewGuid() + "@email.com)", Guid.NewGuid().ToString());

            using (var userManager = userManagerFactory.CreateUserManager())
            {
                await userManagerWrapper.CreateAsync(user, "REPLACE_WITH_YOUR_VALUE", userManager);
            }

            await managerOfTeamService.AddManager(userId, teamId);

            List<Claim> result;
            using (var userManager = userManagerFactory.CreateUserManager())
            {
                result = (await userManagerWrapper.GetClaimsAsync(userId.ToString(), userManager)).ToList();
            }

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("ManagerOf", result.First().Type);
            Assert.AreEqual(teamId.ToString(), result.First().Value);
        }

        [Test]
        public async void WhenIRemoveAManager_ThenLostItsClaim()
        {
            IManagerOfTeamService managerOfTeamService = _container.Resolve<IManagerOfTeamService>();
            var userManagerWrapper = _container.Resolve<IUserManagerWrapper>();
            var userManagerFactory = _container.Resolve<IUserManagerFactory>();

            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            User user = new User(userId.ToString(), Guid.NewGuid() + "@email.com)", Guid.NewGuid().ToString());

            using (var userManager = userManagerFactory.CreateUserManager())
            {
                await userManagerWrapper.CreateAsync(user, "REPLACE_WITH_YOUR_VALUE", userManager);
            }

            await managerOfTeamService.AddManager(userId, teamId);
            await managerOfTeamService.RemoveManagerFromTeam(userId, teamId);

            List<Claim> result;
            using (var userManager = userManagerFactory.CreateUserManager())
            {
                result = (await userManagerWrapper.GetClaimsAsync(userId.ToString(), userManager)).ToList();
            }

            Assert.AreEqual(0, result.Count);
        }
    }
}
