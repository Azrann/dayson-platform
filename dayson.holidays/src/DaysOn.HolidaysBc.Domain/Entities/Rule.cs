using System;
using DaysOn.Common.SqlDataAccess;

namespace DaysOn.HolidaysBc.BoundedContext.Entities
{
    public class Rule: Entity<Guid>
    {
        public Rule(Guid id) : base(id)
        {
        }

        protected Rule()
            : base()
        {
        }

        public string Name { get; set; }

        public Guid TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
