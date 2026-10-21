using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysOn.Common.Events.Events
{
    public class UserLoggedFirstTimeEvent : BaseEvent
    {
        public string Email { get; set; }
    }
}
