using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;

namespace DaysOn.Functions.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    public class VersionController : ApiController
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
            _logger.LogDeb($"VerDebug: {JsonConvertExt.SerializeObject(TestStaticProperties.Keys)}");
            ProcessTestStaticProperties();

            return await Task.Run(
                () =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    return Ok(fileVersionInfo.FileVersion);
                });
        }

        private static void ProcessTestStaticProperties()
        {
            if (TestStaticProperties.Keys == null || TestStaticProperties.Keys.Count > 9)
            {
                TestStaticProperties.Keys = new List<string>();
            }
            else
            {
                TestStaticProperties.Keys.Add(TestStaticProperties.Keys.Count.ToString());
            }
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
            _serviceBus.Publish(new FunctionsPingEvent
            {
                CorrelationId = correlationId,
                SendDate = DateTime.Now,
            });
        }
    }

    public class TestStaticProperties
    {
        public static List<string> Keys;
    }
}
