using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Castle.Windsor;
using DaysOn.Common.AzureBus;
using DaysOn.Common.DataAccess;
using DaysOn.Common.Domain.Ioc;
using DaysOn.Common.Events.Events;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.Common.ServiceBase.AppStart.Windsor;
using DaysOn.Common.ServiceBase.ServiceDependency;
using DaysOn.Common.ServiceBase.ServiceHealth;
using log4net;
using log4net.Config;
using Owin;
using WebApiContrib.IoC.CastleWindsor;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;
using DaysOn.Common.ServiceBase.AppStart.Filters;
using DaysOn.Common.SqlDataAccess.Repositories;
using JustEat.StatsD;
using Microsoft.ServiceBus;

namespace DaysOn.HolidaysBc.Api.App_Start
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public static ServiceHealthStatusEventStore ServiceHealthStatus { get; set; }
        
        public virtual void Configuration(IAppBuilder app)
        {
            XmlConfigurator.Configure();
            GlobalContext.Properties["feature"] = "Absence";
            GlobalContext.Properties["enviroment"] = ConfigurationManager.AppSettings["Enviroment"];
            _logger.LogInf("DaysOn.HolidaysBc startup...");

            LogConfigurationSettings();

            WebApiConfigurationBuilder.SetDefaults();
            
            ServiceHealthStatus = new ServiceHealthStatusEventStore();

            _logger.LogInf("Setting up Windsor Castle container...");
            
            WindsorConfigurator.Configure(ContainerManager.Container);

            _logger.LogInf("Service dependencies healthcheck...");
            ServiceDependencyCheck();

            _logger.LogInf("Registering Bus events...");

            ConfigureServiceBus();

            GlobalConfiguration.Configuration.DependencyResolver = new WindsorResolver(ContainerManager.Container);

            app.ConfigureOAuth();

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(GlobalConfiguration.Configuration);

            _logger.LogInf("Creating NoSqlTables...");
            ConfigureNoSQL();
            PublishStatsFilterAttribute.GetStatsDPublisher = ContainerManager.Container.Resolve<IStatsDPublisher>;
            _logger.LogInf("All done! HolidaysBC started successfully.");
        }

        private static void ConfigureServiceBus()
        {
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Https;
            var serviceBus = ContainerManager.Container.Resolve<IServiceBus>();
            serviceBus.RegisterEvent(typeof(CompanyCreatedBusEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<CompanyCreatedBusEvent>>());
            serviceBus.RegisterEvent(typeof(EmployeeCreatedBusEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<EmployeeCreatedBusEvent>>());
            serviceBus.RegisterEvent(typeof(CalculateBusinessDaysEvent));
            serviceBus.RegisterEvent(typeof(EmployeeAddedToTeamEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<EmployeeAddedToTeamEvent>>());
            serviceBus.RegisterEvent(typeof(EmployeeRemovedFromTeamEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<EmployeeRemovedFromTeamEvent>>());
            serviceBus.RegisterEvent(typeof(TeamCreatedBusEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<TeamCreatedBusEvent>>());
            serviceBus.RegisterEvent(typeof(TeamRemovedBusEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<TeamRemovedBusEvent>>());
            serviceBus.RegisterEvent(typeof(HolidaysPingEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<HolidaysPingEvent>>());
        }

        private void ConfigureNoSQL()
        {
            var noSql = ContainerManager.Container.Resolve<INoSqlDb<OpenClosedDates>>();
            noSql.CreateTableIfNotExists(NoSqlTables.OpenClosedDates);
            noSql.CreateTableIfNotExists(NoSqlTables.TeamMembers);
            noSql.CreateTableIfNotExists(NoSqlTables.EmployeeTeam);
            noSql.CreateTableIfNotExists(NoSqlTables.EmployeesByApprover);
        }

        private ServiceDependencyResolverResultSummary ServiceDependencyCheck()
        {
            bool isServiceHealthy = true;

            var dependenciesHealthCheckResult = new ServiceDependencyContainer()
                .Register(new List<ServiceDependency>()
                              {
                                  new ServiceDependency("HolidaysContext", () => new Repository<Company>(new HolidaysContext())),
                                  new ServiceDependency("IOCComponents", () =>   WindsorConfigurator.Configure(new WindsorContainer()).Verify(),20000)
                              })
                .VerifyAll();

            foreach (var result in dependenciesHealthCheckResult)
            {
                if (result.Value.Resolved)
                {
                    _logger.LogInf($"Dependency [{result.Key}] resolved.");
                }
                else
                {
                    isServiceHealthy = false;
                    var errorMessage = $"Couldn't resove dependency [{result.Key}].ErrorMessage=[{result.Value.Message}].";
                    _logger.LogErr(errorMessage.Replace("\"","'"));
                    ServiceHealthStatus.SetFaultyStatus(errorMessage);
                }
            }

            if (isServiceHealthy)
            {
                ServiceHealthStatus.SetOkStatus();
            }

            return dependenciesHealthCheckResult;
        }

        private void LogConfigurationSettings()
        {
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                _logger.LogInf($"Configuration setting key=[{key}], value=[{ConfigurationManager.AppSettings[key]}].");
            }
        }
    }
}