using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;
using DaysOn.HolidaysBc.Models.Errors;
using System.Collections.Generic;
using System.Reflection;
using DaysOn.HolidaysBc.Models.Results;
using log4net;

namespace DaysOn.HolidaysBc.Api.Controllers
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

        protected ResponseMessageResult ResponseErrorMessageResult(HttpStatusCode httpStatusCode, IList<string> errorMessages)
        {
            var result = new CommandResult
            {
                Succeeded = false,
                Errors = errorMessages
            };

            return ResponseMessage(new HttpResponseMessage(httpStatusCode)
            {
                Content = new ObjectContent(typeof(CommandResult), result, new JsonMediaTypeFormatter())
            });
        }

        protected ResponseMessageResult ResponseErrorMessageResult(HttpStatusCode httpStatusCode, string errorMessage)
        {
            var result = new CommandResult
            {
                Succeeded = false,
                Errors = new List<string> { errorMessage }
            };

            return ResponseMessage(new HttpResponseMessage(httpStatusCode)
            {
                Content = new ObjectContent(typeof(CommandResult), result, new JsonMediaTypeFormatter())
            });
        }

        protected ResponseMessageResult BadRequestResult(string errorMessage)
        {
            return ResponseErrorMessageResult(HttpStatusCode.BadRequest, errorMessage);
        }

        protected Guid GetCompanyId()
        {
            var identity = User.Identity as ClaimsIdentity;

            var companyClaim = identity.Claims.FirstOrDefault(x => x.Type == "Company");
            if (string.IsNullOrEmpty(companyClaim.Value))
            {
                throw new ArgumentException(HolidayApiErrors.CompanyIdIsNull);
            }
            return new Guid(companyClaim.Value);
        }

        protected List<Guid> GetMyTeams()
        {
            var identity = User.Identity as ClaimsIdentity;
            ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            if (identity.Claims.Any(x => x.Type == "ManagerOf"))
            {
                return identity.Claims.Where(x => x.Type == "ManagerOf").Select(x => new Guid(x.Value)).ToList();
            }
            return Enumerable.Empty<Guid>().ToList();
        }

        protected Guid GetEmployeeId()
        {
            var identity = User.Identity as ClaimsIdentity;

            var companyClaim = identity.Claims.FirstOrDefault(x => x.Type == "Employee");
            if (string.IsNullOrEmpty(companyClaim.Value))
            {
                throw new ArgumentException(HolidayApiErrors.EmployeeIdIsNull);
            }
            return new Guid(companyClaim.Value);
        }

        protected string GetRole()
        {
            var identity = User.Identity as ClaimsIdentity;

            var companyClaim = identity.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            if (string.IsNullOrEmpty(companyClaim.Value))
            {
                throw new ArgumentException(HolidayApiErrors.RoleNotFound);
            }
            return companyClaim.Value;
        }

    }
}