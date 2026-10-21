using System;
using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.User;

namespace DaysOn.Facade.Common.Dtos.WebUser
{
    public class UpdateUserRequest : UserDetailUpdate
    {
        public Guid UserId { get; set; }
    }

    public class UpdateUserResponse
    {
        public UserDetails User { get; set; }
    }

    public class UpdateUserOffline
    {

        public static async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request)
        {
            var user = OfflineDb.Users.First(u => u.Id == request.UserId);

            user.ApproverId = request.ApproverId;
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.IsApprover = request.IsApprover;
            user.JobTitle = request.JobTitle;
            user.LastName = request.LastName;
            user.MiddleName = request.MiddleName;
            user.PictureUrl = request.PictureUrl;
            user.Type = request.Type;
            user.Phone = request.Phone;
            user.TeamId = request.TeamId;
            user.TeamName = OfflineDb.Teams.First(t => t.Id == user.TeamId).Name;
            user.TeamName = request.ApproverId != null ? OfflineDb.Users.First(a => a.Id == request.ApproverId).FullName : null;


            var rules = OfflineDb.UserRules.First(r => r.UserId == request.UserId);
            rules.HolidayEntitlement = request.Rules.HolidayEntitlement;
            rules.MaxConsecutiveDaysOff = request.Rules.MaxConsecutiveDaysOff;
            rules.MaxDaysCarriedOverNextYear= request.Rules.MaxDaysCarriedOverNextYear;
            rules.MinDaysAdvanceNoticeHolidays = request.Rules.MinDaysAdvanceNoticeHolidays;
            rules.WorkingDays = request.Rules.WorkingDays;

            return await Task.FromResult(
                new UpdateUserResponse
                {
                    User = OfflineDb.Users.
                        Where(u => u.Id == request.UserId).
                        Select(u => new UserDetails
                        {
                            Id = u.Id,
                            TeamId = u.TeamId,
                            CompanyId = u.CompanyId,
                            TeamName = OfflineDb.Teams.First(t => t.Id == u.TeamId).Name,
                            Phone = u.Phone,
                            Email = u.Email,
                            ApproverId = u.ApproverId,
                            LastName = u.LastName,
                            FirstName = u.FirstName,
                            PictureUrl = u.PictureUrl,
                            MiddleName = u.MiddleName,
                            IsApprover = u.IsApprover,
                            JobTitle = u.JobTitle,
                            Type = u.Type,
                            Rules = OfflineDb.UserRules.First(r => r.UserId == u.Id),
                            ApproverName = u.ApproverId != null ? OfflineDb.Users.First(a => a.Id == u.ApproverId).FullName : null
                        }).
                        First()
                }
            );
        }
    }
}
