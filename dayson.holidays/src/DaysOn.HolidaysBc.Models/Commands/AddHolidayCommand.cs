using System;
using DaysOn.Common.Events.Dtos;

namespace DaysOn.HolidaysBc.Models.Commands
{
    public class AddHolidayCommand : ApiCommand
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int NumberOfDays { get; set; }
        public string Notes { get; set; }

        public Guid EmployeeId { get; set; }

        public Status Status { get; set; }
    }
}
