using System;
using System.Collections.Generic;
using DaysOn.HolidaysBc.BoundedContext.Entities;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    public interface ITeamService
    {
        IEnumerable<Team> GetAll(Guid companyId);
        Team GetTeamById(Guid teamId);
        void AddRule(Rule rule, Guid teamId);
        void RemoveRule(Guid ruleId, Guid teamId);
        void RemoveAllRules(Guid teamId);
        void RemoveTeam(Guid teamId);
        void Add(Team team);
    }
}
