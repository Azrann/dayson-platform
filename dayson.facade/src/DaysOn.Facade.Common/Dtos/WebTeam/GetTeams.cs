using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.Team;

namespace DaysOn.Facade.Common.Dtos.WebTeam
{
    public class GetTeamsRequest
    {
        // nothing for now
    }

    public class GetTeamsResponse
    {
        public List<TeamOverview> Teams { get; set; }
    }

    public class GetTeamsOffline
    {
        public static async Task<GetTeamsResponse> GetTeams(GetTeamsRequest request)
        {
            return await Task.FromResult(
                new GetTeamsResponse
                {
                    Teams = OfflineDb.Teams.Where(t => t.CompanyId == OfflineDb.HardcodedCompanyId).ToList()
                }
            );
        }
    }

}
