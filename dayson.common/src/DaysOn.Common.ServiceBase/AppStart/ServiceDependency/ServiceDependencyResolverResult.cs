namespace DaysOn.Common.ServiceBase.ServiceDependency
{
    public class ServiceDependencyResolverResult
    {
        public ServiceDependencyResolverResult()
        {
            Resolved = false;
        }

        public bool Resolved { get; set; }

        public string Message { get; set; }
    }
}