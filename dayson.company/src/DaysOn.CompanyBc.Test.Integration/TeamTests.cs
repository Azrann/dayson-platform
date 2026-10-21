using System;
using System.Linq;
using DaysOn.CompanyBc.BoundedContext.Data;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.BoundedContext.Services;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.CompanyBc.Test.Common;
using NUnit.Framework;

namespace DaysOn.CompanyBc.Test.Integration
{
    [TestFixture]
    public class TeamTests
    {
        private CompanyFactory _factory;
        private ICompanyService _companyService;
        private ITeamService _teamService;
        private Company _company;

        [OneTimeSetUp]
        public void Initialize()
        {
            _factory = new CompanyFactory();
            var context = new CompanyContext();
            _teamService = new TeamService(new Repository<Team>(context), new Repository<Employee>(context));
            _companyService = new CompanyService(new Repository<Company>(context), _teamService, new Repository<Employee>(context), new Repository<Team>(context));

            _company = _factory.CreateCompany();
            _companyService.AddCompany(_company);
        }

        [Test]
        public void WhenIAddATeamICanAccessByItsId()
        {
            Team team = _factory.CreateTeam();
            _companyService.AddTeam(team,_company.Id);

            var result = _teamService.GetTeamById(team.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(team.Name,result.Name);
        }

        [Test]
        public void WhenIUpdateTeamNameICanAccessIt()
        {
            Team team = _factory.CreateTeam();
            _companyService.AddTeam(team, _company.Id);

            team.Name = "UpdatedName";
            _companyService.UpdateTeam(team, _company.Id);
            var result = _teamService.GetTeamById(team.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual("UpdatedName", result.Name);
        }
        [Test]
        public void WhenIRetrieveATeamThatDoesNotExistsIRecieveNullObject()
        {
            var result = _teamService.GetTeamById(Guid.NewGuid());
            Assert.IsNull(result);
        }

        [Test]
        public void WhenIAddATeamMemberToMyTeamICanRetrieveIt()
        {
            Team team = _factory.CreateTeam();
            Employee teamMember = _factory.CreateEmployee();

            _companyService.AddTeam(team, _company.Id);
            _companyService.AddEmployee(teamMember, _company.Id);

            _teamService.AddTeamMember(teamMember.Id, team.Id);

            var result = _teamService.GetTeamById(team.Id);
            var employeeResult = _companyService.GetEmployee(teamMember.Id, _company.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(1,result.TeamMembers.Count);
            Assert.AreEqual(teamMember.Firstname, result.TeamMembers[0].Firstname);
            Assert.IsNotNull(employeeResult.MyTeam);
            Assert.AreEqual(team.Id, employeeResult.MyTeam.Id);
        }

        [Test]
        public void WhenIAddATeamMemberTwo2TeamsTheLastOneWin()
        {
            Team team = _factory.CreateTeam();
            Team team2 = _factory.CreateTeam();
            Employee teamMember = _factory.CreateEmployee();

            _companyService.AddTeam(team, _company.Id);
            _companyService.AddTeam(team2, _company.Id);
            _companyService.AddEmployee(teamMember, _company.Id);

            _teamService.AddTeamMember(teamMember.Id, team.Id);
            _teamService.AddTeamMember(teamMember.Id, team2.Id);

            var teamresult1 = _teamService.GetTeamById(team.Id);
            var teamresult2 = _teamService.GetTeamById(team2.Id);
            var employeeResult = _companyService.GetEmployee(teamMember.Id, _company.Id);

            Assert.IsNotNull(teamresult1);
            Assert.IsNotNull(teamresult2);
            Assert.AreEqual(0, teamresult1.TeamMembers.Count);
            Assert.AreEqual(1, teamresult2.TeamMembers.Count);
            Assert.AreEqual(teamMember.Firstname, teamresult2.TeamMembers[0].Firstname);
            Assert.IsNotNull(employeeResult.MyTeam);
            Assert.AreEqual(team2.Id, employeeResult.MyTeam.Id);
        }

        [Test]
        public void WhenIAddATeamMemberToMyTeamICanRetrieveItWithGetTeam()
        {
            Team team = _factory.CreateTeam();
            Employee teamMember = _factory.CreateEmployee();

            _companyService.AddTeam(team, _company.Id);
            _companyService.AddEmployee(teamMember, _company.Id);

            _teamService.AddTeamMember(teamMember.Id, team.Id);

            var result = _companyService.GetTeam(team.Id, _company.Id);
            var employeeResult = _companyService.GetEmployee(teamMember.Id, _company.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.TeamMembers.Count);
            Assert.AreEqual(teamMember.Firstname, result.TeamMembers[0].Firstname);
            Assert.IsNotNull(employeeResult.MyTeam);
            Assert.AreEqual(team.Id, employeeResult.MyTeam.Id);
        }

        [Test]
        public void WhenIAddAManagerToMyTeamICanRetrieveIt()
        {
            Team team = _factory.CreateTeam();
            Employee manager = _factory.CreateEmployee();

            _companyService.AddTeam(team, _company.Id);
            _companyService.AddEmployee(manager, _company.Id);

            _teamService.SetManager(manager.Id,team.Id);

            var result = _teamService.GetTeamById(team.Id);
            var employeeResult = _companyService.GetEmployee(manager.Id, _company.Id);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Manager);
            Assert.AreEqual(manager.Firstname, result.Manager.Firstname);
            Assert.IsNotNull(result.Manager.ManagerOf.FirstOrDefault(x=>x.Id==team.Id));
            Assert.IsNotNull(employeeResult.ManagerOf);
            Assert.AreEqual(1,employeeResult.ManagerOf.Count);
            Assert.AreEqual(team.Id, employeeResult.ManagerOf[0].Id);
        }

        [Test]
        public void WhenIAddTwoManagerOnlyTheLastOneIsTheManager()
        {
            Team team = _factory.CreateTeam();
            Employee manager = _factory.CreateEmployee();
            Employee manager2 = _factory.CreateEmployee();

            _companyService.AddTeam(team, _company.Id);
            _companyService.AddEmployee(manager, _company.Id);
            _companyService.AddEmployee(manager2, _company.Id);

            _teamService.SetManager(manager2.Id, team.Id);
            _teamService.SetManager(manager.Id, team.Id);

            var result = _teamService.GetTeamById(team.Id);
            var employee1Result = _companyService.GetEmployee(manager.Id, _company.Id);
            var employee2Result = _companyService.GetEmployee(manager2.Id, _company.Id);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Manager);
            Assert.AreEqual(manager.Firstname, result.Manager.Firstname);
            Assert.IsNotNull(result.Manager.ManagerOf.FirstOrDefault(x => x.Id == team.Id));
            Assert.IsNotNull(employee1Result.ManagerOf);
            Assert.AreEqual(1, employee1Result.ManagerOf.Count);
            Assert.AreEqual(team.Id, employee1Result.ManagerOf[0].Id);
            Assert.IsNotNull(employee2Result.ManagerOf);
            Assert.AreEqual(0, employee2Result.ManagerOf.Count);
        }

        [Test]
        public void WhenIAddATemporalManagerICanRetrieveIt()
        {
            Team team = _factory.CreateTeam();
            Employee temporalManager = _factory.CreateEmployee();

            _companyService.AddTeam(team, _company.Id);
            _companyService.AddEmployee(temporalManager, _company.Id);

            _teamService.SetTemporalManager(temporalManager.Id, team.Id);

            var result = _teamService.GetTeamById(team.Id);
            var employeeResult = _companyService.GetEmployee(temporalManager.Id, _company.Id);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.TemporalManager);
            Assert.AreEqual(temporalManager.Firstname, result.TemporalManager.Firstname);
            Assert.IsNotNull(result.TemporalManager.TemporalManagerOf.FirstOrDefault(x => x.Id == team.Id));
            Assert.IsNotNull(employeeResult.TemporalManagerOf);
            Assert.AreEqual(1, employeeResult.TemporalManagerOf.Count);
            Assert.AreEqual(team.Id, employeeResult.TemporalManagerOf[0].Id);
        }

        [Test]
        public void WhenIRemoveAMTeamMemberTheEmployeeIsNotPartOfTheTeamAnymore()
        {
            Team team = _factory.CreateTeam();
            Employee teamMember = _factory.CreateEmployee();

            _companyService.AddTeam(team, _company.Id);
            _companyService.AddEmployee(teamMember, _company.Id);

            _teamService.AddTeamMember(teamMember.Id, team.Id);
            _teamService.RemoveTeamMember(teamMember.Id, team.Id);

            var result = _teamService.GetTeamById(team.Id);
            var employeeResult = _companyService.GetEmployee(teamMember.Id, _company.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(0,result.TeamMembers.Count);
            Assert.IsNull(employeeResult.MyTeam);
        }

        [Test]
        public void WhenIRemoveAManagerTheEmployeeIsNotAManagerAnymore()
        {
            Team team = _factory.CreateTeam();
            Employee manager = _factory.CreateEmployee();

            _companyService.AddTeam(team, _company.Id);
            _companyService.AddEmployee(manager, _company.Id);

            _teamService.SetManager(manager.Id, team.Id);
            _teamService.RemoveManager(team.Id);

            var result = _teamService.GetTeamById(team.Id);
            var employeeResult = _companyService.GetEmployee(manager.Id, _company.Id);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Manager);
            Assert.AreEqual(0, employeeResult.ManagerOf.Count);
        }

        [Test]
        public void WhenIRemoveAManagerTheManagerIdIsEmpty()
        {
            Team team = _factory.CreateTeam();
            Employee manager = _factory.CreateEmployee();

            _companyService.AddTeam(team, _company.Id);
            _companyService.AddEmployee(manager, _company.Id);

            _teamService.SetManager(manager.Id, team.Id);
            _teamService.RemoveManager(team.Id);

            var result = _teamService.GetTeamById(team.Id);
            var employeeResult = _companyService.GetEmployee(manager.Id, _company.Id);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Manager);
            Assert.IsNull(result.ManagerId);
            Assert.AreEqual(0, employeeResult.ManagerOf.Count);
        }


        [Test]
        public void WhenIRemoveATemporalManagerTheEmployeeIsNotAManagerAnymore()
        {
            Team team = _factory.CreateTeam();
            Employee temporalManager = _factory.CreateEmployee();

            _companyService.AddTeam(team, _company.Id);
            _companyService.AddEmployee(temporalManager, _company.Id);

            _teamService.SetTemporalManager(temporalManager.Id, team.Id);
            _teamService.RemoveTemporalManager(team.Id);

            var result = _teamService.GetTeamById(team.Id);
            var employeeResult = _companyService.GetEmployee(temporalManager.Id, _company.Id);

            Assert.IsNotNull(result);
            Assert.IsNull(result.TemporalManager);
            Assert.AreEqual(0, employeeResult.TemporalManagerOf.Count);
        }

        [Test]
        public void WhenIGetAllTeamsICanSeeTheManagersAndTemporalManagers()
        {
            Team team = _factory.CreateTeam();
            Employee manager = _factory.CreateEmployee();
            Employee temporalManager = _factory.CreateEmployee();
            
            _companyService.AddTeam(team, _company.Id);
            _companyService.AddEmployee(manager, _company.Id);
            _companyService.AddEmployee(temporalManager, _company.Id);
           
            _teamService.SetManager(manager.Id, team.Id);
            _teamService.SetTemporalManager(temporalManager.Id, team.Id);
           
            var result = _companyService.GetTeams(_company.Id).First(x => x.Id == team.Id);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Manager);
            Assert.IsNotNull(result.TemporalManager);
            Assert.AreEqual(manager.Id,result.Manager.Id);
            Assert.AreEqual(temporalManager.Id, result.TemporalManager.Id);
        }

        [Test]
        public void WhenIGetAllTeamsWithRichInfoICanSeeInternalProperties()
        {
            Team team = _factory.CreateTeam();
            Employee manager = _factory.CreateEmployee();
            Employee temporalManager = _factory.CreateEmployee();
            Employee teamMember = _factory.CreateEmployee();

            _companyService.AddTeam(team, _company.Id);
            _companyService.AddEmployee(manager, _company.Id);
            _companyService.AddEmployee(temporalManager, _company.Id);
            _companyService.AddEmployee(teamMember, _company.Id);

            _teamService.SetManager(manager.Id, team.Id);
            _teamService.SetTemporalManager(temporalManager.Id, team.Id);
            _teamService.AddTeamMember(teamMember.Id, team.Id);

            var result = _companyService.GetTeams(_company.Id).First(x => x.Id == team.Id);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Manager);
            Assert.IsNotNull(result.TemporalManager);
            Assert.AreEqual(manager.Id, result.Manager.Id);
            Assert.AreEqual(temporalManager.Id, result.TemporalManager.Id);
            Assert.That(temporalManager.PhoneNumber, Is.Not.Null.And.Not.Empty);
            Assert.That(temporalManager.JobTitle, Is.Not.Null.And.Not.Empty);
            Assert.That(temporalManager.Role, Is.Not.Null.And.Not.Empty);
            Assert.That(manager.Firstname, Is.Not.Null.And.Not.Empty);
            Assert.That(manager.Lastname, Is.Not.Null.And.Not.Empty);
            Assert.That(manager.PictureUrl, Is.Not.Null.And.Not.Empty);
            Assert.That(result.TeamMembers[0].EmailAddress, Is.Not.Null.And.Not.Empty);
            Assert.That(result.TeamMembers[0].PhoneNumber, Is.Not.Null.And.Not.Empty);
            Assert.That(result.TeamMembers[0].JobTitle, Is.Not.Null.And.Not.Empty);

        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _companyService.RemoveCompany(_company.Id);
        }
    }
}
