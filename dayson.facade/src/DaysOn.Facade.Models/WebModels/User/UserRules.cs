using System;
using DaysOn.Facade.Models.WebModels.Shared;

namespace DaysOn.Facade.Models.WebModels.User
{
    public class UserRules
    {
        public Guid? UserId { get; set; }
        public Guid? CompanyId { get; set; }

        public Date YearStartDay { get; set; }
        public WorkingDays WorkingDays { get; set; }

        public int HolidayEntitlement { get; set; }
        public int MaxDaysCarriedOverNextYear { get; set; }
        public int MinDaysAdvanceNoticeHolidays { get; set; }
        public int MaxConsecutiveDaysOff { get; set; }
    }
}
