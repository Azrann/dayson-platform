using System;
using System.Globalization;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DaysOn.Facade.Controllers;
using JustEat.StatsD;

namespace DaysOn.Facade
{
    public static class WindsorConfiguration
    {
        public static IWindsorContainer Configure(IWindsorContainer container)
        {
            container.Register(
                Types.FromAssemblyNamed("DaysOn.Facade")
                    .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(ApiControllerBase)))
                .LifestyleTransient());

            container.Register(
                Types.FromAssemblyNamed("DaysOn.Facade")
                    .Where(t => Attribute.IsDefined(t, typeof(CastleComponentAttribute)))
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient());


            var statsDConfig = new StatsDConfiguration
            {
                Host = "statsd.hopto.org",
                Port = 8125,
                Prefix = "5e4360be-cafa-4a93-bfdd-e1a0989074a3",
            };

            container.Register(Component.For<StatsDConfiguration>().Instance(statsDConfig));
            container.Register(Component.For<IStatsDPublisher>().ImplementedBy<StatsDPublisher>());

            return container;
        }
    }
}