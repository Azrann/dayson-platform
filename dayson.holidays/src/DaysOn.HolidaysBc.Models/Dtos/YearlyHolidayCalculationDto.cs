using System;

namespace DaysOn.HolidaysBc.Models.Dtos
{
    public class YearlyHolidayCalculationDto
    {
        public int Year { get; set; }
        public decimal DaysCarriedForwardFromPreviousYear { get; set; }
        public decimal DaysCarriedForwardToNextYear { get; set; }
        public decimal TotalHolidays { get; set; }

        public decimal Correction { get; set; }
        public int NumberOfDays { get; set; }
        public decimal NumberOfDaysLeft { get; set; }

        public Guid EmployeeId { get; set; }
    }
}
