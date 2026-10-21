using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Castle.Windsor;
using log4net;

namespace DaysOn.CompanyBc.SharedKernel.AppStart.Windsor
{
    [ExcludeFromCodeCoverage]
    public static class WindsorCastleExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Verify(this IWindsorContainer container)
        {
            string logLine = string.Empty;

            foreach (var handler in container.Kernel.GetAssignableHandlers(typeof(object)))
            {
                foreach (var service in handler.ComponentModel.Services)
                {
                    try
                    {
                        logLine = string.Format("Resolving [{0}]...", service.FullName);
                        container.Resolve(service);
                        logLine += "Ok.";
                    }
                    catch 
                    {
                        logLine += "Failed.";
                        throw;
                    }
                    finally
                    {
                        Logger.Info(logLine);
                    }
                }
            }
        }
    }
}