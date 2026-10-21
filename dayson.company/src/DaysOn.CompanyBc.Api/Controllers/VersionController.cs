using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;
using log4net;

namespace DaysOn.CompanyBc.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    public class VersionController : ApiControllerBase
    {
        private readonly IServiceBus _serviceBus;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public VersionController(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        [Route("version")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAsync()
        {
            _logger.LogDeb("Version method called.");
            return await Task.Run(
                () =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    return Ok(fileVersionInfo.FileVersion);
                });
        }

        [Route("versionAuth")]
        [Authorize]
        public async Task<IHttpActionResult> GetAsyncAuth()
        {
            _logger.LogDeb("versionAuth method called.");
            return await Task.Run(
                () =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    return Ok(fileVersionInfo.FileVersion);
                });
        }

        [Route("ping")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> Ping()
        {
            _logger.LogDeb("Ping method called.");
            var correlationId = Guid.NewGuid();
            SendPingEvent(correlationId);
            _logger.LogDeb("Ping method sent event.");
            if (LastPingEvent.GetDate().AddMinutes(10) > DateTime.Now)
            {
                _logger.LogDeb("Ping method Ok().");
                return Ok();
            }
            _logger.LogDeb("Ping method NotFound().");
            return NotFound();
        }

        private void SendPingEvent(Guid correlationId)
        {
            LastPingEvent.AddGuid(correlationId);
            _serviceBus.Publish(new CompanyPingEvent
            {
                CorrelationId = correlationId,
                SendDate = DateTime.Now,
            });
        }
    }
}
