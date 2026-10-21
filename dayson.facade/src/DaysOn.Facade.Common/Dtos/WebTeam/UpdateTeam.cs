using System;
using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.Team;

namespace DaysOn.Facade.Common.Dtos.WebTeam
{
    public class UpdateTeamRequest
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
    }

    public class UpdateTeamResponse
    {
        public TeamOverview Team { get; set; }
    }

    public class UpdateTeamOffline
    {
        public static async Task<UpdateTeamResponse> UpdateTeam(UpdateTeamRequest request)
        {
            var team = OfflineDb.Teams.First(t => t.Id == request.TeamId);

            team.Name = request.Name;

            foreach (var user in OfflineDb.Users.Where(u => u.TeamId == team.Id))
            {
                user.TeamName = team.Name;
            }

            return await Task.FromResult(
                new UpdateTeamResponse
                {
                    Team = team
                }
            );
        }
    }

}
