using System;
using Castle.Windsor;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Auth.Domain.Events;
using DaysOn.Auth.Web;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.Common.Domain.Ioc;
using NUnit.Framework;

namespace DaysOn.Auth.Test.Integration.Events
{
    [TestFixture]
    public class CheckRemoveManagerRoleEventHandlerTests
    {
        private IWindsorContainer _container;

        [SetUp]
        public void Initialize()
        {
            ContainerManager.ReleaseAll();
            _container = new WindsorContainer();
            WindsorConfiguration.Configure(ContainerManager.Container);
            WindsorConfiguration.Configure(_container);
        }

        [Test]
        public async void WhenCallTheHandlerAndIDontManageAnyTeamILoseMyRole()
        {
            var userManagerWrapper = _container.Resolve<IUserManagerWrapper>();
            var userManagerFactory = _container.Resolve<IUserManagerFactory>();

            var userId = Guid.NewGuid();
            User user = new User(userId.ToString(), Guid.NewGuid() + "@email.com)", Guid.NewGuid().ToString());

            using (var userManager = userManagerFactory.CreateUserManager())
            {
                await userManagerWrapper.CreateAsync(user, "REPLACE_WITH_YOUR_VALUE", userManager);
                await userManagerWrapper.AddToRoleAsync(userId.ToString(), "Manager", userManager);
            }

            var handler = _container.Resolve<IAsyncEventHandler<CheckRemoveManagerRoleEvent>>();

            await handler.Handle(new CheckRemoveManagerRoleEvent(userId));

            using (var userManager = userManagerFactory.CreateUserManager())
            {
                var roles = await userManager.GetRolesAsync(userId.ToString());
                Assert.IsEmpty(roles);
            }
        }

        [Test]
        public async void WhenCallTheHandlerAndIManage1TeamNothingHappen()
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
                await managerOfTeamService.AddManager(userId, teamId);            
            }

            var handler = _container.Resolve<IAsyncEventHandler<CheckRemoveManagerRoleEvent>>();

            await handler.Handle(new CheckRemoveManagerRoleEvent(userId));

            using (var userManager = userManagerFactory.CreateUserManager())
            {
                var roles = await userManager.GetRolesAsync(userId.ToString());
                Assert.IsNotEmpty(roles);
                Assert.AreEqual("Manager",roles[0]);
                Assert.AreEqual(1, roles.Count);
            }
        }
    }
}