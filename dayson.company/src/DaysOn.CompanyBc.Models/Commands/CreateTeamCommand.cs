using System;

namespace DaysOn.CompanyBc.Models.Commands
{
    public class CreateTeamCommand : ApiCommand
    {
        public string Name { get; set; }
    }

    public class UpdateTeamCommand : ApiCommand
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
