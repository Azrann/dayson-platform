using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DaysOn.CompanyBc.SharedKernel.ServiceDependency
{
    public class ServiceDependency
    {
        private const int DefaultTimeoutMilliseconds = 10000;
        private readonly Action _resolver;
        private readonly int _timoutMilliseconds;

        public ServiceDependency(string name, Action resolver)
        {
            _timoutMilliseconds = DefaultTimeoutMilliseconds;
            Name = name;
            _resolver = resolver;
        }

        public ServiceDependency(string name, Action resolver, int timeoutInMilliseconds)
        {
            _timoutMilliseconds = timeoutInMilliseconds;
            Name = name;
            _resolver = resolver;
        }

        public string Name { get; private set; }

        public async Task<ServiceDependencyResolverResult> Verify()
        {
            var task = Task.Run(() => ResolveWrapper());
            var task2 = Task.Delay(_timoutMilliseconds);
            
            var tasks = new List<Task> { task, task2 };
            var whenAnyResult = await Task.WhenAny(tasks);
            if (whenAnyResult == task)
            {
                return task.Result;
            }

            return new ServiceDependencyResolverResult
            {
                           Resolved = false,
                           Message = "timeout expired"
                       };
        }

        private ServiceDependencyResolverResult ResolveWrapper()
        {
            var resolved = new ServiceDependencyResolverResult();

            try
            {
                _resolver();
                resolved.Resolved = true;
            }
            catch (Exception ex)
            {
                resolved.Resolved = false;
                resolved.Message = ex.ToString();
            }

            return resolved;
        }
    }
}