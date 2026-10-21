using System.Collections.Generic;
using DaysOn.Common.DataAccess;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.HolidaysBc.BoundedContext.Entities
{
    public class EmployeesByApprover : TableEntity
    {
        public List<IdNameDto> Employees { get; set; }
    }
}
