using System;

namespace DaysOn.CompanyBc.SharedKernel.ServiceHealth
{
    public class ServiceHealthEvent
    {
        public ServiceHealthEvent(HealthStatus healthStatus, string logMessage)
        {
            HealthStatus = healthStatus;
            LogMessage = logMessage;
            TimestampUtc = DateTime.UtcNow;
        }

        public HealthStatus HealthStatus { get; set; }

        public DateTime TimestampUtc { get; set; }

        public string LogMessage { get; set; }
    }
}