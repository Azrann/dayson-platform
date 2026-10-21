using System;
using System.Collections.Generic;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    public interface ITeamMembersService
    {
        void AddEmployeeToATeam(Guid employeeId, Guid teamId);
        List<Guid> GetTeamMembers(Guid teamId);
        void RemoveEmployeeFromTeam(Guid employeeId, Guid teamId);
    }
}