namespace DaysOn.CompanyBc.SharedKernel.ServiceDependency
{
    public interface IServiceDependency
    {
        ServiceDependencyResolverResult Verify();
    }
}