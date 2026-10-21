using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Results;

namespace DaysOn.Functions.Api.Controllers
{
    public abstract class ApiControllerBase : ApiController
    {
        protected ResponseMessageResult ResponseMessage<T>(HttpStatusCode httpStatusCode, T result) where T : class
        {
            return ResponseMessage(new HttpResponseMessage(httpStatusCode)
            {
                Content = new ObjectContent(typeof(T), result, new JsonMediaTypeFormatter())
            });
        }      
    }
}