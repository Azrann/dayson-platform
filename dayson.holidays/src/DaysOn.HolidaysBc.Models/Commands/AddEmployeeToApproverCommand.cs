using System;

namespace DaysOn.HolidaysBc.Models.Commands
{
    public class AddEmployeeToApproverCommand : ApiCommand
    {
        public Guid ApproverId { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
