using System;

namespace DaysOn.CompanyBc.Models.Commands
{
    public class AddTemporalManagerCommand
    {
        public Guid EmployeeId { get; set; }
        public Guid TeamId { get; set; }
    }
}
