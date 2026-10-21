using System;
using System.Collections.Generic;
using System.Linq;
using DaysOn.Common.SqlDataAccess;
using DaysOn.CompanyBc.BoundedContext.Infrastructure;


namespace DaysOn.CompanyBc.BoundedContext.Entities
{
    public class Company : Entity<Guid>
    {
        public Company(Guid id) : base(id)
        {
            Teams = new List<Team>();
            Employees = new List<Employee>();
        }

        protected Company()
            : base()
        {
            if (Teams == null)
            {
                Teams = new List<Team>();
            }

            if (Employees == null)
            {
                Employees = new List<Employee>();
            }
        }

        public string Name { get; set; }
        public List<Team> Teams { get; private set; }
        public List<Employee> Employees { get; private set; }
        public bool Enabled { get; set; }

        public void AddTeam(Team team)
        {
            if (team == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull, "Team"));
            }

            if (Teams.FirstOrDefault(x=> x.Id == team.Id) != null)
            {
                return;
            }

            Teams.Add(team);
        }

        public void RemoveTeam(Team team)
        {
            if (team == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull, "Team"));
            }

            Teams.Remove(team);
        }

        public void AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull, "Employee"));
            }

            if (Employees.FirstOrDefault(x => x.Id == employee.Id) != null)
            {
                return;
            }

            Employees.Add(employee);
        }

        public void RemoveEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull, "Employee"));
            }

            Employees.Remove(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.ArgumentNull, "Employee"));
            }

            var currentEmployee = Employees.FirstOrDefault(x => x.Id == employee.Id);
            if (currentEmployee == null)
            {
                throw new ArgumentException(string.Format(Error.EmployeeNotFound, employee.Id));
            }

            currentEmployee.UpdateDetails(employee);
        }
    }
}
