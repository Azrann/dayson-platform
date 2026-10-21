using System;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Reflection;
using System.Web.Http;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DaysOn.Common;
using DaysOn.Common.AzureBus;
using DaysOn.Common.DataAccess;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using JustEat.StatsD;
using log4net;

namespace DaysOn.Functions.Api.App_Start
{
    public static class WindsorConfigurator
    {
        public static IWindsorContainer Configure(IWindsorContainer container)
        {
            container.Register(
                Classes.FromAssemblyNamed("DaysOn.Functions.Api").
                BasedOn<ApiController>()
                .LifestyleTransient());

            container.Register(
                Classes.FromAssemblyNamed("DaysOn.Functions.BoundedContext")
                .Where(t => Attribute.IsDefined(t, typeof(CastleComponentAttribute))).WithService.DefaultInterfaces().LifestyleTransient());

            container.Register(
                Classes.FromAssemblyNamed("DaysOn.HolidaysBc.BoundedContext")
                .Where(t => Attribute.IsDefined(t, typeof(CastleComponentAttribute))).WithService.DefaultInterfaces().LifestyleTransient());

            var statsDConfig = new StatsDConfiguration
            {
                Host = "statsd.hopto.org",
                Port = 8125,
                Prefix = "5e4360be-cafa-4a93-bfdd-e1a0989074a3",
            };

            container.Register(Component.For<StatsDConfiguration>().Instance(statsDConfig));
            container.Register(Component.For<IStatsDPublisher>().ImplementedBy<StatsDPublisher>());

            container.Register(Component.For<DbContext>().ImplementedBy<HolidaysContext>().LifestyleTransient());
            container.Register(Component.For<IRepository<Company>>().ImplementedBy<Repository<Company>>().LifestyleTransient());
            container.Register(Component.For<IRepository<Team>>().ImplementedBy<Repository<Team>>().LifestyleTransient());
            container.Register(Component.For<IRepository<Employee>>().ImplementedBy<Repository<Employee>>().LifestyleTransient());
            container.Register(Component.For<IRepository<HolidaysBc.BoundedContext.Entities.Rule>>().ImplementedBy<Repository<HolidaysBc.BoundedContext.Entities.Rule>>().LifestyleTransient());
            container.Register(Component.For<IRepository<GlobalRules>>().ImplementedBy<Repository<GlobalRules>>().LifestyleTransient());

            container.Register(Component.For<IRepository<HolidayRequest>>().ImplementedBy<Repository<HolidayRequest>>().LifestyleTransient());
            container.Register(Component.For<IRepository<YearlyHolidayCalculation>>().ImplementedBy<Repository<YearlyHolidayCalculation>>().LifestyleTransient());

            container.Register(Component.For<INoSqlDb<OpenClosedDates>>().ImplementedBy<DapperNoSql<OpenClosedDates>>().LifestyleTransient());
            container.Register(Component.For<INoSqlDb<TeamMembers>>().ImplementedBy<DapperNoSql<TeamMembers>>().LifestyleTransient());
            container.Register(Component.For<INoSqlDb<EmployeeTeam>>().ImplementedBy<DapperNoSql<EmployeeTeam>>().LifestyleTransient());
            container.Register(Component.For<INoSqlDb<TeamOwners>>().ImplementedBy<DapperNoSql<TeamOwners>>().LifestyleTransient());
            container.Register(Component.For<INoSqlDb<EmployeesByApprover>>().ImplementedBy<DapperNoSql<EmployeesByApprover>>().LifestyleTransient());
            container.Register(Component.For<IMessageHandler<FunctionsPingEvent>>().ImplementedBy<PingAsyncEventHandler<FunctionsPingEvent>>().LifestyleTransient());

            container.Register(
                 Classes.FromAssemblyNamed("DaysOn.Functions.Api")
                .Where(t => Attribute.IsDefined(t, typeof(CastleComponentAttribute))).WithService.DefaultInterfaces().LifestyleTransient());

            container.Register(Component.For<ILog>().Instance(LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)).LifestyleSingleton());
            
            container.Register(Component.For<IServiceBus>().Instance(new AzureServiceBus(ConfigurationManager.AppSettings["Enviroment"], ConfigurationManager.AppSettings["Feature"], container.Resolve<IStatsDPublisher>())).LifestyleSingleton());
            container.Register(Component.For<IPubnubMessageBus>().Instance(new PubnubMessageBus(
                container.Resolve<ILog>(),
                ConfigurationManager.AppSettings["Enviroment"])).LifestyleSingleton());

            return container;
        }
    }
}