using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysOn.Common.Events.Events
{
    public class SendEmailEvent: BaseEvent
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string BodyHtml { get; set; }
        public string PlainTextContent { get; set; }
    }
}
