using System;
using Newtonsoft.Json;

namespace DaysOn.CompanyBc.Models.Commands
{
    public class ApiCommand
    {
        public Guid CorrelationId { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
