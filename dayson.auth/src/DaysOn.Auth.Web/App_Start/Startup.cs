using System;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using DaysOn.Auth.Domain.Data;
using DaysOn.Auth.Web.Formats;
using DaysOn.Auth.Web.Providers;
using DaysOn.Common;
using DaysOn.Common.AzureBus;
using DaysOn.Common.DataAccess;
using DaysOn.Common.Domain.Ioc;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.ServiceBase.AppStart.Filters;
using DaysOn.Common.Utilities.Extension;
using JustEat.StatsD;
using log4net;
using log4net.Config;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebApiContrib.IoC.CastleWindsor;

namespace DaysOn.Auth.Web
{
    public class Startup
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Configuration(IAppBuilder app)
        {
            XmlConfigurator.Configure();

            GlobalContext.Properties["feature"] = "Auth";
            GlobalContext.Properties["enviroment"] = ConfigurationManager.AppSettings["Enviroment"];

            _logger.LogInf("Setting up Windsor Castle container...");

            WindsorConfiguration.Configure(ContainerManager.Container);

            _logger.LogInf("Registering Bus events...");

            GetBusConfiguration();

            WebApiConfigurationBuilder.SetDefaults();
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            GlobalConfiguration.Configuration.DependencyResolver = new WindsorResolver(ContainerManager.Container);

            _logger.LogInf("Configuring OAuth...");
            ConfigureOAuthTokenConsumption(app);
            app.UseStageMarker(PipelineStage.Authenticate);
            ConfigureOAuth(app);

            app.UseWebApi(GlobalConfiguration.Configuration);
            PublishStatsFilterAttribute.GetStatsDPublisher = ContainerManager.Container.Resolve<IStatsDPublisher>;
            _logger.LogInf("Auth server started");

            ConfigureNoSQL();
            DatabaseSeed.Run();
        }

        private void GetBusConfiguration()
        {
            var serviceBus = ContainerManager.Container.Resolve<IServiceBus>();
            serviceBus.RegisterEvent(typeof(CompanyCreatedBusEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<CompanyCreatedBusEvent>>());

            serviceBus.RegisterEvent(typeof(EmployeeCreatedBusEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<EmployeeCreatedBusEvent>>());

            serviceBus.RegisterEvent(typeof(TemporalManagerAddedToTeamEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<TemporalManagerAddedToTeamEvent>>());

            serviceBus.RegisterEvent(typeof(TemporalManagerRemovedFromTeamEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<TemporalManagerRemovedFromTeamEvent>>());

            serviceBus.RegisterEvent(typeof(ManagerAddedToTeamEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<ManagerAddedToTeamEvent>>());

            serviceBus.RegisterEvent(typeof(ManagerRemovedFromTeamEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<ManagerRemovedFromTeamEvent>>());

            serviceBus.RegisterEvent(typeof(EmployeeAddedToTeamEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<EmployeeAddedToTeamEvent>>());

            serviceBus.RegisterEvent(typeof(EmployeeRemovedFromTeamEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<EmployeeRemovedFromTeamEvent>>());
            
            serviceBus.RegisterEvent(typeof(UserLoggedFirstTimeEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<UserLoggedFirstTimeEvent>>());

            serviceBus.RegisterEvent(typeof(AuthPingEvent));
            serviceBus.Subscribe(ContainerManager.Container.Resolve<IMessageHandler<AuthPingEvent>>());
            
            serviceBus.RegisterEvent(typeof(SendEmailEvent));
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings["JWT.Issuer"];
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                //TODO change this on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth2/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat(issuer)
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
        }

        private void ConfigureNoSQL()
        {
            var noSql = ContainerManager.Container.Resolve<INoSqlDb<NonActivatedUsers>>();
            noSql.CreateTableIfNotExists(NoSqlTables.NonActivatedUsers);
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings["JWT.Issuer"];
            var audience = ConfigurationManager.AppSettings["JWT.Audience"];
            var secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["JWT.Secret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {

                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audience },
                    IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[]
                    {
                        new SymmetricKeyIssuerSecurityKeyProvider(issuer, secret), 
                    }
                });
        }
    }
}