using System;
using System.Collections.Generic;

namespace DaysOn.HolidaysBc.Models.Commands
{
    public class AddRecurrentDatesCommand : ApiCommand
    {
        public List<DateTime> ClosedDays { get; set; }

        public List<DateTime> ClosedHalfDays { get; set; }
    }
}
