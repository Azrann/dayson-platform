using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DaysOn.Log.Web.Controllers
{
   // [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VersionController : ApiController
    {
        [Route("version")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Index()
        {
           return Ok();
        }
    }
}
