using System;
using System.Linq;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.CompanyBc.BoundedContext.Data;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.BoundedContext.Services;
using DaysOn.CompanyBc.Test.Common;
using NUnit.Framework;

namespace DaysOn.CompanyBc.Test.Integration
{
    [TestFixture]
    public class CompanyTests
    {
        private CompanyFactory _factory;
        private ICompanyService _companyService;
        private Company _company;

        [OneTimeSetUp]
        public void Initialize()
        {
            _factory = new CompanyFactory();
            var context = new CompanyContext();
            _companyService = new CompanyService(new Repository<Company>(context), _factory.CreateTeamService(), new Repository<Employee>(context), new Repository<Team>(context));
            _company = _factory.CreateCompany();
            _companyService.AddCompany(_company);
        }

        [Test]
        public void WhenIAddACompanyThenICanRetrieveIt()
        {
            Company company = _factory.CreateCompany();

            _companyService.AddCompany(company);

            var companyResult = _companyService.GetAll().First(x => x.Id == company.Id);

            Assert.IsNotNull(companyResult);
        }

        [Test]
        public void WhenIAddATeamToAnExistingCompanyThenICanRetrieveIt()
        {
            var myCompany = _companyService.GetAll().First(x => x.Id == _company.Id);
            var count = myCompany.Teams.Count;

            Team team = _factory.CreateTeam();

            _companyService.AddTeam(team, myCompany.Id);

            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);

            Assert.IsNotNull(companyResult);
            Assert.AreEqual(count+1, _company.Teams.Count);
            Assert.IsNotNull(_company.Teams[count]);
        }

        [Test]
        public void WhenIAddATeamToAnExistingCompanyThenICanRetrieveItIndividually()
        {
            var myCompany = _companyService.GetAll().First(x => x.Id == _company.Id);
            var count = myCompany.Teams.Count;

            Team team = _factory.CreateTeam();

            _companyService.AddTeam(team, myCompany.Id);

            var companyResult = _companyService.GetTeam(team.Id, _company.Id);

            Assert.IsNotNull(companyResult);
            Assert.AreEqual(team.Name, companyResult.Name);
        }

        [Test]
        public void When_I_add_a_team_and_the_company_does_not_exists_Then_an_exception_is_raised()
        {
            Assert.That(() => _companyService.AddTeam(_factory.CreateTeam(), new Guid("9d42870f-e0ac-4eb8-9587-a6d6c376a7eb")),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Company with Id 9d42870f-e0ac-4eb8-9587-a6d6c376a7eb not found."));
        }

        [Test]
        public void WhenIRemoveATeamICannotAccessItAnymore()
        {
            Team team = _factory.CreateTeam();
            _companyService.AddTeam(team, _company.Id);

            _companyService.RemoveTeam(team.Id, _company.Id);
        }

        [Test]
        public void WhenIAddAnEmployeeICanRetrieveIt()
        {
            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);
            
            Employee employee1 = _factory.CreateEmployee();
            Employee employee2 = _factory.CreateEmployee();

            _companyService.AddEmployee(employee1, companyResult.Id);
            _companyService.AddEmployee(employee2, companyResult.Id);

            var emp1 = _companyService.GetEmployee(employee1.Id, companyResult.Id);
            Assert.IsNotNull(emp1);
        }

        [Test]
        public void WhenIAddAnEmployeeAndITryToRetrieveItFromTheWrongCompanyIdThenNullIsReturned()
        {
            var companyResult = _companyService.GetAll().First(x => x.Id == _company.Id);

            Employee employee1 = _factory.CreateEmployee();
            Employee employee2 = _factory.CreateEmployee();

            _companyService.AddEmployee(employee1, companyResult.Id);
            _companyService.AddEmployee(employee2, companyResult.Id);

            var emp1 = _companyService.GetEmployee(employee1.Id, Guid.NewGuid());
            Assert.IsNull(emp1);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _companyService.RemoveCompany(_company.Id);
        }
    }
}
