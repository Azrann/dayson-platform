using System;

namespace DaysOn.CompanyBc.Models.Commands
{
    public class RemoveEmployeeFromATeamCommand
    {
        public Guid EmployeeId { get; set; }
        public Guid TeamId { get; set; }
    }
}
