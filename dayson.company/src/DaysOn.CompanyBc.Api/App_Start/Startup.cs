using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.Http;
using Castle.Windsor;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.ServiceBase.AppStart.Filters;
using DaysOn.Common.Utilities.Extension;
using DaysOn.CompanyBc.BoundedContext.Data;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.Common.Domain.Ioc;
using DaysOn.Common.ServiceBase.AppStart.Windsor;
using DaysOn.Common.ServiceBase.ServiceDependency;
using DaysOn.Common.ServiceBase.ServiceHealth;
using DaysOn.Common.SqlDataAccess.Repositories;
using JustEat.StatsD;
using log4net;
using log4net.Config;
using Owin;
using WebApiContrib.IoC.CastleWindsor;

namespace DaysOn.CompanyBc.Api.App_Start
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public static ServiceHealthStatusEventStore ServiceHealthStatus { get; set; }
        
        public virtual void Configuration(IAppBuilder app)
        {
            XmlConfigurator.Configure();

            GlobalContext.Properties["feature"] = "CompanyBc";
            GlobalContext.Properties["enviroment"] = ConfigurationManager.AppSettings["Enviroment"];

            _logger.LogInf("DaysOn.CompanyBc startup...");

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
                _logger.LogErr("DaysOn.CompanyBc could not load all dependencies. Service status is faulty.");
                app.UseWebApi(GlobalConfiguration.Configuration);
                return;
            }

            _logger.LogInf("All service dependencies are OK!");

            _logger.LogInf("Setting up Windsor Castle container...");

            var serviceBus = ContainerManager.Container.Resolve<IServiceBus>();
            serviceBus.RegisterEvent(typeof(CompanyPingEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<CompanyPingEvent>>());

            app.ConfigureOAuth();

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(GlobalConfiguration.Configuration);

            PublishStatsFilterAttribute.GetStatsDPublisher = ContainerManager.Container.Resolve<IStatsDPublisher>;
            _logger.LogInf("All done! CompanyBC started successfully.");
        }

        private ServiceDependencyResolverResultSummary ServiceDependencyCheck()
        {
            bool isServiceHealthy = true;

            var dependenciesHealthCheckResult = new ServiceDependencyContainer()
                .Register(new List<ServiceDependency>
                {
                                  new ServiceDependency("CompanyContext", () => new Repository<Company>(new CompanyContext())),
                                  new ServiceDependency("IOCComponents", () => WindsorConfigurator.Configure(new WindsorContainer()).Verify())
                              })
                .VerifyAll();

            foreach (var result in dependenciesHealthCheckResult)
            {
                if (result.Value.Resolved)
                {
                    _logger.LogInf(string.Format("Dependency [{0}] resolved.", result.Key));
                }
                else
                {
                    isServiceHealthy = false;
                    var errorMessage = string.Format("Couldn't resove dependency [{0}].ErrorMessage=[{1}].", result.Key, result.Value.Message);
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
                _logger.LogInf(string.Format("Configuration setting key=[{0}], value=[{1}].", key, ConfigurationManager.AppSettings[key]));
            }
        }
    }
}