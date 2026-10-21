using System;

namespace DaysOn.HolidaysBc.Models.Commands
{
    public class EmployeeHolidayCommand : ApiCommand
    {
        public Guid HolidayId { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
