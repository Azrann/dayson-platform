using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using DaysOn.Common.ServiceBase.AppStart.Filters;
using Newtonsoft.Json.Serialization;

namespace DaysOn.Facade
{
    [ExcludeFromCodeCoverage]
    public static class WebApiConfig
    {
        public static HttpConfiguration GetHttpConfiguration()
        {
            var configuration = new HttpConfiguration();

            configuration.MapHttpAttributeRoutes();

            configuration.Formatters.Where(x => x.GetType() != typeof(JsonMediaTypeFormatter))
                         .ToList()
                         .ForEach(x => configuration.Formatters.Remove(x));

            var jsonFormatter = configuration.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ssK";

            configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never;
            configuration.Filters.Add(new ExceptionHandlingAttribute());

            configuration.PublishWebApiTiming();

            GlobalConfiguration.Configuration = configuration;
            return configuration;
        }
    }
}
