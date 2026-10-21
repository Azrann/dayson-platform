using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Test.Common;
using NUnit.Framework;

namespace DaysOn.CompanyBc.Test.Unit
{
    [TestFixture]
    public class CompanyTests
    {
        private CompanyFactory _factory;

        [SetUp]
        public void Initialize()
        {
            _factory = new CompanyFactory();
        }

        [Test]
        public void WhenIAddATeamTwoTimesIsOnlyAddedOnce()
        {
            var company = _factory.CreateCompany();

            var team = _factory.CreateTeam();

            company.AddTeam(team);
            company.AddTeam(team);

            Assert.AreEqual(1,company.Teams.Count);
        }

        [Test]
        public void WhenIAddATeamIsAddedToTheCompany()
        {
            var company = _factory.CreateCompany();

            var team = _factory.CreateTeam();

            company.AddTeam(team);

            Assert.AreEqual(1, company.Teams.Count);
        }

        [Test]
        public void WhenIRemoveATeamIsRemovedFromTheCompany()
        {
            var company = _factory.CreateCompany();

            var team = _factory.CreateTeam();

            company.AddTeam(team);
            company.RemoveTeam(team);
            Assert.AreEqual(0, company.Teams.Count);
        }

        [Test]
        public void WhenIRemoveATeamThatDoesntExistsNothingHappends()
        {
            var company = _factory.CreateCompany();

            var team1 = _factory.CreateTeam();
            var team2 = _factory.CreateTeam();
            var team3 = _factory.CreateTeam();
            company.AddTeam(team1);
            company.AddTeam(team2);
            company.RemoveTeam(team3);
            Assert.AreEqual(2, company.Teams.Count);
        }


        [Test]
        public void WhenIAddAEmployeeTwoTimesIsOnlyAddedOnce()
        {
            var company = _factory.CreateCompany();

            var employee = _factory.CreateEmployee();

            company.AddEmployee(employee);
            company.AddEmployee(employee);

            Assert.AreEqual(1, company.Employees.Count);
        }

        [Test]
        public void WhenIAddAEmployeeIsAddedToTheCompany()
        {
            var company = _factory.CreateCompany();

            var employee = _factory.CreateEmployee();

            company.AddEmployee(employee);

            Assert.AreEqual(1, company.Employees.Count);
        }

        [Test]
        public void WhenIRemoveAEmployeeIsRemovedFromTheCompany()
        {
            var company = _factory.CreateCompany();

            var employee = _factory.CreateEmployee();

            company.AddEmployee(employee);
            company.RemoveEmployee(employee);
            Assert.AreEqual(0, company.Employees.Count);
        }

        [Test]
        public void WhenIRemoveAEmployeeThatDoesntExistsNothingHappends()
        {
            var company = _factory.CreateCompany();

            var employee1 = _factory.CreateEmployee();
            var employee2 = _factory.CreateEmployee();
            var employee3 = _factory.CreateEmployee();

            company.AddEmployee(employee1);
            company.AddEmployee(employee2);
            company.RemoveEmployee(employee3);
            Assert.AreEqual(2, company.Employees.Count);
        }

        [Test]
        public void WhenIAddANullEmployeeAnExceptionIsThrown()
        {
            var company = _factory.CreateCompany();
            Assert.That(() => company.AddEmployee(null),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Argument Employee is null."));
        }

        [Test]
        public void WhenIAddANullTeamAnExceptionIsThrown()
        {
            var company = _factory.CreateCompany();
            Assert.That(() => company.AddTeam(null),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Argument Team is null."));
        }

        [Test]
        public void WhenIUpdateAnEmployeeTheUpdatedVersionIsSavedToTheCompany()
        {
            var company = _factory.CreateCompany();

            var employee = _factory.CreateEmployee();
            var updatedEmployee = _factory.CreateEmployee();
            updatedEmployee.Id = employee.Id;

            company.AddEmployee(employee);
            company.UpdateEmployee(updatedEmployee);

            Assert.AreEqual(updatedEmployee.Firstname, company.Employees[0].Firstname);
            Assert.AreEqual(updatedEmployee.Middlename, company.Employees[0].Middlename);
            Assert.AreEqual(updatedEmployee.Lastname, company.Employees[0].Lastname);
            Assert.AreEqual(updatedEmployee.DateOfBirth, company.Employees[0].DateOfBirth);
            Assert.AreEqual(updatedEmployee.JobTitle, company.Employees[0].JobTitle);
            Assert.AreEqual(updatedEmployee.OthersJson, company.Employees[0].OthersJson);
            Assert.AreEqual(updatedEmployee.EmailAddress, company.Employees[0].EmailAddress);
            Assert.AreEqual(updatedEmployee.PhoneNumber, company.Employees[0].PhoneNumber);
            Assert.AreEqual(updatedEmployee.PictureUrl, company.Employees[0].PictureUrl);
        }

        [Test]
        public void WhenIUpdateAnEmployeeAndSomeValuesAreNullOnlySaveTheNonNullValuesToTheCompany()
        {
            var company = _factory.CreateCompany();

            var employee = _factory.CreateEmployee();
            var updatedEmployee = _factory.CreateEmployee();
            updatedEmployee.Id = employee.Id;
            updatedEmployee.Role = null;
            updatedEmployee.EmailAddress = "";
            updatedEmployee.Middlename = null;
            company.AddEmployee(employee);
            company.UpdateEmployee(updatedEmployee);

            Assert.AreEqual(updatedEmployee.Firstname, company.Employees[0].Firstname);
            Assert.AreEqual(employee.Middlename, company.Employees[0].Middlename);
            Assert.AreEqual(updatedEmployee.Lastname, company.Employees[0].Lastname);
            Assert.AreEqual(updatedEmployee.DateOfBirth, company.Employees[0].DateOfBirth);
            Assert.AreEqual(updatedEmployee.JobTitle, company.Employees[0].JobTitle);
            Assert.AreEqual(updatedEmployee.OthersJson, company.Employees[0].OthersJson);
            Assert.AreEqual(employee.EmailAddress, company.Employees[0].EmailAddress);
            Assert.AreEqual(updatedEmployee.PhoneNumber, company.Employees[0].PhoneNumber);
            Assert.AreEqual(updatedEmployee.PictureUrl, company.Employees[0].PictureUrl);
            Assert.AreEqual(employee.Role, company.Employees[0].Role);
        }

        [Test]
        public void WhenIUpdateANullEmployeeAnExceptionIsThrown()
        {
            var company = _factory.CreateCompany();
            Assert.That(() => company.UpdateEmployee(null),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Argument Employee is null."));
        }

        [Test]
        public void WhenIUpdateAnEmployeeThatDoesNotExistsEmployeeNotFound()
        {
            var company = _factory.CreateCompany();
            var employee = _factory.CreateEmployee();
            employee.Id = new Guid("b07a1453-de02-49a0-8ebd-649a93f1b621");
            Assert.That(() => company.UpdateEmployee(employee),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Employee with Id b07a1453-de02-49a0-8ebd-649a93f1b621 not found."));
        }
    }
}
