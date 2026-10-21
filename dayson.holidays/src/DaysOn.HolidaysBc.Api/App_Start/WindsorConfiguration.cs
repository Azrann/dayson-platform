using System;
using System.Configuration;
using System.Data.Entity;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DaysOn.Common.AzureBus;
using DaysOn.Common.DataAccess;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.HolidaysBc.Api.Controllers;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Events;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.HolidaysBc.BoundedContext.Services;
using JustEat.StatsD;
using log4net;

namespace DaysOn.HolidaysBc.Api.App_Start
{
    public static class WindsorConfigurator
    {
        public static IWindsorContainer Configure(IWindsorContainer container)
        {
            container.Register(
                Types.FromAssemblyNamed("DaysOn.HolidaysBc.Api").Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(ApiControllerBase)))
                .LifestyleTransient());

            container.Register(
                Types.FromAssemblyNamed("DaysOn.HolidaysBc.BoundedContext")
                .Where(t => Attribute.IsDefined(t, typeof(CastleComponentAttribute))).WithService.DefaultInterfaces().LifestyleTransient());

            container.Register(
                Types.FromAssemblyNamed("DaysOn.HolidaysBc.Api")
                .Where(t => Attribute.IsDefined(t, typeof(CastleComponentAttribute))).WithService.DefaultInterfaces().LifestyleTransient());

            container.Register(Component.For<ILog>().Instance(LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)).LifestyleSingleton());

            var statsDConfig = new StatsDConfiguration
            {
                Host = "statsd.hopto.org",
                Port = 8125,
                Prefix = "5e4360be-cafa-4a93-bfdd-e1a0989074a3"
            };

            container.Register(Component.For<StatsDConfiguration>().Instance(statsDConfig));
            container.Register(Component.For<IStatsDPublisher>().ImplementedBy<StatsDPublisher>());

            container.Register(Component.For<DbContext>().ImplementedBy<HolidaysContext>().LifestyleTransient());
            
            container.Register(Component.For<IRepository<Company>>().ImplementedBy<Repository<Company>>().LifestyleTransient());
            container.Register(Component.For<IRepository<Team>>().ImplementedBy<Repository<Team>>().LifestyleTransient());
            container.Register(Component.For<IRepository<Employee>>().ImplementedBy<Repository<Employee>>().LifestyleTransient());
            container.Register(Component.For<IRepository<Rule>>().ImplementedBy<Repository<Rule>>().LifestyleTransient());
            container.Register(Component.For<IRepository<GlobalRules>>().ImplementedBy<Repository<GlobalRules>>().LifestyleTransient());

            container.Register(Component.For<IRepository<HolidayRequest>>().ImplementedBy<Repository<HolidayRequest>>().LifestyleTransient());
            container.Register(Component.For<IRepository<YearlyHolidayCalculation>>().ImplementedBy<Repository<YearlyHolidayCalculation>>().LifestyleTransient());
            container.Register(Component.For<IServiceBus>().Instance(new AzureServiceBus(ConfigurationManager.AppSettings["Enviroment"], ConfigurationManager.AppSettings["Feature"], container.Resolve<IStatsDPublisher>())).LifestyleSingleton());

            container.Register(Component.For<INoSqlDb<OpenClosedDates>>().ImplementedBy<DapperNoSql<OpenClosedDates>>().LifestyleTransient());
            container.Register(Component.For<INoSqlDb<TeamMembers>>().ImplementedBy<DapperNoSql<TeamMembers>>().LifestyleTransient());
            container.Register(Component.For<INoSqlDb<EmployeeTeam>>().ImplementedBy<DapperNoSql<EmployeeTeam>>().LifestyleTransient());
            container.Register(Component.For<INoSqlDb<EmployeesByApprover>>().ImplementedBy<DapperNoSql<EmployeesByApprover>>().LifestyleTransient());
            container.Register(Component.For<ITeamMembersService>().ImplementedBy<TeamMembersService>().LifestyleTransient());
            container.Register(Component.For<IEmployeeTeamService>().ImplementedBy<EmployeeTeamService>().LifestyleTransient());

            
            container.Register(Component.For<IEventHandler<UpdateCalculationsEvent>>().ImplementedBy<UpdateCalculationsEventHandler>().LifestyleTransient());
            container.Register(Component.For<IEventHandler<UpdateDaysCarryOverEvent>>().ImplementedBy<UpdateDaysCarryOverEventHandler>().LifestyleTransient());
            container.Register(Component.For<IMessageHandler<HolidaysPingEvent>>().ImplementedBy<PingAsyncEventHandler<HolidaysPingEvent>>().LifestyleTransient());


            return container;
        }
    }
}