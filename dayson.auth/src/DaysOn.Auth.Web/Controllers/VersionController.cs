using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;
using log4net;

namespace DaysOn.Auth.Web.Controllers
{
    public class VersionController : ApiControllerBase
    {
        private readonly IServiceBus _serviceBus;

        public VersionController(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        [Route("version")]
        [HttpGet]
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
        [Authorize(Roles = "Admin, Owner, MegaAdmin")]
        [HttpGet]
        public async Task<IHttpActionResult> GetVersionWithAsync()
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

        [Route("ping")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> Ping()
        {
            _logger.LogDeb("Ping method called.");
            var correlationId = Guid.NewGuid();
            SendPingEvent(correlationId);

            if (LastPingEvent.GetDate().AddMinutes(10) > DateTime.Now)
            {
                return Ok();
            }
            return NotFound();
        }

        private void SendPingEvent(Guid correlationId)
        {
            LastPingEvent.AddGuid(correlationId);
            _serviceBus.Publish(new AuthPingEvent
            {
                CorrelationId = correlationId,
                SendDate = DateTime.Now,
            });
        }
    }
}