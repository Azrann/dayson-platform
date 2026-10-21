using System.Web.Http;
using System.Web.Http.Cors;
using DaysOn.Log.Web.Services;

namespace DaysOn.Log.Web.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : ApiController
    {
        [Route("logs")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Index()
        {
            LogService service = new LogService();
            var viewModel = service.GetList();
            return Ok(viewModel);
        }

        [Route("errors")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Errors()
        {
            LogService service = new LogService();
            var viewModel = service.GetErrors();
            return Ok(viewModel);
        }

        [Route("clear")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Clear()
        {
            LogService service = new LogService();
            service.CleanLogs();
            return Ok();
        }
    }
}
