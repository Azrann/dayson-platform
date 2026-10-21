using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DaysOn.Common.Utilities.Extension;
using log4net;

namespace DaysOn.Common.ServiceBase.ServiceHealth
{
    public class ServiceHealthStatusEventStore
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IList<ServiceHealthEvent> _serviceHealthEvents;

        public ServiceHealthStatusEventStore()
        {
            _serviceHealthEvents = new List<ServiceHealthEvent>();
        }

        public IEnumerable<ServiceHealthEvent> ServiceHealthEvents
        {
            get { return _serviceHealthEvents; }
        }

        public void SetFaultyStatus(string errorMessage)
        {
            var healthEvent = new ServiceHealthEvent(HealthStatus.Faulty, errorMessage);
            _serviceHealthEvents.Add(healthEvent);
            _logger.LogErr($"Service health status = {Enum.GetName(typeof(HealthStatus), healthEvent.HealthStatus)}. Recorderd at {healthEvent.TimestampUtc.ToLocalTime()}. Message={healthEvent.LogMessage}");
        }

        public void SetOkStatus()
        {
            var healthEvent = new ServiceHealthEvent(HealthStatus.Ok, string.Empty);
            _serviceHealthEvents.Add(healthEvent);
            _logger.LogInf(
                $"Service health status = {Enum.GetName(typeof(HealthStatus), healthEvent.HealthStatus)}. Recorderd at {healthEvent.TimestampUtc.ToLocalTime()}. Message={healthEvent.LogMessage}");
        }

        public ServiceHealthEvent GetCurrentStatus()
        {
            return _serviceHealthEvents.Last();
        }
    }
}
