using System;
using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.User;

namespace DaysOn.Facade.Common.Dtos.WebUser
{
    public class GetUserDetailsRequest
    {
        public Guid UserId { get; set; }
    }

    public class GetUserDetailsResponse
    {
        public UserDetails User { get; set; }
    }

    public class GetUserDetailsOffline
    {

        public static async Task<GetUserDetailsResponse> GetUserDetails(GetUserDetailsRequest request)
        {
            return await Task.FromResult(
                new GetUserDetailsResponse
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
