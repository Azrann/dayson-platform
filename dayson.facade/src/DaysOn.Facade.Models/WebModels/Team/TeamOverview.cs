using System;

namespace DaysOn.Facade.Models.WebModels.Team
{
    public class TeamOverview
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }

        // team data
        public string Name { get; set; }

        // aggregated data
        public int MemberCount { get; set; }
    }
}
