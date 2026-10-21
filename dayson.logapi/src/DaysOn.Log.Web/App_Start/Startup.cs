using System.Reflection;
using System.Web.Http;
using log4net;
using log4net.Config;
using Microsoft.Owin;
using Owin;
using WebApiContrib.IoC.CastleWindsor;

[assembly: OwinStartup(typeof(DaysOn.Log.Web.App_Start.Startup))]
namespace DaysOn.Log.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            logger.Info("DaysOn.Log API Started.");
            HttpConfiguration configuration = WebApiConfig.GetHttpConfiguration();

            var container = ContainerConfig.GetWindsorContainer();

            configuration.DependencyResolver = new WindsorResolver(container);

            app.UseWebApi(configuration);
        }
    }
}
