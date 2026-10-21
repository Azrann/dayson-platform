using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Filters;
using log4net;
using System.Configuration;
using DaysOn.Common.Utilities.Extension;

namespace DaysOn.Common.ServiceBase.AppStart.Filters
{
    [ExcludeFromCodeCoverage]
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnException(HttpActionExecutedContext context)
        {
            GlobalContext.Properties["feature"] = ConfigurationManager.AppSettings["Feature"]; 
            GlobalContext.Properties["enviroment"] = ConfigurationManager.AppSettings["Enviroment"];

            if (context.Exception.InnerException != null)
            {
                _logger.LogErr(JsonConvertExt.SerializeObject(context.Exception.InnerException));
            }

            _logger.LogErr(JsonConvertExt.SerializeObject(context.Exception));

            if (context.Exception is ArgumentException)
            {
                var result = new CommandResult
                {
                    Succeeded = false,
                    Errors = new List<string> { context.Exception.Message }
                };
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent(typeof(CommandResult), result, new JsonMediaTypeFormatter())
                };
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator."),
                    ReasonPhrase = "Critical Exception"
                });
            }
        }
    }

    public class CommandResult
    {
        public IEnumerable<string> Errors { get; set; }
        public bool Succeeded { get; set; }
    }
}
