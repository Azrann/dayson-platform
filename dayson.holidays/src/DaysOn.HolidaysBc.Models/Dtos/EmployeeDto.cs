using System;
using System.Collections.Generic;

namespace DaysOn.HolidaysBc.Models.Dtos
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public bool IsApprover { get; set; }
        public IdNameDto Approver { get; set; }

        public int HolidayEntitlement { get; set; }
        public int MaxDaysCarriedOverNextYear { get; set; }
        public int MinDaysAdvanceNoticeHolidays { get; set; }
        public int MaxConsecutiveDaysOff { get; set; }
        public DateTime YearStartDay { get; set; }
        public Guid CompanyId { get; set; }
        public Dictionary<DayOfWeek, bool> WorkingDaysOfTheWeek { get; set; }
    }
}
