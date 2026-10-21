using System;
using System.Collections.Generic;
using DaysOn.Common.DataAccess;

namespace DaysOn.HolidaysBc.BoundedContext.Entities
{
    public class TeamMembers : TableEntity
    {
        public TeamMembers() : base()
        {
            Employees = new List<Guid>();
        }

        public List<Guid> Employees { get; set; }
    }
}

