using System.Configuration;
using System.Web.Http;

namespace DaysOn.Common.ServiceBase.AppStart.Filters
{
    public static class HttpConfigurationExtensions
    {
        public static void PublishWebApiTiming(this HttpConfiguration config)
        {
            PublishStatsFilterAttribute.Feature = ConfigurationManager.AppSettings["Feature"];
            PublishStatsFilterAttribute.Enviroment = ConfigurationManager.AppSettings["Enviroment"];

            config.Filters.Add(new PublishStatsFilterAttribute());

        }
    }
}
