using System;
using System.Collections.Generic;
using DaysOn.CompanyBc.BoundedContext.Entities;

namespace DaysOn.CompanyBc.BoundedContext.Services
{
    public interface ITeamService
    {
        IEnumerable<Team> GetAll();
        Team GetTeamById(Guid teamId);
        void AddTeamMember(Guid employeeId, Guid teamId);
        void RemoveTeamMember(Guid employeeId, Guid teamId);
        void RemoveAllTeamMembers(Guid teamId);
        void SetTemporalManager(Guid employeeId, Guid teamId);
        Guid RemoveTemporalManager(Guid teamId);
        void SetManager(Guid employeeId, Guid teamId);
        Guid RemoveManager(Guid teamId);
        void RemoveTeam(Guid teamId);
    }
}
