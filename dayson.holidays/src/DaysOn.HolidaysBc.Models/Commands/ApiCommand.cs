using System;
using Newtonsoft.Json;

namespace DaysOn.HolidaysBc.Models.Commands
{
    public class ApiCommand
    {
        public Guid CorrelationId { get; set; }
        public Guid SubscriberId { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
