namespace DaysOn.Common.ServiceBase.ServiceDependency
{
    public interface IServiceDependency
    {
        ServiceDependencyResolverResult Verify();
    }
}