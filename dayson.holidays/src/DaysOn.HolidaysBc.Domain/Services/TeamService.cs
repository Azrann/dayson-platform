using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Infrastructure;
using DaysOn.Common.SqlDataAccess.Repositories;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    [CastleComponent("TeamService", LifestyleType.Transient)]
    public class TeamService : ITeamService
    {
        private readonly IRepository<Team> _repositoryTeam;
        private readonly IRepository<Rule> _repositoryRule;

        public TeamService(IRepository<Team> repositoryTeamTeam, IRepository<Rule> repositoryRule)
        {
            _repositoryTeam = repositoryTeamTeam;
            _repositoryRule = repositoryRule;
            _repositoryRule.Context = _repositoryTeam.Context;
        }

        IEnumerable<Team> ITeamService.GetAll(Guid companyId)
        {
            return _repositoryTeam.GetAll(x => x.CompanyId == companyId);
        }

        public Team GetTeamById(Guid teamId)
        {
            List<string> include = new List<string>() { "Rules" };
            return _repositoryTeam.GetAll(x => x.Id == teamId, include).FirstOrDefault();
        }

        public void AddRule(Rule rule, Guid teamId)
        {
            Team team = _repositoryTeam.GetAll(x => x.Id == teamId).FirstOrDefault();

            if (team == null)
            {
                throw new ArgumentException(string.Format(Error.TeamNotFound, teamId));
            }

            team.AddRule(rule);
            _repositoryTeam.Save();
        }

        public void RemoveRule(Guid ruleId, Guid teamId)
        {
            Team team = _repositoryTeam.GetAll(x => x.Id == teamId).FirstOrDefault();

            if (team == null)
            {
                throw new Exception(string.Format(Error.TeamNotFound, teamId));
            }

            Rule rule = _repositoryRule.GetAll(x => x.Id == ruleId).FirstOrDefault();

            if (rule == null)
            {
                throw new ArgumentException(string.Format(Error.RuleNotFound, ruleId));
            }

            team.RemoveRule(rule);
            _repositoryTeam.Save();
        }

        public void RemoveAllRules(Guid teamId)
        {
            Team team = _repositoryTeam.GetAll(x => x.Id == teamId).FirstOrDefault();

            if (team == null)
            {
                throw new Exception(string.Format(Error.TeamNotFound, teamId));
            }

            team.RemoveAllRules();
            _repositoryTeam.Save();
        }

        public void RemoveTeam(Guid teamId)
        {
            Team team = _repositoryTeam.GetAll(x => x.Id == teamId).FirstOrDefault();

            if (team == null)
            {
                return;
            }

            _repositoryTeam.Delete(team);
            _repositoryTeam.Save();
        }

        public void Add(Team team)
        {
            Team teamResult = _repositoryTeam.GetAll(x => x.Id == team.Id).FirstOrDefault();

            if (teamResult != null)
            {
                throw new ArgumentException(string.Format(Error.TeamAlreadyExists, team.Id));
            }

            _repositoryTeam.Add(team);
            _repositoryTeam.Save();
        }
    }
}
