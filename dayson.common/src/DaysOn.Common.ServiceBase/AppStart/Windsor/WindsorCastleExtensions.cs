using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Castle.Windsor;
using DaysOn.Common.Utilities.Extension;
using log4net;

namespace DaysOn.Common.ServiceBase.AppStart.Windsor
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
                        logLine = string.IsNullOrEmpty(service?.FullName) ? $"Resolving service where fullName is NullOREmpty [{JsonConvertExt.SerializeObject(service)}]..." : $"Resolving [{service.FullName}]...";
                        container.Resolve(service);
                        logLine += "Ok.";
                    }
                    catch (Exception ex)
                    {
                        logLine += $"Failed. [Exception][{ex.Message}]";
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