using System;
using System.Collections.Generic;
using System.Linq;
using DaysOn.Common.DataAccess;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.HolidaysBc.Test.Common;
using NUnit.Framework;

namespace DaysOn.HolidaysBc.Test.Integration.Repositories
{
    [TestFixture]
    public class EmployeeRepoTest
    {
        private IEmployeeRepo _repo;
        private CompanyFactory _factory;
        private ICompanyService _companyService;
        private IEmployeeService _employeeService;
        private List<Employee> _employeesToRemove;
        private Company _company;

        [OneTimeSetUp]
        public void Initialize()
        {
            _repo = new EmployeeRepo();
            _employeesToRemove = new List<Employee>();
            _factory = new CompanyFactory();
            var context = new HolidaysContext();
            var context2 = new HolidaysContext();

            _companyService = new CompanyService(new Repository<Company>(context), new DapperNoSql<OpenClosedDates>());
            _employeeService = new EmployeeService(new Repository<Employee>(context2), _companyService);

            _company = _factory.CreateCompany();
            _companyService.AddCompany(_company);
        }

        [Test]
        public void WhenICallGetHolidaysICanRetrieveTheHoliday()
        {
            var employee = _factory.CreateEmployee();
            _employeeService.AddEmployee(employee);
            Guid employeeId = employee.Id;
            Guid id = Guid.NewGuid();
            var employeeDetails = _repo.GetEmployeeDetails(employeeId);

            Assert.AreEqual(employee.Id, employeeDetails.Id);
            Assert.AreEqual(employee.CompanyId, employeeDetails.CompanyId);
            Assert.AreEqual(employee.IsApprover, employeeDetails.IsApprover);
            Assert.AreEqual(employee.Email, employeeDetails.Email);
            Assert.AreEqual(employee.Friday, employeeDetails.Friday);
            Assert.AreEqual(employee.MaxConsecutiveDaysOff, employeeDetails.MaxConsecutiveDaysOff);
            Assert.AreEqual(employee.MaxDaysCarriedOverNextYear, employeeDetails.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(employee.Id, employeeDetails.Id);
            Assert.AreEqual(employee.Monday, employeeDetails.Monday);
            Assert.AreEqual(employee.Tuesday, employeeDetails.Tuesday);
            Assert.AreEqual(employee.Wednesday, employeeDetails.Wednesday);
            Assert.AreEqual(employee.Thursday, employeeDetails.Thursday);
            Assert.AreEqual(employee.Name, employeeDetails.Name);
            _employeesToRemove.Add(employee);
        }
       
        [OneTimeTearDown]
        public void CleanUp()
        {
            _companyService.RemoveCompany(_company.Id);
            foreach (var employee in _employeesToRemove.ToList())
            {
                _employeeService.RemoveEmployee(employee.Id);
            }
        }
    }
}
