using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;
using DaysOn.CompanyBc.Models.Errors;
using log4net;

namespace DaysOn.CompanyBc.Api.Controllers
{
    public abstract class ApiControllerBase : ApiController
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected ResponseMessageResult ResponseMessage<T>(HttpStatusCode httpStatusCode, T result) where T : class
        {
            return ResponseMessage(new HttpResponseMessage(httpStatusCode)
            {
                Content = new ObjectContent(typeof(T), result, new JsonMediaTypeFormatter())
            });
        }

        protected Guid GetCompanyId()
        {
            var identity = User.Identity as ClaimsIdentity;

            var companyClaim = identity.Claims.FirstOrDefault(x => x.Type == "Company");
            if (string.IsNullOrEmpty(companyClaim.Value))
            {
                throw new ArgumentException(CompanyApiErrors.CompanyIdIsNull);
            }
            return new Guid(companyClaim.Value);
        }

        protected Guid GetEmployeeId()
        {
            var identity = User.Identity as ClaimsIdentity;

            var companyClaim = identity.Claims.FirstOrDefault(x => x.Type == "Employee");
            if (string.IsNullOrEmpty(companyClaim.Value))
            {
                //TODO: Change this an all of the references to return Guid.Emty and LogWarning.
                _logger.Warn(CompanyApiErrors.TeamIdIsNull);
                throw new ArgumentException(CompanyApiErrors.EmployeeIdIsNull);
            }
            return new Guid(companyClaim.Value);
        }

        protected Guid GetTeamId()
        {
            var identity = User.Identity as ClaimsIdentity;

            var companyClaim = identity.Claims.FirstOrDefault(x => x.Type == "Team");
            if (string.IsNullOrEmpty(companyClaim.Value))
            {
                _logger.Warn(CompanyApiErrors.TeamIdIsNull);
                return Guid.Empty;
            }
            return new Guid(companyClaim.Value);
        }
    }
}