using System.Collections.Generic;

namespace DaysOn.HolidaysBc.Models.Results
{
    public class CommandResult
    {
        public IEnumerable<string> Errors { get; set; }
        public bool Succeeded { get; set; }
    }
}
