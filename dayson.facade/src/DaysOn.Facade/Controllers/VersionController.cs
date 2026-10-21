using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.Utilities.Extension;
using JustEat.StatsD;
using log4net;

namespace DaysOn.Facade.Controllers
{
    public class VersionController : ApiControllerBase
    {
        private readonly IStatsDPublisher _publisher;
        private ILog _logger;

        public VersionController(IStatsDPublisher publisher)
        {
            _publisher = publisher;
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [Route("version")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAsync()
        {
            _logger.LogInf($"Version method called.");

            return await Task.Run(
                () =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    return Ok(fileVersionInfo.ToString());
                });
        }
    }
}
