using Castle.Windsor;

namespace DaysOn.Common.Domain.Ioc
{
    public class ContainerManager
    {
        private static IWindsorContainer _container;

        public static IWindsorContainer Container
        {
            get { return _container ?? (_container = new WindsorContainer()); }
        }

        public static void ReleaseAll()
        {
            if (_container != null)
            {
                _container = new WindsorContainer();
            }
        }
    }
}
