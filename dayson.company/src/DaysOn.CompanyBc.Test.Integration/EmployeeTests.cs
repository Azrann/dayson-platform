using System;
using System.Collections.Generic;
using System.Linq;
using DaysOn.CompanyBc.BoundedContext.Data;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.BoundedContext.Services;
using DaysOn.Common.SqlDataAccess.Repositories;
using NUnit.Framework;
using DaysOn.CompanyBc.Test.Common;

namespace DaysOn.CompanyBc.Test.Integration
{
    [TestFixture]
    public class EmployeeTests
    {
        private CompanyFactory _factory;
        private ICompanyService _companyService;
        private TeamService _teamService;
        private Company _company;
        private ISearchService _searchService;

        [OneTimeSetUp]
        public void Initialize()
        {
            _factory = new CompanyFactory();
            var context = new CompanyContext();
            _teamService = new TeamService(new Repository<Team>(context), new Repository<Employee>(context));
            _companyService = new CompanyService(new Repository<Company>(context), _teamService, new Repository<Employee>(context), new Repository<Team>(context));
            _company = _factory.CreateCompany();
            _companyService.AddCompany(_company);
            _searchService = new SearchService();
        }

        [Test]
        public void WhenIAddANewEmployeeICanSeeItInTheEmployeeListWithRole()
        {
            Team team = _factory.CreateTeam();
            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);
            _companyService.AddTeam(team, companyResult.Id);

            Employee employee1 = _factory.CreateEmployee();
            Employee employee2 = _factory.CreateEmployee("Admin");

            _companyService.AddEmployee(employee1, companyResult.Id);
            _companyService.AddEmployee(employee2, companyResult.Id);

            var empResult1 = _companyService.GetEmployee(employee1.Id, _company.Id);
            var empResult2 = _companyService.GetEmployee(employee2.Id, _company.Id);

            Assert.AreEqual("User", empResult1.Role);
            Assert.AreEqual("Admin", empResult2.Role);
        }

        [Test]
        public void WhenIAddANewEmployeeICanSeeItInTheEmployeeList()
        {
            Team team = _factory.CreateTeam();
            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);
            _companyService.AddTeam(team, companyResult.Id);

            Employee employee1 = _factory.CreateEmployee();
            Employee employee2 = _factory.CreateEmployee();
            Employee employee3 = _factory.CreateEmployee();
            Employee employee4 = _factory.CreateEmployee();

            _companyService.AddEmployee(employee1, companyResult.Id);
            _companyService.AddEmployee(employee2, companyResult.Id);
            _companyService.AddEmployee(employee3, companyResult.Id);
            _companyService.AddEmployee(employee4, companyResult.Id);

            var teamResult = _teamService.GetTeamById(team.Id);

            _teamService.AddTeamMember(employee1.Id, teamResult.Id);
            _teamService.AddTeamMember(employee2.Id, teamResult.Id);
            _teamService.AddTeamMember(employee3.Id, teamResult.Id);
            _teamService.AddTeamMember(employee4.Id, teamResult.Id);

            teamResult = _teamService.GetTeamById(teamResult.Id);

            Assert.AreEqual(4, teamResult.TeamMembers.Count);
            Assert.AreEqual(team.Id, teamResult.TeamMembers[0].MyTeam.Id);
            Assert.AreEqual(team.Id, teamResult.TeamMembers[3].MyTeam.Id);
        }

        [Test]
        public void WhenIRemoveAEmployeeICannotSeeItInTheEmployeeList()
        {
            Team team = _factory.CreateTeam();
            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);
            _companyService.AddTeam(team, companyResult.Id);

            Employee employee1 = _factory.CreateEmployee();
            Employee employee2 = _factory.CreateEmployee();

            _companyService.AddEmployee(employee1, companyResult.Id);
            _companyService.AddEmployee(employee2, companyResult.Id);

            var teamResult = _teamService.GetTeamById(team.Id);

            _teamService.AddTeamMember(employee1.Id, teamResult.Id);
            _teamService.AddTeamMember(employee2.Id, teamResult.Id);

            teamResult = _teamService.GetTeamById(teamResult.Id);

            _teamService.RemoveTeamMember(employee2.Id, teamResult.Id);
            Assert.AreEqual(1, teamResult.TeamMembers.Count);
            Assert.AreEqual(employee1.Id, teamResult.TeamMembers[0].Id);
        }

        [Test]
        public void WhenIUpdateAnEmployeeICanSeeTheNewDetails()
        {
            Team team = _factory.CreateTeam();
            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);
            _companyService.AddTeam(team, companyResult.Id);

            Employee employee1 = _factory.CreateEmployee();
            Employee employee2 = _factory.CreateEmployee();

            _companyService.AddEmployee(employee1, companyResult.Id);
            _companyService.AddEmployee(employee2, companyResult.Id);

            var teamResult = _teamService.GetTeamById(team.Id);

            _teamService.AddTeamMember(employee1.Id, teamResult.Id);
            _teamService.AddTeamMember(employee2.Id, teamResult.Id);

            Employee updatedEmployee1 = _factory.CreateEmployee();
            updatedEmployee1.Id = employee1.Id;

            _companyService.UpdateEmployee(updatedEmployee1, _company.Id);

            teamResult = _teamService.GetTeamById(teamResult.Id);
            var resultEmployee1 = teamResult.TeamMembers[0];

            Assert.AreEqual(updatedEmployee1.Firstname, resultEmployee1.Firstname);
            Assert.AreEqual(updatedEmployee1.Middlename, resultEmployee1.Middlename);
            Assert.AreEqual(updatedEmployee1.Lastname, resultEmployee1.Lastname);
            Assert.AreEqual(updatedEmployee1.DateOfBirth, resultEmployee1.DateOfBirth);
            Assert.AreEqual(updatedEmployee1.JobTitle, resultEmployee1.JobTitle);
            Assert.AreEqual(updatedEmployee1.OthersJson, resultEmployee1.OthersJson);
            Assert.AreEqual(updatedEmployee1.EmailAddress, resultEmployee1.EmailAddress);
            Assert.AreEqual(updatedEmployee1.PhoneNumber, resultEmployee1.PhoneNumber);
            Assert.AreEqual(updatedEmployee1.PictureUrl, resultEmployee1.PictureUrl);
        }


        [Test]
        public void WhenIRemoveAllEmployeesFromATeamTheyAreStillInTheCompany()
        {
            Team team = _factory.CreateTeam();
            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);
            _companyService.AddTeam(team, companyResult.Id);

            Employee employee1 = _factory.CreateEmployee();
            Employee employee2 = _factory.CreateEmployee();

            _companyService.AddEmployee(employee1, companyResult.Id);
            _companyService.AddEmployee(employee2, companyResult.Id);

            var teamResult = _teamService.GetTeamById(team.Id);

            _teamService.AddTeamMember(employee1.Id, teamResult.Id);
            _teamService.AddTeamMember(employee2.Id, teamResult.Id);

            teamResult = _teamService.GetTeamById(teamResult.Id);

            _teamService.RemoveAllTeamMembers(teamResult.Id);
            companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);
            Assert.AreEqual(0, teamResult.TeamMembers.Count);
            Assert.IsNull(companyResult.Employees[0].MyTeam);
        }


        [Test]
        public void WhenIAddAEmployeeThatDoesNotExistsIGetException()
        {
            Team team = _factory.CreateTeam();
            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);
            _companyService.AddTeam(team, companyResult.Id);
            Assert.That(() => _teamService.AddTeamMember(new Guid("432a12e3-a3bb-492e-be93-792983735925"), team.Id),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Employee with Id 432a12e3-a3bb-492e-be93-792983735925 not found."));
        }

        [Test]
        public void WhenIRemoveAEmployeeThatDoesNotExistsIGetException()
        {
            Team team = _factory.CreateTeam();
            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);
            _companyService.AddTeam(team, companyResult.Id);
            Assert.That(() => _teamService.RemoveTeamMember(new Guid("432a12e3-a3bb-492e-be93-792983735925"), team.Id),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Employee with Id 432a12e3-a3bb-492e-be93-792983735925 not found."));
        }

        [Test]
        public void WhenIAddAEmployeeAndTheTeamDoesNotExistsIGetException()
        {
            Team team = _factory.CreateTeam();
            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);
            _companyService.AddTeam(team, companyResult.Id);
            Employee employee1 = _factory.CreateEmployee();
            _companyService.AddEmployee(employee1, companyResult.Id);
            Assert.That(() => _teamService.AddTeamMember(employee1.Id, new Guid("432a12e3-a3bb-492e-be93-792983735925")),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Team with Id 432a12e3-a3bb-492e-be93-792983735925 not found."));
        }

        [Test]
        public void WhenIRemoveAEmployeeAndTheTeamDoesNotExistsIGetException()
        {
            Team team = _factory.CreateTeam();
            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);
            _companyService.AddTeam(team, companyResult.Id);
            Employee employee1 = _factory.CreateEmployee();
            _companyService.AddEmployee(employee1, companyResult.Id);
            
            Assert.That(() => _teamService.AddTeamMember(employee1.Id, new Guid("432a12e3-a3bb-492e-be93-792983735925")),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Team with Id 432a12e3-a3bb-492e-be93-792983735925 not found."));
        }

        [Test]
        public void WhenISearchIGetTheRightResults()
        {
            Team team = _factory.CreateTeam();
            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);
            _companyService.AddTeam(team, companyResult.Id);

            Employee employee1 = _factory.CreateEmployee();
            Employee employee2 = _factory.CreateEmployee();
            Employee employee3 = _factory.CreateEmployee();
            Employee employee4 = _factory.CreateEmployee();

            _companyService.AddEmployee(employee1, companyResult.Id);
            _companyService.AddEmployee(employee2, companyResult.Id);
            _companyService.AddEmployee(employee3, companyResult.Id);
            _companyService.AddEmployee(employee4, companyResult.Id);

            var teamResult = _teamService.GetTeamById(team.Id);

            _teamService.AddTeamMember(employee1.Id, teamResult.Id);
            _teamService.AddTeamMember(employee2.Id, teamResult.Id);
            _teamService.AddTeamMember(employee3.Id, teamResult.Id);
            _teamService.AddTeamMember(employee4.Id, teamResult.Id);

            var result = _searchService.Search(new List<Guid>() {Guid.NewGuid(), teamResult.Id}, employee1.Role, companyResult.Id).ToList();
            Assert.AreEqual(4, result.Count());
            Assert.AreEqual(team.Id, result[0].MyTeam.Id);
            Assert.AreEqual(team.Id, result[3].MyTeam.Id);

            var result2 = _searchService.Search(new List<Guid>() { Guid.NewGuid(), teamResult.Id }, employee1.Firstname, companyResult.Id).ToList();
            Assert.AreEqual(1, result2.Count());

            var result3 = _searchService.Search(new List<Guid>() { Guid.NewGuid(), teamResult.Id }, string.Empty, companyResult.Id).ToList();
            Assert.AreEqual(4, result3.Count());

            var result4 = _searchService.Search(null, string.Empty, companyResult.Id).ToList();
            Assert.AreEqual(4, result4.Count());
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _companyService.RemoveCompany(_company.Id);
        }
    }
}
