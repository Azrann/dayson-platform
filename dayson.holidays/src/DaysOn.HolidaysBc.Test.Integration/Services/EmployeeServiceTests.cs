using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EmployeeServiceTests
    {
        private CompanyFactory _factory;
        private ICompanyService _companyService;
        private IEmployeeService _employeeService;
        private List<Employee> _employeesToRemove;
        private Company _company;

        [OneTimeSetUp]
        public void Initialize()
        {
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
        public void WhenIAddAnEmployee_ThenICanRetrieveIt()
        {
            var employee = _factory.CreateEmployee();
            _employeeService.AddEmployee(employee);
            
            var result = _employeeService.GetEmployee(employee.Id);
            Assert.AreEqual(employee.MaxDaysCarriedOverNextYear, result.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(employee.Email, result.Email);
            Assert.AreEqual(employee.HolidayEntitlement, result.HolidayEntitlement);
            Assert.AreEqual(employee.HolidayRequests, result.HolidayRequests);
            Assert.AreEqual(employee.MaxConsecutiveDaysOff, result.MaxConsecutiveDaysOff);
            Assert.AreEqual(employee.MinDaysAdvanceNoticeHolidays, result.MinDaysAdvanceNoticeHolidays);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public void WhenIAddAnEmployeeWithCompany_ThenIGlobalSettingsAreUpdated()
        {
            var employee = _factory.CreateEmployee();

            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);

            var result = _employeeService.GetEmployee(employee.Id);
            Assert.AreEqual(5, result.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(25, result.HolidayEntitlement);
            Assert.AreEqual(10, result.MaxConsecutiveDaysOff);
            Assert.AreEqual(15, result.MinDaysAdvanceNoticeHolidays);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public void WhenIUpdateGlobalSettingsForAnEmployee_ThenGlobalSettingsAreUpdated()
        {
            var employee = _factory.CreateEmployee();

            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);

            _employeeService.SetGlobalRules(employee.Id, employee.CompanyId, new GlobalRules(Guid.NewGuid(), _company.Id)
            {
                MaxDaysCarriedOverNextYear = 3,
                DefaultNumberOfHolidays = 30,
                MaxConsecutiveDaysOff =15,
                MinDaysAdvanceNoticeHolidays = 5
            });

            var result = _employeeService.GetEmployee(employee.Id);
            Assert.AreEqual(3, result.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(30, result.HolidayEntitlement);
            Assert.AreEqual(15, result.MaxConsecutiveDaysOff);
            Assert.AreEqual(5, result.MinDaysAdvanceNoticeHolidays);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public void WhenIRemoveAnEmployee_ThenICannotRetrieveIt()
        {
            var employee = _factory.CreateEmployee();
            employee.Id = new Guid("c2823996-f7f5-4f18-a297-901b176dff75");
            _employeeService.AddEmployee(employee);

            var result = _employeeService.GetEmployee(employee.Id);
            Assert.IsNotNull(result);
            _employeeService.RemoveEmployee(employee.Id);
            
            Assert.That(() => _employeeService.GetEmployee(employee.Id),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Employee with Id c2823996-f7f5-4f18-a297-901b176dff75 not found."));
        }

        [Test]
        public void WhenIGetTheCalculationsForHolidays_ThenICanRetrieveThem()
        {
            var employee = _factory.CreateEmployee();
            _employeeService.AddEmployee(employee);
            _employeeService.CreateCalculationsForEmployeeInYear(employee.Id, 2000);
            _employeeService.CreateCalculationsForEmployeeInYear(employee.Id, 2001);

            var result = _employeeService.GetEmployee(employee.Id);
            
            Assert.AreEqual(2, result.YearlyHolidayCalculations.Count);

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