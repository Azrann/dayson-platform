using System;

namespace DaysOn.HolidaysBc.Models.Commands
{
    public class RemoveApproverToAnEmployeeCommand : ApiCommand
    {
        public Guid EmployeeId { get; set; }
    }
}