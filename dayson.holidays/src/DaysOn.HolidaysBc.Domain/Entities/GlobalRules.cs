using System;
using System.Collections.Generic;
using DaysOn.Common.SqlDataAccess;

namespace DaysOn.HolidaysBc.BoundedContext.Entities
{
    public class GlobalRules : Entity<Guid>
    {
        public GlobalRules(Guid id, Guid companyId) : base(id)
        {
            DefaultNumberOfHolidays = 25;
            MaxConsecutiveDaysOff = 365;
            MinDaysAdvanceNoticeHolidays = 0;
            MaxDaysCarriedOverNextYear = 5;
            YearStartDay = new DateTime(2000, 1, 1);
            SetMondayToFriday();
            OpenDuringBankHolidays = false;
            CompanyId = companyId;
            CountryId = "gb";
        }

        protected GlobalRules()
            : base()
        {
        }

        public int DefaultNumberOfHolidays { get; set; }

        public int MaxConsecutiveDaysOff { get; set; }

        public int MinDaysAdvanceNoticeHolidays { get; set; }

        public int MaxDaysCarriedOverNextYear { get; set; }

        public Guid CompanyId { get; set; }

        public DateTime YearStartDay { get; set; }

        public bool Monday { get; private set; }
        public bool Tuesday { get; private set; }
        public bool Wednesday { get; private set; }
        public bool Thursday { get; private set; }
        public bool Friday { get; private set; }
        public bool Saturday { get; private set; }
        public bool Sunday { get; private set; }

        public bool OpenDuringBankHolidays { get; set; }
        public string CountryId { get; set; }

        public void SetWorkingDay(DayOfWeek dayOfTheWeek)
        {
            switch (dayOfTheWeek)
            {
                case DayOfWeek.Monday: Monday = true; break;
                case DayOfWeek.Tuesday: Tuesday = true; break;
                case DayOfWeek.Wednesday: Wednesday = true; break;
                case DayOfWeek.Thursday: Thursday = true; break;
                case DayOfWeek.Friday: Friday = true; break;
                case DayOfWeek.Saturday: Saturday = true; break;
                case DayOfWeek.Sunday: Sunday = true; break;
            }
        }

        public void SetNonWorkingDay(DayOfWeek dayOfTheWeek)
        {
            switch (dayOfTheWeek)
            {
                case DayOfWeek.Monday: Monday = false; break;
                case DayOfWeek.Tuesday: Tuesday = false; break;
                case DayOfWeek.Wednesday: Wednesday = false; break;
                case DayOfWeek.Thursday: Thursday = false; break;
                case DayOfWeek.Friday: Friday = false; break;
                case DayOfWeek.Saturday: Saturday = false; break;
                case DayOfWeek.Sunday: Sunday = false; break;
            }
        }

        public void InitializeWorkinDaysOfTheWeek(Dictionary<DayOfWeek, bool> daysOfTheWeek)
        {
            Monday = daysOfTheWeek[DayOfWeek.Monday];
            Tuesday = daysOfTheWeek[DayOfWeek.Tuesday];
            Wednesday = daysOfTheWeek[DayOfWeek.Wednesday];
            Thursday = daysOfTheWeek[DayOfWeek.Thursday];
            Friday = daysOfTheWeek[DayOfWeek.Friday];
            Saturday = daysOfTheWeek[DayOfWeek.Saturday];
            Sunday = daysOfTheWeek[DayOfWeek.Sunday];
        }

        public Dictionary<DayOfWeek, bool> WorkingDaysOfTheWeek
        {
            get
            {
                var result = new Dictionary<DayOfWeek, bool>();
                result.Add(DayOfWeek.Monday, Monday);
                result.Add(DayOfWeek.Tuesday, Tuesday);
                result.Add(DayOfWeek.Wednesday, Wednesday);
                result.Add(DayOfWeek.Thursday, Thursday);
                result.Add(DayOfWeek.Friday, Friday);
                result.Add(DayOfWeek.Saturday, Saturday);
                result.Add(DayOfWeek.Sunday, Sunday);
                return result;
            }
        }

        private void SetMondayToFriday()
        {
            Monday = true;
            Tuesday = true;
            Wednesday = true;
            Thursday = true;
            Friday = true;
            Saturday = false;
            Sunday = false;
        }
    }
}
