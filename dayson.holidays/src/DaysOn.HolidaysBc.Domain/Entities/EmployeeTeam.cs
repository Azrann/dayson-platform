using System;
using DaysOn.Common.DataAccess;

namespace DaysOn.HolidaysBc.BoundedContext.Entities
{
    public class EmployeeTeam : TableEntity
    {
        public Guid Employee { get; set; }
        public Guid Team { get; set; }
    }
}

