using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DaysOn.Log.Web.Services;

namespace DaysOn.Log.Web.App_Start
{
    internal static class ContainerConfig
    {
        public static WindsorContainer GetWindsorContainer()
        {
            WindsorContainer container = new WindsorContainer();
            container.Register(Component.For<LogService>().ImplementedBy(typeof(LogService)).DependsOn(Dependency.OnValue("container", container)).LifestyleTransient());
            container.Register(Classes.FromThisAssembly().BasedOn<ApiController>().LifestylePerWebRequest());

            return container;
        }
    }
}