using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysOn.Functions.Models.Commands
{
    public class CalculateBusinessDaysCommand : ApiCommand
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<DateTime> NonWorkingDays { get; set; }
        public List<DateTime> NonWorkingHalfDays { get; set; }
    }
}
