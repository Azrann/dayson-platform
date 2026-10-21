using System;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    public interface IEmployeeTeamService
    {
        void AddEmployeeToATeam(Guid employeeId, Guid teamId);
        Guid GetTeamBy(Guid employeeId);
        void RemoveEmployeeFromTeam(Guid employeeId);
    }
}