using System;
using Castle.Core;
using DaysOn.Common.DataAccess;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    [CastleComponent("EmployeeTeamService", LifestyleType.Transient)]
    public class EmployeeTeamService : IEmployeeTeamService
    {
        private readonly INoSqlDb<EmployeeTeam> _repositoryTeamMembers;

        public EmployeeTeamService(INoSqlDb<EmployeeTeam> repositoryTeamMembers)
        {
            _repositoryTeamMembers = repositoryTeamMembers;
        }

        public Guid GetTeamBy(Guid employeeId)
        {
            var employeeTeam = _repositoryTeamMembers.GetByKey(NoSqlTables.EmployeeTeam, employeeId.ToString());

            if (employeeTeam == null)
            {
                return Guid.Empty;
            }

            return employeeTeam.Team;
        }

        public void AddEmployeeToATeam(Guid employeeId, Guid teamId)
        {
            var employeeTeam = new EmployeeTeam
            {
                Employee = employeeId,
                Team = teamId
            };

            _repositoryTeamMembers.AddOrUpdate(NoSqlTables.EmployeeTeam, employeeId.ToString(), employeeTeam);
        }

        public void RemoveEmployeeFromTeam(Guid employeeId)
        {
            _repositoryTeamMembers.Delete(NoSqlTables.EmployeeTeam, employeeId.ToString());
        }
    }
}
