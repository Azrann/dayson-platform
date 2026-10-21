using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DaysOn.Auth.Domain.Data;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Auth.Domain.Events;
using DaysOn.Auth.Web.Controllers;
using DaysOn.Common;
using DaysOn.Common.AzureBus;
using DaysOn.Common.DataAccess;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.SqlDataAccess.Repositories;
using JustEat.StatsD;
using log4net;

namespace DaysOn.Auth.Web
{
    public static class WindsorConfiguration
    {
        public static IWindsorContainer Configure(IWindsorContainer container)
        {
            container.Register(
                Types.FromAssemblyNamed("DaysOn.Auth.Web")
                    .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(ApiControllerBase)))
                .LifestyleTransient());

            container.Register(
                Types.FromAssemblyNamed("DaysOn.Auth.Web")
                    .Where(t => Attribute.IsDefined(t, typeof (CastleComponentAttribute)))
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient());

            container.Register(
                Types.FromAssemblyNamed("DaysOn.Auth.Domain")
                    .Where(t => Attribute.IsDefined(t, typeof(CastleComponentAttribute)))
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient());

            var statsDConfig = new StatsDConfiguration
            {
                Host = "statsd.hopto.org",
                Port = 8125,
                Prefix = "5e4360be-cafa-4a93-bfdd-e1a0989074a3"
            };

            container.Register(Component.For<StatsDConfiguration>().Instance(statsDConfig));
            container.Register(Component.For<IStatsDPublisher>().ImplementedBy<StatsDPublisher>());

            container.Register(Component.For<IRepository<ActivationCode>>().Instance(new Repository<ActivationCode>(new AuthContext())).LifestyleTransient());
            container.Register(Component.For<IRepository<ResetPasswordCode>>().Instance(new Repository<ResetPasswordCode>(new AuthContext())).LifestyleTransient());

            container.Register(
                Component.For<ILog>()
                    .Instance(LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType))
                    .LifestyleSingleton());

            container.Register(Component.For<IEventHandler<UserRegisteredEvent>>().ImplementedBy(typeof(UserRegisteredEventHandler)).LifestyleTransient());
            container.Register(Component.For<IAsyncEventHandler<CheckAddManagerRoleEvent>>().ImplementedBy(typeof(CheckAddManagerRoleEventHandler)).LifestyleTransient());
            container.Register(Component.For<IAsyncEventHandler<CheckRemoveManagerRoleEvent>>().ImplementedBy(typeof(CheckRemoveManagerRoleEventHandler)).LifestyleTransient());
            container.Register(Component.For<IMessageHandler<AuthPingEvent>>().ImplementedBy<PingAsyncEventHandler<AuthPingEvent>>().LifestyleTransient());

            container.Register(Component.For<INoSqlDb<NonActivatedUsers>>().ImplementedBy<DapperNoSql<NonActivatedUsers>>().LifestyleTransient());

            container.Register(Component.For<IServiceBus>().Instance(new AzureServiceBus(ConfigurationManager.AppSettings["Enviroment"], ConfigurationManager.AppSettings["Feature"], container.Resolve<IStatsDPublisher>())).LifestyleSingleton());
            container.Register(Component.For<IPubnubMessageBus>().Instance(new PubnubMessageBus(
                container.Resolve<ILog>(),
                ConfigurationManager.AppSettings["Enviroment"])).LifestyleSingleton());


            return container;
        }
    }
}