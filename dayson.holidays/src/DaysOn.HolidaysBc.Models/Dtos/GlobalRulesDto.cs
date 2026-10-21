using System;
using System.Collections.Generic;

namespace DaysOn.HolidaysBc.Models.Dtos
{
    public class GlobalRulesDto
    {
        public int DefaultNumberOfHolidays { get; set; }

        public int MaxConsecutiveDaysOff { get; set; }

        public int MinDaysAdvanceNoticeHolidays { get; set; }

        public int MaxDaysCarriedOverNextYear { get; set; }

        public DateTime YearStartDay { get; set; }

        public Guid CompanyId { get; set; }

        public Dictionary<DayOfWeek, bool> WorkingDaysOfTheWeek { get; set; }

        public bool OpenDuringBankHolidays { get; set; }

        public string CountryId { get; set; }
    }
}
