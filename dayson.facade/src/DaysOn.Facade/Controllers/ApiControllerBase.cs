using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace DaysOn.Facade.Controllers
{
    public class ApiControllerBase : ApiController
    {
        protected string GetTokenFromRequest()
        {
            var token = Request.Headers.GetValues("Authorization");
            if (token.Any())
            {
                if (token.FirstOrDefault().Split(' ').Any())
                {
                    return token.FirstOrDefault().Split(' ')[1];
                }
            }

            return string.Empty;
        }

        protected string GetRole()
        {
            var identity = User.Identity as ClaimsIdentity;

            var roleClaim = identity?.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            if (string.IsNullOrEmpty(roleClaim?.Value))
            {
                return "User";
            }
            return roleClaim.Value;
        }
    }
}
