using System;


namespace DaysOn.HolidaysBc.Models.Commands
{
    public class CorrectionDaysCommand : ApiCommand
    {
        public Guid EmployeeId { get; set; }
        public int Year { get; set; }
        public decimal Days { get; set; }
    }
}
