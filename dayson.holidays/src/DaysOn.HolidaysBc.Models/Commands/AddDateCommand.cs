using System;

namespace DaysOn.HolidaysBc.Models.Commands
{
    public class AddDateCommand : ApiCommand
    {
        public DateTime Date { get; set; }
    }
}
