using System;
using System.Collections.Generic;
using DaysOn.Common.DataAccess;

namespace DaysOn.HolidaysBc.BoundedContext.Entities
{
    public class TeamOwners : TableEntity
    {
        public List<Guid> Owners { get; set; }
        public Guid CompanyId { get; set; }
    }
}