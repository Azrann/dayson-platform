using System;
using System.Collections.Generic;
using System.Linq;
using DaysOn.Common.SqlDataAccess;
using DaysOn.CompanyBc.BoundedContext.Infrastructure;

namespace DaysOn.CompanyBc.BoundedContext.Entities
{
    public class Team : Entity<Guid> 
    {
        public Team(Guid id) : base(id)
        {
            TeamMembers = new List<Employee>();
        }
        protected Team()
            : base()
        {
            TeamMembers = new List<Employee>();
        }

        public string Name { get; set; }

        public Guid? ManagerId { get; set; }
        public Employee Manager { get; set; }

        public Guid? TemporalManagerId { get; set; }
        public virtual Employee TemporalManager { get; set; }

        public virtual List<Employee> TeamMembers { get; private set; }


        public void SetManager(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull,"Employee"));
            }

            if (TeamMembers.FirstOrDefault(x => x.Id == employee.Id) != null)
            {
                throw new ArgumentException(Error.CannotAddManagerBecauseIsTeamMember);
            }

            Manager?.RemoveManagerOf(this);

            Manager = employee;
            employee.AddManagerOf(this);
        }

        public void RemoveManager()
        {
            if (Manager != null)
            {
                Manager.RemoveManagerOf(this);
                Manager = null;
            }
        }

        public void SetTemporalManager(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull, "Employee"));
            }

            if ((Manager != null) && (Manager.Id == employee.Id))
            {
                return;
            }

            if ((TemporalManager != null) && (TemporalManager.Id == employee.Id))
            {
                return;
            }

            TemporalManager?.RemoveTemporalManagerOf(this);

            TemporalManager = employee;
            employee.AddTemporalManagerOf(this);
        }

        public void RemoveTemporalManager()
        {
            if (TemporalManager != null)
            {
                TemporalManager.RemoveTemporalManagerOf(this);
                TemporalManager = null;
            }
        }

        public void AddTeamMember(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull, "Employee"));
            }

            if (TeamMembers.FirstOrDefault(x => x.Id == employee.Id) != null)
            {
                return;
            }

            if ((Manager != null) && (Manager.Id == employee.Id))
            {
                throw new ArgumentException(Error.CannotAddTeamMemberBecauseIsManager);
            }

            TeamMembers.Add(employee);

            employee.MyTeam?.RemoveTeamMember(employee);

            employee.SetMyTeam(this);
        }

        public void RemoveTeamMember(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull, "Employee"));
            }

            TeamMembers.Remove(employee);
            employee.SetMyTeam(null);
        }

        public void RemoveAllTeamMembers()
        {
            foreach (var employee in TeamMembers)
            {
                employee.SetMyTeam(null);
            }
            TeamMembers.Clear();
        }
    }
}
