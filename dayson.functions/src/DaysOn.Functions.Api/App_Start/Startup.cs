using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.Http;
using Castle.Windsor;
using DaysOn.Common.AzureBus;
using log4net;
using log4net.Config;
using Owin;
using WebApiContrib.IoC.CastleWindsor;
using DaysOn.Common.ServiceBase.ServiceHealth;
using DaysOn.Common.ServiceBase.ServiceDependency;
using DaysOn.Common.Domain.Ioc;
using DaysOn.Common.Events.Events;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.ServiceBase.AppStart.Filters;
using DaysOn.Common.ServiceBase.AppStart.Windsor;
using DaysOn.Common.Utilities.Extension;
using JustEat.StatsD;

namespace DaysOn.Functions.Api.App_Start
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public static ServiceHealthStatusEventStore ServiceHealthStatus { get; set; }
        
        public virtual void Configuration(IAppBuilder app)
        {
            XmlConfigurator.Configure();

            GlobalContext.Properties["feature"] = "Functions";
            GlobalContext.Properties["enviroment"] = ConfigurationManager.AppSettings["Enviroment"];
            _logger.LogInf("DaysOn.Functions startup...");

            LogConfigurationSettings();

            WebApiConfigurationBuilder.SetDefaults();
            
            ServiceHealthStatus = new ServiceHealthStatusEventStore();

            WindsorConfigurator.Configure(ContainerManager.Container);
       
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorResolver(ContainerManager.Container);

            _logger.LogInf("Service dependencies healthcheck...");
            var dependenciesHealthCheckResult = ServiceDependencyCheck();

            if (dependenciesHealthCheckResult.HealthStatus == HealthStatus.Faulty)
            {
                GlobalConfiguration.Configuration.Routes.MapHttpRoute("Root", "version", new { controller = "Version", action = "GetAsync" });
                _logger.LogErr("DaysOn.Functions could not load all dependencies. Service status is faulty.");
                app.UseWebApi(GlobalConfiguration.Configuration);
                return;
            }

            _logger.LogInf("All service dependencies are OK!");

            var serviceBus = ContainerManager.Container.Resolve<IServiceBus>();
            serviceBus.RegisterEvent(typeof(CalculateBusinessDaysEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<CalculateBusinessDaysEvent>>());
            serviceBus.RegisterEvent(typeof(AddHolidayRequestEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandlerAsync<AddHolidayRequestEvent>>());
            serviceBus.RegisterEvent(typeof(SendEmailEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandlerAsync<SendEmailEvent>>());
            serviceBus.RegisterEvent(typeof(FunctionsPingEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<FunctionsPingEvent>>());

            _logger.LogInf("Setting up Windsor Castle container...");

            app.ConfigureOAuth();

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(GlobalConfiguration.Configuration);
            PublishStatsFilterAttribute.GetStatsDPublisher = ContainerManager.Container.Resolve<IStatsDPublisher>;
            _logger.LogInf("All done! DaysOn.Functions started successfully.");
        }

        private ServiceDependencyResolverResultSummary ServiceDependencyCheck()
        {
            bool isServiceHealthy = true;

            var dependenciesHealthCheckResult = new ServiceDependencyContainer()
                .Register(new List<ServiceDependency>()
                              {
                                  new ServiceDependency("IOCComponents", () => WindsorConfigurator.Configure(new WindsorContainer()).Verify() , 20000)
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
                    _logger.LogErr(errorMessage);
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