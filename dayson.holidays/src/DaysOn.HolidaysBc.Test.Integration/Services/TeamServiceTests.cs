using System;
using System.Collections.Generic;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.HolidaysBc.Test.Common;
using NUnit.Framework;
using DaysOn.Common.DataAccess;

namespace DaysOn.HolidaysBc.Test.Integration.Services
{
    [TestFixture]
    public class TeamServiceTests
    {
        private CompanyFactory _factory;
        private ITeamService _teamService;
        private List<Guid> _teamsToRemove;

        [OneTimeSetUp]
        public void Initialize()
        {
            _factory = new CompanyFactory();
            var teamOwnersRepo = new DapperNoSql<TeamOwners>();
            var context = new HolidaysContext();
            _teamService = new TeamService(new Repository<Team>(context),new Repository<Rule>(context));
            _teamsToRemove= new List<Guid>();
            teamOwnersRepo.CreateTableIfNotExists(NoSqlTables.EmployeesByApprover);
        }

        [Test]
        public void WhenIAddATeam_ThenICanRetrieveIt()
        {
            var team = _factory.CreateRichTeam();
            _teamService.Add(team);
            
            var result = _teamService.GetTeamById(team.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(team.Id, result.Id);
            Assert.AreEqual(team.Name, result.Name);
            Assert.AreEqual(team.CompanyId, result.CompanyId);

            _teamsToRemove.Add(team.Id);
        }

        [Test]
        public void WhenIAddATeamThatAlreadyExists_IGetAnException()
        {
            var team = _factory.CreateRichTeam();
            team.Id = new Guid("c2823996-f7f5-4f18-a297-901b176dff76");
            Assert.That(() => _teamService.Add(team),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Team with Id c2823996-f7f5-4f18-a297-901b176dff76 already exists."));

            _teamsToRemove.Add(team.Id);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            foreach (var teamId in _teamsToRemove)
            {
                _teamService.RemoveTeam(teamId);
            }
        }
    }
}