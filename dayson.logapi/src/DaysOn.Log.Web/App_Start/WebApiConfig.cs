using System.Web.Http;

namespace DaysOn.Log.Web.App_Start
{
    static class WebApiConfig
    {
        public static HttpConfiguration GetHttpConfiguration()
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.EnableCors();
            return httpConfiguration;
        }
    }
}