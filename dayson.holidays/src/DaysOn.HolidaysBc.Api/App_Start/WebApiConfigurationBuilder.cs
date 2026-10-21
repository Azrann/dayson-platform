using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using DaysOn.Common.ServiceBase.AppStart.Filters;

namespace DaysOn.HolidaysBc.Api.App_Start
{
    [ExcludeFromCodeCoverage]
    public static class WebApiConfigurationBuilder 
    {
        public static void SetDefaults()
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();

            httpConfiguration.Formatters.Where(x => x.GetType() != typeof(JsonMediaTypeFormatter))
                         .ToList()
                         .ForEach(x => httpConfiguration.Formatters.Remove(x));

            var jsonFormatter = httpConfiguration.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ssK";

            httpConfiguration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never;
            httpConfiguration.Filters.Add(new ExceptionHandlingAttribute());

            httpConfiguration.PublishWebApiTiming();

            GlobalConfiguration.Configuration = httpConfiguration;

        }
    }
}
