using System;
using System.Collections.Generic;
using DaysOn.Common.SqlDataAccess;

namespace DaysOn.CompanyBc.BoundedContext.Entities
{
    public class Employee : Entity<Guid>
    {
        public Employee(Guid id) : base(id)
        {
            ManagerOf = new List<Team>();
            TemporalManagerOf = new List<Team>();
            DateOfBirth = new DateTime(1900, 1, 1);
        }

        protected Employee()
            : base()
        {
            ManagerOf = new List<Team>();
            TemporalManagerOf = new List<Team>();
            DateOfBirth = new DateTime(1900, 1, 1);
        }
        
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string JobTitle { get; set; }
        public string Role { get; set; }
        public string PictureUrl { get; set; }
        public string OthersJson { get; set; }

        public Guid CompanyId { get; private set; }
        public virtual Company Company {get; private set;}

        public Guid? MyTeamId { get; set; }
        public virtual Team MyTeam { get; private set; }

        public virtual List<Team> ManagerOf { get; private set; }
        public virtual List<Team> TemporalManagerOf { get; private set; }

        public void AddManagerOf(Team team)
        {
            ManagerOf.Add(team);
        }

        public void RemoveManagerOf(Team team)
        {
            ManagerOf.Remove(team);
        }

        public void AddTemporalManagerOf(Team team)
        {
            TemporalManagerOf.Add(team);
        }

        public void RemoveTemporalManagerOf(Team team)
        {
            TemporalManagerOf.Remove(team);
        }

        public void SetMyTeam(Team team)
        {
            MyTeam = team;
        }

        public void UpdateDetails(Employee updatedEmployee)
        {
            DateOfBirth = updatedEmployee.DateOfBirth;
            Firstname = !string.IsNullOrEmpty(updatedEmployee.Firstname) ? updatedEmployee.Firstname : Firstname; 
            Middlename = !string.IsNullOrEmpty(updatedEmployee.Middlename) ? updatedEmployee.Middlename : Middlename; 
            Lastname = !string.IsNullOrEmpty(updatedEmployee.Lastname) ? updatedEmployee.Lastname : Lastname;
            PhoneNumber = !string.IsNullOrEmpty(updatedEmployee.PhoneNumber) ? updatedEmployee.PhoneNumber : PhoneNumber; 
            EmailAddress = !string.IsNullOrEmpty(updatedEmployee.EmailAddress) ? updatedEmployee.EmailAddress : EmailAddress;
            JobTitle = !string.IsNullOrEmpty(updatedEmployee.JobTitle) ? updatedEmployee.JobTitle : JobTitle; 
            PictureUrl = !string.IsNullOrEmpty(updatedEmployee.PictureUrl) ? updatedEmployee.PictureUrl : PictureUrl;
            OthersJson = !string.IsNullOrEmpty(updatedEmployee.OthersJson) ? updatedEmployee.OthersJson : OthersJson;
            Role = !string.IsNullOrEmpty(updatedEmployee.Role) ? updatedEmployee.Role : Role;
        }
    }
}
