using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysOn.Common.Events.Events
{
    public class TeamCreatedBusEvent : BaseEvent
    {
        public Guid CompanyId { get; set; }
        public Guid TeamId { get; set; }
        public string Name { get; set; }
    }
}
