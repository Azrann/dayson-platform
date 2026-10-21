using System;

namespace DaysOn.CompanyBc.Models.Commands
{
    public class AddManagerCommand
    {
        public Guid EmployeeId { get; set; }
        public Guid TeamId { get; set; }
    }
}
