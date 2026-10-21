using System;
using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.User;

namespace DaysOn.Facade.Common.Dtos.WebUser
{
    public class CreateUserRequest : UserDetailUpdate
    {
    }

    public class CreateUserResponse
    {
        public UserOverview User { get; set; }
    }

    public class CreateUserOffline
    {
        public static async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            var user = new UserOverview
            {
                Id = Guid.NewGuid(),
                TeamId = request.TeamId,
                TeamName = OfflineDb.Teams.First(t => t.Id == request.TeamId).Name,
                CompanyId = OfflineDb.HardcodedCompanyId,
                ApproverId = request.ApproverId,
                Phone = request.Phone,
                LastName = request.LastName,
                Email = request.Email,
                MiddleName = request.MiddleName,
                IsApprover = request.IsApprover,
                FirstName = request.FirstName,
                PictureUrl = request.PictureUrl,
                Type = request.Type,
                JobTitle = request.JobTitle,
            };

            OfflineDb.Users.Add(user);
            OfflineDb.UserRules.Add(new UserRules
            {
                WorkingDays = request.Rules.WorkingDays,
                HolidayEntitlement = request.Rules.HolidayEntitlement,
                MaxDaysCarriedOverNextYear = request.Rules.MaxDaysCarriedOverNextYear,
                MaxConsecutiveDaysOff = request.Rules.MaxConsecutiveDaysOff,
                MinDaysAdvanceNoticeHolidays = request.Rules.MinDaysAdvanceNoticeHolidays,
                UserId = user.Id,
            });

            return await Task.FromResult(new CreateUserResponse
            {
                User = user
            });
        }
    }
}
