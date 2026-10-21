using System;
using DaysOn.Common.Events.Dtos;

namespace DaysOn.HolidaysBc.Models.Dtos
{
    public class HolidayRequestDto
    {
        public Guid Id { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdated { get; set; }

        public Status Status { get; set; }
        public decimal NumberOfDays { get; set; }
        public string Notes { get; set; }

        public Guid EmployeeId { get; set; }

        public int Year { get; set; }
    }
}
