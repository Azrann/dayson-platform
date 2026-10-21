using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.BoundedContext.Infrastructure;
using DaysOn.Common.SqlDataAccess.Repositories;

namespace DaysOn.CompanyBc.BoundedContext.Services
{
    [CastleComponent("TeamService", LifestyleType.Transient)]
    public class TeamService : ITeamService
    {
        private readonly IRepository<Team> _repositoryTeam;
        private readonly IRepository<Employee> _repositoryEmpl;
        public TeamService(IRepository<Team> repositoryTeamTeam, IRepository<Employee> repositoryEmployee)
        {
            _repositoryTeam = repositoryTeamTeam;
            _repositoryEmpl = repositoryEmployee;
            _repositoryTeam.Context = _repositoryEmpl.Context;
        }

        IEnumerable<Team> ITeamService.GetAll()
        {
            return _repositoryTeam.GetAll(x => true);
        }

        public Team GetTeamById(Guid teamId)
        {
            List<string> include = new List<string> { "Manager", "TemporalManager" };
            return _repositoryTeam.GetAll(x => x.Id == teamId, include).FirstOrDefault();
        }

        public void AddTeamMember(Guid employeeId, Guid teamId)
        {
            Team team = _repositoryTeam.GetAll(x => x.Id == teamId).FirstOrDefault();

            if (team == null)
            {
                throw new ArgumentException(string.Format(Error.TeamNotFound, teamId));
            }

            Employee employee = _repositoryEmpl.GetAll(x => x.Id == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.EmployeeNotFound, employeeId));
            }

            if (employee.MyTeamId.HasValue)
            {
                RemoveTeamMember(employeeId, employee.MyTeamId.Value);
            }

            team.AddTeamMember(employee);
            _repositoryTeam.Save();
        }

        public void RemoveTeamMember(Guid employeeId, Guid teamId)
        {
            Team team = _repositoryTeam.GetAll(x => x.Id == teamId).First();

            if (team == null)
            {
                throw new Exception(string.Format(Error.TeamNotFound, teamId));
            }

            Employee employee = _repositoryEmpl.GetAll(x => x.Id == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.EmployeeNotFound, employeeId));
            }

            team.RemoveTeamMember(employee);
            _repositoryTeam.Save();
        }

        public void RemoveAllTeamMembers(Guid teamId)
        {
            Team team = _repositoryTeam.GetAll(x => x.Id == teamId).First();

            if (team == null)
            {
                throw new Exception(string.Format(Error.TeamNotFound, teamId));
            }

            team.RemoveAllTeamMembers();
            _repositoryTeam.Save();
        }

        public void SetTemporalManager(Guid employeeId, Guid teamId)
        {
            List<string> include = new List<string> { "Manager", "TemporalManager" };
            Team team = _repositoryTeam.GetAll(x => x.Id == teamId, include).First();

            if (team == null)
            {
                throw new Exception(string.Format(Error.TeamNotFound, teamId));
            }

            Employee employee = _repositoryEmpl.GetAll(x => x.Id == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.EmployeeNotFound, employeeId));
            }

            team.SetTemporalManager(employee);
            _repositoryTeam.Save();
        }

        public Guid RemoveTemporalManager(Guid teamId)
        {
            List<string> include = new List<string> { "Manager", "TemporalManager" };
            Team team = _repositoryTeam.GetAll(x => x.Id == teamId, include).First();

            if (team == null)
            {
                throw new Exception(string.Format(Error.TeamNotFound, teamId));
            }

            var temporalManagerId = team.TemporalManagerId ?? Guid.Empty;
            team.RemoveTemporalManager();
            _repositoryTeam.Save();

            return temporalManagerId;
        }

        public void SetManager(Guid employeeId, Guid teamId)
        {
            List<string> include = new List<string> { "Manager", "TemporalManager" };
            Team team = _repositoryTeam.GetAll(x => x.Id == teamId, include).First();

            if (team == null)
            {
                throw new Exception(string.Format(Error.TeamNotFound, teamId));
            }

            Employee employee = _repositoryEmpl.GetAll(x => x.Id == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new ArgumentException(string.Format(Error.EmployeeNotFound, employeeId));
            }

            team.SetManager(employee);
            _repositoryTeam.Save();
        }

        public Guid RemoveManager(Guid teamId)
        {
            List<string> include = new List<string> {"Manager","TemporalManager"};
            Team team = _repositoryTeam.GetAll(x => x.Id == teamId, include).First();

            if (team == null)
            {
                throw new Exception(string.Format(Error.TeamNotFound, teamId));
            }

            var managerId = team.ManagerId ?? Guid.Empty;
            team.RemoveManager();
            _repositoryTeam.Save();

            return managerId;
        }

        public void RemoveTeam(Guid teamId)
        {
            Team team = _repositoryTeam.GetAll(x => x.Id == teamId).First();

            if (team == null)
            {
                throw new Exception(string.Format(Error.TeamNotFound, teamId));
            }

            _repositoryTeam.Delete(team);
            _repositoryTeam.Save();
        }
    }
}
