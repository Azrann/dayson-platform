using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using log4net;
using System.Reflection;

namespace DaysOn.Auth.Web.Controllers
{
    public abstract class ApiControllerBase : ApiController
    {
        private const string CorrelationIdHeader = "X-CorrelationId";
        protected readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected ResponseMessageResult ResponseMessage<T>(HttpStatusCode httpStatusCode, T result) where T : class
        {
            return ResponseMessage(new HttpResponseMessage(httpStatusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(result))
            });
        }

        protected string GetCorrelationId()
        {
            if (Request?.Headers == null || !Request.Headers.Contains(CorrelationIdHeader))
            {
                return string.Empty;
            }

            return $"and correlation Id = {Request.Headers.GetValues(CorrelationIdHeader).FirstOrDefault()}";
        }

        protected string GetCompanyFromClaim()
        {
            var identity = User.Identity as ClaimsIdentity;

            var companyClaim = identity?.Claims.FirstOrDefault(x => x.Type == "Company");

            return companyClaim?.Value;
        }
    }
}