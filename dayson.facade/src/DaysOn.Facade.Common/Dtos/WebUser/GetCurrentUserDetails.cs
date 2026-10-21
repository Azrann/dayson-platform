using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.User;

namespace DaysOn.Facade.Common.Dtos.WebUser
{
    public class GetCurrentUserDetailsRequest
    {
    }

    public class GetCurrentUserDetailsResponse
    {
        public UserDetails User { get; set; }
    }

    public class GetCurrentUserDetailsOffline
    {

        public static async Task<GetCurrentUserDetailsResponse> GetCurrentUserDetails(GetCurrentUserDetailsRequest request)
        {
            return await Task.FromResult(
                new GetCurrentUserDetailsResponse
                {
                    User = OfflineDb.Users.
                        Where(u => u.Id == OfflineDb.OwnerId).
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
                            MiddleName = u.MiddleName,
                            IsApprover = u.IsApprover,
                            PictureUrl = u.PictureUrl,
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
