using System.Web.Http;
using WebActivatorEx;
using DaysOn.Facade;
using Swashbuckle.Application;
using Swashbuckle.Examples;

[assembly: PostApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace DaysOn.Facade
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                     c.SingleApiVersion("v1", "DaysOn.Facade");
                        c.OperationFilter<ExamplesOperationFilter>();
                        c.OperationFilter<AuthorizationInputOperationFilter>();
                        c.OperationFilter<DescriptionOperationFilter>();
                    })
                .EnableSwaggerUi(c =>
                    {
                    });
        }
    }
}
