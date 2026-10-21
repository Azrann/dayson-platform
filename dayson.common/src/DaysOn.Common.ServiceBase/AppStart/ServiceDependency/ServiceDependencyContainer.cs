using System.Collections.Generic;

namespace DaysOn.Common.ServiceBase.ServiceDependency
{
    public class ServiceDependencyContainer
    {
        private readonly List<ServiceDependency> _dependencies;

        public ServiceDependencyContainer()
        {
            _dependencies = new List<ServiceDependency>();
        }

        public ServiceDependencyContainer Register(ServiceDependency dependency)
        {
            _dependencies.Add(dependency);
            return this;
        }

        public ServiceDependencyContainer Register(List<ServiceDependency> dependencies)
        {
            dependencies.ForEach(d => Register(d));
            return this;
        }

        public ServiceDependencyResolverResultSummary VerifyAll()
        {
            var serviceDependencyResolverResults = new ServiceDependencyResolverResultSummary();

            foreach (var serviceDependency in _dependencies)
            {
                serviceDependencyResolverResults.Add(serviceDependency.Name, serviceDependency.Verify().Result);
            }

            return serviceDependencyResolverResults;
        }
    }
}