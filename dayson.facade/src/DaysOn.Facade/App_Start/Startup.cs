using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoMapper;
using DaysOn.Common.Domain.Ioc;
using DaysOn.Common.Utilities.Extension;
using DaysOn.Common.ServiceBase.AppStart.Filters;
using DaysOn.CompanyBc.Models.Dtos;
using DaysOn.Facade.Models;
using JustEat.StatsD;
using log4net;
using log4net.Config;
using Owin;
using WebApiContrib.IoC.CastleWindsor;

namespace DaysOn.Facade
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Startup()
        {
            GlobalConfiguration.Configuration = WebApiConfig.GetHttpConfiguration();
        }

        public virtual void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            XmlConfigurator.Configure();

            GlobalContext.Properties["feature"] = "Facade";
            GlobalContext.Properties["enviroment"] = ConfigurationManager.AppSettings["Enviroment"];

            _logger.LogInf("Setting up Windsor Castle container...");

            WindsorConfiguration.Configure(ContainerManager.Container);

            GlobalConfiguration.Configuration.DependencyResolver = new WindsorResolver(ContainerManager.Container);
            app.ConfigureOAuth();
            app.UseWebApi(GlobalConfiguration.Configuration);

            Mapper.Initialize(GetConfig);
            PublishStatsFilterAttribute.GetStatsDPublisher = ContainerManager.Container.Resolve<IStatsDPublisher>;
            _logger.LogInf("Facade server started");
        }

        private void GetConfig(IMapperConfigurationExpression obj)
        {
            obj.CreateMap<ShortEmployeeDto, EmployeeOverviewDto>();
            obj.CreateMap<RichTeamDto, TeamOverviewDto>();
        }
    }
}
