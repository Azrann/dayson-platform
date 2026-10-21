using System;
using System.Collections.Generic;
using System.Linq;
using DaysOn.Common.SqlDataAccess;
using DaysOn.HolidaysBc.BoundedContext.Infrastructure;

namespace DaysOn.HolidaysBc.BoundedContext.Entities
{
    public class Employee : Entity<Guid>
    {
        public Employee(Guid id) : base(id)
        {
            HolidayRequests = new List<HolidayRequest>();
            YearlyHolidayCalculations = new List<YearlyHolidayCalculation>();
        }

        protected Employee()
            : base()
        {
            HolidayRequests = new List<HolidayRequest>();
            YearlyHolidayCalculations = new List<YearlyHolidayCalculation>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public Employee Approver { get; set; }

        public int HolidayEntitlement { get; set; }
        public int MaxDaysCarriedOverNextYear { get; set; }
        public int MinDaysAdvanceNoticeHolidays { get; set; }
        public int MaxConsecutiveDaysOff { get; set; }
        public DateTime YearStartDay { get; set; }

        public bool Monday { get; private set; }
        public bool Tuesday { get; private set; }
        public bool Wednesday { get; private set; }
        public bool Thursday { get; private set; }
        public bool Friday { get; private set; }
        public bool Saturday { get; private set; }
        public bool Sunday { get; private set; }

        public bool Enabled { get; set; }
        public bool IsApprover { get; set; }
        public Guid CompanyId { get; set; }

        public List<YearlyHolidayCalculation> YearlyHolidayCalculations { get; set; }
        public List<HolidayRequest> HolidayRequests { get; set; }

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

        public void ApplyGlobalRules(GlobalRules globalRules)
        {
            if (globalRules == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull, "GlobalRules"));
            }

            HolidayEntitlement = globalRules.DefaultNumberOfHolidays;
            MaxDaysCarriedOverNextYear = globalRules.MaxDaysCarriedOverNextYear;
            MaxConsecutiveDaysOff = globalRules.MaxConsecutiveDaysOff;
            MinDaysAdvanceNoticeHolidays = globalRules.MinDaysAdvanceNoticeHolidays;
            YearStartDay = globalRules.YearStartDay;
            InitializeWorkinDaysOfTheWeek(globalRules.WorkingDaysOfTheWeek);
        }

        public YearlyHolidayCalculation GetCalculationInYear(int year)
        {
            return YearlyHolidayCalculations.FirstOrDefault(x => x.Year == year);
        }

        public YearlyHolidayCalculation CreateCalculationInYear(int year)
        {
            var result = new YearlyHolidayCalculation(Guid.NewGuid())
            {
                CreatedOn = DateTime.Now,
                NumberOfDays = HolidayEntitlement,
                NumberOfDaysLeft = HolidayEntitlement,
                Year = year
            };

            YearlyHolidayCalculations.Add(result);

            return result;
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
    }
}
