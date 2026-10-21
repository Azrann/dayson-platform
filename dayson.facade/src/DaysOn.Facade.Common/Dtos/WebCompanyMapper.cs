using System;
using System.Collections.Generic;
using DaysOn.Facade.Models.WebModels.Company;
using DaysOn.Facade.Models.WebModels.Shared;
using DaysOn.Facade.Models.WebModels.User;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.Facade.Common.Dtos
{
    public class WebCompanyMapper
    {
        public static GlobalRulesDto GetGlobalRulesDto(CompanyRules rules)
        {
            return new GlobalRulesDto()
            {
                CompanyId = rules.CompanyId,
                YearStartDay = new DateTime(rules.YearStartDay.Year, rules.YearStartDay.Month, rules.YearStartDay.Day),
                DefaultNumberOfHolidays = rules.MinDaysAdvanceNoticeHolidays,
                MaxConsecutiveDaysOff = rules.MaxConsecutiveDaysOff,
                MaxDaysCarriedOverNextYear = rules.MaxDaysCarriedOverNextYear,
                MinDaysAdvanceNoticeHolidays = rules.MinDaysAdvanceNoticeHolidays,
                OpenDuringBankHolidays = false, /*TODO: check trello >> I Checked trello and there is nothing*/
                CountryId = string.Empty,
                WorkingDaysOfTheWeek = new Dictionary<DayOfWeek, bool>()
                {
                    {DayOfWeek.Monday, rules.WorkingDays.Monday},
                    {DayOfWeek.Tuesday, rules.WorkingDays.Tuesday},
                    {DayOfWeek.Wednesday, rules.WorkingDays.Wednesday},
                    {DayOfWeek.Thursday, rules.WorkingDays.Thursday},
                    {DayOfWeek.Friday, rules.WorkingDays.Friday},
                    {DayOfWeek.Saturday, rules.WorkingDays.Saturday},
                    {DayOfWeek.Sunday, rules.WorkingDays.Sunday},
                }
            };
        }

        public static CompanyRules GetCompanyRules(GlobalRulesDto rules)
        {
            return new CompanyRules()
            {
                Id = Guid.NewGuid(),
                CompanyId = rules.CompanyId,
                HolidayEntitlement = rules.DefaultNumberOfHolidays,
                MaxConsecutiveDaysOff = rules.MaxConsecutiveDaysOff,
                MaxDaysCarriedOverNextYear = rules.MaxDaysCarriedOverNextYear,
                MinDaysAdvanceNoticeHolidays = rules.MinDaysAdvanceNoticeHolidays,
                WorkingDays = GetWorkingDays(rules.WorkingDaysOfTheWeek),
                YearStartDay = new Date(rules.YearStartDay.Year, rules.YearStartDay.Month, rules.YearStartDay.Day)
            };
        }

        public static WorkingDays GetWorkingDays(Dictionary<DayOfWeek, bool> workingDaysOfTheWeek)
        {
            return new WorkingDays()
            {
                Monday = workingDaysOfTheWeek[DayOfWeek.Monday],
                Tuesday = workingDaysOfTheWeek[DayOfWeek.Tuesday],
                Wednesday = workingDaysOfTheWeek[DayOfWeek.Wednesday],
                Thursday = workingDaysOfTheWeek[DayOfWeek.Thursday],
                Friday = workingDaysOfTheWeek[DayOfWeek.Friday],
                Saturday = workingDaysOfTheWeek[DayOfWeek.Saturday],
                Sunday = workingDaysOfTheWeek[DayOfWeek.Sunday]
            };
        }

        public static GlobalRulesDto GetGlobalRulesDto(UserRules rules)
        {
            
            return new GlobalRulesDto()
            {
                CompanyId = rules.CompanyId ?? Guid.Empty,
                YearStartDay = new DateTime(rules.YearStartDay.Year, rules.YearStartDay.Month, rules.YearStartDay.Day),
                DefaultNumberOfHolidays = rules.MinDaysAdvanceNoticeHolidays,
                MaxConsecutiveDaysOff = rules.MaxConsecutiveDaysOff,
                MaxDaysCarriedOverNextYear = rules.MaxDaysCarriedOverNextYear,
                MinDaysAdvanceNoticeHolidays = rules.MinDaysAdvanceNoticeHolidays,
                OpenDuringBankHolidays = false, /*TODO: check trello >> I Checked trello and there is nothing*/
                CountryId = string.Empty,
                WorkingDaysOfTheWeek = new Dictionary<DayOfWeek, bool>()
                {
                    {DayOfWeek.Monday, rules.WorkingDays.Monday},
                    {DayOfWeek.Tuesday, rules.WorkingDays.Tuesday},
                    {DayOfWeek.Wednesday, rules.WorkingDays.Wednesday},
                    {DayOfWeek.Thursday, rules.WorkingDays.Thursday},
                    {DayOfWeek.Friday, rules.WorkingDays.Friday},
                    {DayOfWeek.Saturday, rules.WorkingDays.Saturday},
                    {DayOfWeek.Sunday, rules.WorkingDays.Sunday},
                }
            };
        }
    }
}
