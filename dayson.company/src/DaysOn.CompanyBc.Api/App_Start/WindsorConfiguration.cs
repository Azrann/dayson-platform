using System;
using System.Configuration;
using System.Data.Entity;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.CompanyBc.Api.Controllers;
using DaysOn.CompanyBc.BoundedContext.Data;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.BoundedContext.Events;
using DaysOn.Common.Domain.Ioc;
using DaysOn.Common.SqlDataAccess.Repositories;
using JustEat.StatsD;
using log4net;

namespace DaysOn.CompanyBc.Api.App_Start
{
    public static class WindsorConfigurator
    {
        public static IWindsorContainer Configure(IWindsorContainer container)
        {
            container.Register(Classes.FromAssemblyNamed("DaysOn.CompanyBc.Api").BasedOn<ApiControllerBase>()
                .LifestyleTransient());

            container.Register(Classes.FromAssemblyNamed("DaysOn.CompanyBc.BoundedContext")
                .Where(t => Attribute.IsDefined(t, typeof(CastleComponentAttribute))).WithService.DefaultInterfaces()
                .LifestyleTransient());

            container.Register(Classes.FromAssemblyNamed("DaysOn.CompanyBc.Api")
                .Where(t => Attribute.IsDefined(t, typeof(CastleComponentAttribute))).WithService.DefaultInterfaces()
                .LifestyleTransient());

            container.Register(Component.For<ILog>()
                .Instance(LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)).LifestyleSingleton());

            var statsDConfig = new StatsDConfiguration
            {
                Host = "statsd.hopto.org",
                Port = 8125,
                Prefix = "5e4360be-cafa-4a93-bfdd-e1a0989074a3",
            };

            container.Register(Component.For<StatsDConfiguration>().Instance(statsDConfig));
            container.Register(Component.For<IStatsDPublisher>().ImplementedBy<StatsDPublisher>());
            container.Register(Component.For<DbContext>().ImplementedBy<CompanyContext>().LifestyleTransient());

            container.Register(Component.For<IRepository<Company>>().ImplementedBy<Repository<Company>>()
                .LifestyleTransient());
            container.Register(
                Component.For<IRepository<Team>>().ImplementedBy<Repository<Team>>().LifestyleTransient());
            container.Register(Component.For<IRepository<Employee>>().ImplementedBy<Repository<Employee>>()
                .LifestyleTransient());
            container.Register(Component.For<IServiceBus>()
                .Instance(new AzureServiceBus(ConfigurationManager.AppSettings["Enviroment"],
                    ConfigurationManager.AppSettings["Feature"], container.Resolve<IStatsDPublisher>()))
                .LifestyleSingleton());

            container.Register(Component.For<IEventHandler<CompanyCreatedEvent>>()
                .ImplementedBy<CompanyCreatedEventHandler>().LifestyleTransient());
            container.Register(Component.For<IEventHandler<EmployeeCreatedEvent>>()
                .ImplementedBy<EmployeeCreatedEventHandler>().LifestyleTransient());
            container.Register(Component.For<IEventHandler<EmployeeUpdatedEvent>>()
                .ImplementedBy<EmployeeUpdatedEventHandler>().LifestyleTransient());

            container.Register(Component.For<IEventHandler<TeamCreatedEvent>>().ImplementedBy<TeamCreatedEventHandler>()
                .LifestyleTransient());
            container.Register(Component.For<IEventHandler<TeamRemovedEvent>>().ImplementedBy<TeamRemovedEventHandler>()
                .LifestyleTransient());
            container.Register(Component.For<IMessageHandler<CompanyPingEvent>>()
                .ImplementedBy<PingAsyncEventHandler<CompanyPingEvent>>().LifestyleTransient());

            return container;
        }
    }
}