using System.Collections.Generic;
using DaysOn.Facade.Models;

namespace DaysOn.Facade.Common.Dtos
{
    public class TeamOverviewResponse
    {
        public bool CanAddNewTeams { get; set; }
        public List<TeamOverviewDto> Teams { get; set; }
    }
}
