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
    public class HolidayRepoTest
    {
        private IHolidayRepo _repo;
        private CompanyFactory _factory;
        private ICompanyService _companyService;
        private IEmployeeService _employeeService;
        private List<Employee> _employeesToRemove;
        private Company _company;

        [OneTimeSetUp]
        public void Initialize()
        {
            _repo = new HolidayRepo();
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
            _repo.AddHoliday(id, DateTime.Now, DateTime.Now.AddDays(3), Status.Requested,3,"Holidays", employeeId, 2018);

            var request = _repo.GetHoliday(id);

            _employeesToRemove.Add(employee);
            Assert.AreEqual(id, request.Id);
            Assert.AreEqual("Requested", request.Status.ToString());
            Assert.AreEqual("Holidays", request.Notes);
            Assert.AreEqual(3, request.NumberOfDays);
            Assert.AreEqual(2018, request.Year);
        }

        [Test]
        public void WhenIChangeTheStatusIGetTheRightValue()
        {
            var employee = _factory.CreateEmployee();
            _employeeService.AddEmployee(employee);
            Guid employeeId = employee.Id;
            Guid id = Guid.NewGuid();
            _repo.AddHoliday(id, DateTime.Now, DateTime.Now.AddDays(3), Status.Requested, 3, "Holidays", employeeId, 2018);
            _repo.ChangeStatus(id, Status.Aproved);
            var request = _repo.GetHoliday(id);

            _employeesToRemove.Add(employee);
            Assert.AreEqual(id, request.Id);
            Assert.AreEqual("Aproved", request.Status.ToString());
            Assert.AreEqual("Holidays", request.Notes);
            Assert.AreEqual(3, request.NumberOfDays);
            Assert.AreEqual(2018, request.Year);
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
