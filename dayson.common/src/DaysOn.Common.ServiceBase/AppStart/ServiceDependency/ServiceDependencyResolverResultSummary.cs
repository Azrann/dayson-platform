using System.Collections.Generic;
using System.Linq;
using DaysOn.Common.ServiceBase.ServiceHealth;

namespace DaysOn.Common.ServiceBase.ServiceDependency
{
    public class ServiceDependencyResolverResultSummary : Dictionary<string, ServiceDependencyResolverResult>
    {
        public HealthStatus HealthStatus
        {
            get 
            {
                return this.Any(x => !x.Value.Resolved) ? HealthStatus.Faulty : HealthStatus.Ok;
            }
        }
    }
}