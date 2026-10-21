using System;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.Team;

namespace DaysOn.Facade.Common.Dtos.WebTeam
{
    public class CreateTeamRequest
    {
        public string Name { get; set; }
    }

    public class CreateTeamResponse
    {
        public TeamOverview Team { get; set; }
    }

    public class CreateTeamOffline
    {
        public static async Task<CreateTeamResponse> CreateTeam(CreateTeamRequest request)
        {
            var team = new TeamOverview
            {
                CompanyId = OfflineDb.HardcodedCompanyId,
                Id = Guid.NewGuid(),
                MemberCount = 0,
                Name = request.Name
            };

            OfflineDb.Teams.Add(team);

            return await Task.FromResult(
                new CreateTeamResponse
                {
                    Team = team
                }
            );
        }
    }

}
