using System;
using System.Collections.Generic;
using System.Linq;
using DaysOn.Facade.Models.WebModels.Company;
using DaysOn.Facade.Models.WebModels.Holiday;
using DaysOn.Facade.Models.WebModels.Shared;
using DaysOn.Facade.Models.WebModels.Team;
using DaysOn.Facade.Models.WebModels.User;

namespace DaysOn.Facade.Common.Database
{
    internal class OfflineDb
    {
        public static readonly Guid HardcodedCompanyId = Guid.NewGuid();
        public static readonly Guid OwnerId = Guid.NewGuid();

        public static List<UserOverview> Users = new List<UserOverview>();
        public static List<UserRules> UserRules = new List<UserRules>();
        public static List<CompanyOverview> Companies = new List<CompanyOverview>();
        public static List<CompanyRules> CompanyRules = new List<CompanyRules>();
        public static List<TeamOverview> Teams = new List<TeamOverview>();
        public static List<HolidayOverview> Holidays = new List<HolidayOverview>();

        static OfflineDb()
        {
            Companies.Add(new CompanyOverview
            {
                Id = HardcodedCompanyId,
                Name = "Test company",
                OwnerId = OwnerId
            });

            Guid teamId = Guid.NewGuid();

            Teams.Add(new TeamOverview
            {
                Id = teamId,
                CompanyId = HardcodedCompanyId,
                MemberCount = 1,
                Name = "Default team"
            });

            Users.Add(new UserOverview
            {
                Id = OwnerId,
                ApproverId = OwnerId,
                CompanyId = HardcodedCompanyId,
                Email = "user@example.invalid",
                FirstName = "Admin",
                MiddleName = "nis",
                LastName = "strator",
                IsApprover = true,
                JobTitle = "Company administrator",
                Phone = "REPLACE_WITH_YOUR_VALUE",
                TeamId = teamId,
                TeamName = Teams.First(t => t.Id == teamId).Name,
                PictureUrl = "",
                Type = UserType.Administrator
            });

            CompanyRules.Add(new CompanyRules
            {
                Id = Guid.NewGuid(),
                CompanyId = HardcodedCompanyId,
                HolidayEntitlement = 25,
                YearStartDay = new Date(2018, 1, 1),
                MaxDaysCarriedOverNextYear = 5,
                MaxConsecutiveDaysOff = 15,
                WorkingDays = new WorkingDays
                {
                    Monday = true,
                    Tuesday = true,
                    Wednesday = true,
                    Thursday = true,
                    Friday = true,
                    Saturday = false,
                    Sunday = false
                },
                MinDaysAdvanceNoticeHolidays = 14
            });

            UserRules.Add(new UserRules
            {
                UserId = OwnerId,
                HolidayEntitlement = 25,
                MaxDaysCarriedOverNextYear = 5,
                MaxConsecutiveDaysOff = 15,
                WorkingDays = new WorkingDays
                {
                    Monday = true,
                    Tuesday = true,
                    Wednesday = true,
                    Thursday = true,
                    Friday = true,
                    Saturday = false,
                    Sunday = false
                },
                MinDaysAdvanceNoticeHolidays = 14
            });
        }
    }
}
