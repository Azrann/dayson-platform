using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.User;

namespace DaysOn.Facade.Common.Dtos.WebTeam
{
    public class GetTeamApproversRequest
    {
        public Guid TeamId { get; set; }
    }

    public class GetTeamApproversResponse
    {
        public List<ApproverOverview> Approvers { get; set; }
    }

    public class GetTeamApproversOffline
    {
        public static async Task<GetTeamApproversResponse> GetTeamApprovers(GetTeamApproversRequest request)
        {
            return await Task.FromResult(
                new GetTeamApproversResponse
                {
                    Approvers = OfflineDb.Users.
                        Where(u => u.TeamId == request.TeamId && u.IsApprover).
                        Select(u => new ApproverOverview
                        {
                            Id = u.Id,
                            CompanyId = u.CompanyId,
                            TeamId = u.TeamId,
                            TeamName = OfflineDb.Teams.First(t => t.Id == u.TeamId).Name,
                            LastName = u.LastName,
                            IsApprover = u.IsApprover,
                            ApproverId = u.ApproverId,
                            JobTitle = u.JobTitle,
                            Phone = u.Phone,
                            FirstName = u.FirstName,
                            PictureUrl = u.PictureUrl,
                            Email = u.Email,
                            Type = u.Type,
                            MiddleName = u.MiddleName,

                        }).
                        ToList()
                }
            );
        }
    }

}
