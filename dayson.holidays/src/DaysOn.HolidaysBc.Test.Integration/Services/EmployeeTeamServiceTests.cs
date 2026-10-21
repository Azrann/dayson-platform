using System;
using DaysOn.Common.DataAccess;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using NUnit.Framework;

namespace DaysOn.HolidaysBc.Test.Integration.Services
{
    [TestFixture]
    public class EmployeeTeamServiceTests
    {
        private IEmployeeTeamService _employeeTeamService;

        [OneTimeSetUp]
        public void Initialize()
        {
            var db = new DapperNoSql<EmployeeTeam>();
            _employeeTeamService = new EmployeeTeamService(db);
            db.CreateTableIfNotExists(NoSqlTables.EmployeeTeam);
        }

        [Test]
        public void WhenIRequestForAEmployeeThatDoesNotExists_IGetGuidEmpty()
        {
            var result = _employeeTeamService.GetTeamBy(Guid.NewGuid());
            Assert.AreEqual(Guid.Empty,result);
        }

        [Test]
        public void WhenIAddANewEmployeeWithTeam_ICanRetrieveIt()
        {
            Guid teamId = Guid.NewGuid();
            Guid employeeId = Guid.NewGuid();
            _employeeTeamService.AddEmployeeToATeam(employeeId, teamId);

            var result = _employeeTeamService.GetTeamBy(employeeId);
            Assert.AreEqual(teamId, result);
        }

        [Test]
        public void WhenIAddTheSameEmployee3Times_ItGetUpdated()
        {
            Guid employeeId = Guid.NewGuid();
            Guid teamId1 = Guid.NewGuid();
            Guid teamId2 = Guid.NewGuid();
            Guid teamId3 = Guid.NewGuid();

            _employeeTeamService.AddEmployeeToATeam(employeeId, teamId1);
            _employeeTeamService.AddEmployeeToATeam(employeeId, teamId2);
            _employeeTeamService.AddEmployeeToATeam(employeeId, teamId3);

            var result = _employeeTeamService.GetTeamBy(employeeId);
            Assert.AreEqual(teamId3, result);
        }

        [Test]
        public void WhenIRemoveAEmployeeWithATeam_ItGetsRemoved()
        {
            Guid teamId = Guid.NewGuid();
            Guid employeeId = Guid.NewGuid();
           
            _employeeTeamService.AddEmployeeToATeam(employeeId, teamId);

            _employeeTeamService.RemoveEmployeeFromTeam(employeeId);
            var result = _employeeTeamService.GetTeamBy(teamId);
            Assert.AreEqual(Guid.Empty, result);
        }

        [Test]
        public void WhenIRemoveAEmployeeThatDoesNotExists_DoesNotThrowException()
        {
            _employeeTeamService.RemoveEmployeeFromTeam(Guid.NewGuid());
        }

        [Test]
        public void WhenIAdd2EmployeesAndRemove1_ICanAccessTheOneIDidNotRemove()
        {
            Guid teamId1 = Guid.NewGuid();
            Guid employeeId1 = Guid.NewGuid();
            Guid teamId2 = Guid.NewGuid();
            Guid employeeId2 = Guid.NewGuid();

            _employeeTeamService.AddEmployeeToATeam(employeeId1, teamId1);
            _employeeTeamService.AddEmployeeToATeam(employeeId2, teamId2);

            _employeeTeamService.RemoveEmployeeFromTeam(employeeId1);
            var result1 = _employeeTeamService.GetTeamBy(employeeId2);
            var result2 = _employeeTeamService.GetTeamBy(employeeId1);
            Assert.AreEqual(teamId2, result1);
            Assert.AreEqual(Guid.Empty, result2);
        }
    }
}
