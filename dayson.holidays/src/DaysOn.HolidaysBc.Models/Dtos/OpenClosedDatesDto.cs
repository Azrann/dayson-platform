using System;
using System.Collections.Generic;

namespace DaysOn.HolidaysBc.Models.Dtos
{
    public class OpenClosedDatesDto
    {
        public List<DateTime> ClosedDays { get; set; }

        public List<DateTime> ClosedHalfDays { get; set; }
    }
}
