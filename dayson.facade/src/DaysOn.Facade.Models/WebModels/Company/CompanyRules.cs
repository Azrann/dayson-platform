using System;
using DaysOn.Facade.Models.WebModels.Shared;
using DaysOn.Facade.Models.WebModels.User;

namespace DaysOn.Facade.Models.WebModels.Company
{
    public class CompanyRules
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }

        public Date YearStartDay { get; set; }

        public WorkingDays WorkingDays { get; set; }

        public int HolidayEntitlement { get; set; }
        public int MaxDaysCarriedOverNextYear { get; set; }
        public int MinDaysAdvanceNoticeHolidays { get; set; }
        public int MaxConsecutiveDaysOff { get; set; }
    }
}
