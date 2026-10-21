using System;

namespace DaysOn.HolidaysBc.Models.Commands
{
    public class GetBusinessDaysCommand : ApiCommand
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
