using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using DaysOn.Common.DataAccess;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    [CastleComponent("TeamMembersService", LifestyleType.Transient)]
    public class TeamMembersService : ITeamMembersService
    {
        private readonly INoSqlDb<TeamMembers> _repositoryTeamMembers;

        public TeamMembersService(INoSqlDb<TeamMembers> repositoryTeamMembers)
        {
            _repositoryTeamMembers = repositoryTeamMembers;
        }

        public List<Guid> GetTeamMembers(Guid teamId)
        {
            var members = _repositoryTeamMembers.GetByKey(NoSqlTables.TeamMembers, teamId.ToString());

            if (members == null)
            {
                return Enumerable.Empty<Guid>().ToList();
            }

            return members.Employees;
        }

        public void AddEmployeeToATeam(Guid employeeId, Guid teamId)
        {
            var members = _repositoryTeamMembers.GetByKey(NoSqlTables.TeamMembers, teamId.ToString());

            if (members == null)
            {
                members = new TeamMembers();
            }

            members.Employees.Add(employeeId);
            _repositoryTeamMembers.AddOrUpdate(NoSqlTables.TeamMembers, teamId.ToString(), members);
        }

        public void RemoveEmployeeFromTeam(Guid employeeId, Guid teamId)
        {
            var members = _repositoryTeamMembers.GetByKey(NoSqlTables.TeamMembers, teamId.ToString());
            members?.Employees.RemoveAll(x => x == employeeId);

            _repositoryTeamMembers.AddOrUpdate(NoSqlTables.TeamMembers, teamId.ToString(), members);
        }
    }
}
