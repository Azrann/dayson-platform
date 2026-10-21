using System;
using DaysOn.Common.SqlDataAccess;

namespace DaysOn.HolidaysBc.BoundedContext.Entities
{
    public class Company : Entity<Guid>
    {
        public Company(Guid id) : base(id)
        {
        }

        protected Company()
            : base()
        {
        }

        public string Name { get; set; }

        public Guid GlobalRulesId { get; set; }
        public virtual GlobalRules GlobalRules { get; set; }
    }
}
