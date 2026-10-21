using System;
using System.Collections.Generic;
using DaysOn.Common.SqlDataAccess;
using DaysOn.HolidaysBc.BoundedContext.Infrastructure;

namespace DaysOn.HolidaysBc.BoundedContext.Entities
{
    public class Team : Entity<Guid> 
    {
        public Team(Guid id) : base(id)
        {
            Rules = new List<Rule>();
        }

        protected Team()
            : base()
        {
            Rules = new List<Rule>();
        }

        public string Name { get; set; }

        public Guid CompanyId { get; set; }

        public List<Rule> Rules { get; }
        
        public void AddRule(Rule rule)
        {
            if (rule == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull, "Rule"));
            }

            if (Rules.Contains(rule))
            {
                return;
            }

            rule.Team = this;
            rule.TeamId = Id;

            Rules.Add(rule);
        }

        public void RemoveRule(Rule rule)
        {
            if (rule == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull, "Rule"));
            }


            rule.Team = null;
            rule.TeamId = Guid.Empty;
            Rules.Remove(rule);
        }

        public void RemoveAllRules()
        {
            Rules.Clear();
        }
    }
}
