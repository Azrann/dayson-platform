using System;
using System.Diagnostics;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using DaysOn.Common.Utilities.Extension;
using JustEat.StatsD;
using log4net;

namespace DaysOn.Common.ServiceBase.AppStart.Filters
{
    public class PublishStatsFilterAttribute : ActionFilterAttribute
    {
        const string StopwatchKey = "StopwatchFilter.Value";
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static Func<IStatsDPublisher> GetStatsDPublisher { get; set; }
        public static string Enviroment { get; set; }
        public static string Feature { get; set; }

        public override void OnActionExecuting(HttpActionContext context)
        {
            context.Request.Properties[StopwatchKey] = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            if (context.Response == null)
                return;

            if (GetStatsDPublisher == null)
            {
                Logger.Debug("No StatsDPublisherResolver, cannot publish API timing");
                return;
            }

            Stopwatch stopwatch = (Stopwatch)context.Request.Properties[StopwatchKey];

            GetStatsDPublisher().Timing(stopwatch.Elapsed, GetBucketName(context));
        }

        private string GetBucketName(HttpActionExecutedContext context)
        {
            try
            {
                HttpActionContext actionContext = context.ActionContext;

                string controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string actionName = actionContext.ActionDescriptor.ActionName;
                string httpMethod = actionContext.Request.Method.Method;
                int statusCode = (int)actionContext.Response.StatusCode;

                return $"{Enviroment}.{Feature}.webapi.{controllerName}.{actionName}.{httpMethod}.{statusCode}".ToLower();
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception when trying to publish statd stat to URL={context.Request.RequestUri.ToString()} with Exception {JsonConvertExt.SerializeObject(ex)}");
                return "exception-publishing-stat";
            }
        }
    }
}
