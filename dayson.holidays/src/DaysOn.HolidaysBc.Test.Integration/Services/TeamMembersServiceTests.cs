using System;
using DaysOn.Common.DataAccess;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using NUnit.Framework;

namespace DaysOn.HolidaysBc.Test.Integration.Services
{
    [TestFixture]
    public class TeamMembersServiceTests
    {
        private ITeamMembersService _teamMembersService;

        [OneTimeSetUp]
        public void Initialize()
        {
            var db = new DapperNoSql<TeamMembers>();
            _teamMembersService = new TeamMembersService(db);
            db.CreateTableIfNotExists(NoSqlTables.TeamMembers);
        }

        [Test]
        public void WhenIRequestForATeamThatDoesNotExists_IGetEmptyList()
        {
            var result = _teamMembersService.GetTeamMembers(Guid.NewGuid());
            Assert.IsEmpty(result);
        }

        [Test]
        public void WhenIAddANewTeamMember_ICanRetrieveIt()
        {
            Guid teamId = Guid.NewGuid();
            Guid employeeId1 = Guid.NewGuid();
            _teamMembersService.AddEmployeeToATeam(employeeId1, teamId);

            var result = _teamMembersService.GetTeamMembers(teamId);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(employeeId1, result[0]);
        }

        [Test]
        public void WhenIAdd3NewTeamMembers_ICanRetrieveThem()
        {
            Guid teamId = Guid.NewGuid();
            Guid employeeId1 = Guid.NewGuid();
            Guid employeeId2 = Guid.NewGuid();
            Guid employeeId3 = Guid.NewGuid();

            _teamMembersService.AddEmployeeToATeam(employeeId1, teamId);
            _teamMembersService.AddEmployeeToATeam(employeeId2, teamId);
            _teamMembersService.AddEmployeeToATeam(employeeId3, teamId);

            var result = _teamMembersService.GetTeamMembers(teamId);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(employeeId1, result[0]);
            Assert.AreEqual(employeeId2, result[1]);
            Assert.AreEqual(employeeId3, result[2]);
        }

        [Test]
        public void WhenIRemoveATeamMember_ItGetsRemovedFromTheTeam()
        {
            Guid teamId = Guid.NewGuid();
            Guid employeeId1 = Guid.NewGuid();
            Guid employeeId2 = Guid.NewGuid();
            Guid employeeId3 = Guid.NewGuid();

            _teamMembersService.AddEmployeeToATeam(employeeId1, teamId);
            _teamMembersService.AddEmployeeToATeam(employeeId2, teamId);
            _teamMembersService.AddEmployeeToATeam(employeeId3, teamId);

            _teamMembersService.RemoveEmployeeFromTeam(employeeId2, teamId);
            var result = _teamMembersService.GetTeamMembers(teamId);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(employeeId1, result[0]);
            Assert.AreEqual(employeeId3, result[1]);
        }

        [Test]
        public void WhenIAddAEmployeeMultipleTimes_ItGetsDuplicated()
        {
            Guid teamId = Guid.NewGuid();
            Guid employeeId1 = Guid.NewGuid();

            _teamMembersService.AddEmployeeToATeam(employeeId1, teamId);
            _teamMembersService.AddEmployeeToATeam(employeeId1, teamId);
            _teamMembersService.AddEmployeeToATeam(employeeId1, teamId);

            var result = _teamMembersService.GetTeamMembers(teamId);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(employeeId1, result[0]);
            Assert.AreEqual(employeeId1, result[1]);
            Assert.AreEqual(employeeId1, result[2]);
        }


        [Test]
        public void WhenIRemoveAnEmployee_AllDuplicateGetsRemoved()
        {
            Guid teamId = Guid.NewGuid();
            Guid employeeId1 = Guid.NewGuid();

            _teamMembersService.AddEmployeeToATeam(employeeId1, teamId);
            _teamMembersService.AddEmployeeToATeam(employeeId1, teamId);
            _teamMembersService.AddEmployeeToATeam(employeeId1, teamId);

            _teamMembersService.RemoveEmployeeFromTeam(employeeId1,teamId);
            var result = _teamMembersService.GetTeamMembers(teamId);
            Assert.AreEqual(0, result.Count);
        }
    }
}
