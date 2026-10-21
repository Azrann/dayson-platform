using System;
using System.Collections.Generic;
using System.Linq;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using NUnit.Framework;
using DaysOn.Common.DataAccess;
using DaysOn.Common.SqlDataAccess.Repositories;

namespace DaysOn.HolidaysBc.Test.Integration.Services
{
    [TestFixture]
    public class EmployeesByApproverServiceTests
    {
        private IEmployeesByApproverService _employeesByApproverService;
        private List<Guid> _approversToBeRemoved;
        private Repository<Employee> _employeeRepo;

        [OneTimeSetUp]
        public void Initialize()
        {
            _employeeRepo = new Repository<Employee>(new HolidaysContext());
            DapperNoSql<EmployeesByApprover> repo = new DapperNoSql<EmployeesByApprover>();
            repo.CreateTableIfNotExists(NoSqlTables.EmployeesByApprover);
            _employeesByApproverService = new EmployeesByApproverService(repo);
            _approversToBeRemoved= new List<Guid>();
        }

        [Test]
        public void WhenIAddAnEmployeeToAnApprover_ThenICanRetrieveIt()
        {
            Guid approverId = Guid.NewGuid();
            _approversToBeRemoved.Add(approverId);
            Guid employeeId1 = Guid.NewGuid();
            Guid employeeId2 = Guid.NewGuid();

            _employeesByApproverService.AddEmployeeToApprover(approverId, employeeId1, "Name1");
            _employeesByApproverService.AddEmployeeToApprover(approverId, employeeId2, "Name2");

            var result = _employeesByApproverService.GetEmployeesForApprover(approverId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Employees.Any(x => x.Id == employeeId1 && x.Name == "Name1"));
            Assert.IsTrue(result.Employees.Any(x => x.Id == employeeId2 && x.Name == "Name2"));
            Assert.AreEqual(2, result.Employees.Count);
        }

        [Test]
        public void WhenIRemoveAnEmployeeFromAnApprover_ThenICannotRetrieveIt()
        {

            Guid approverId = Guid.NewGuid();
            _approversToBeRemoved.Add(approverId);
            Guid employeeId1 = Guid.NewGuid();
            Guid employeeId2 = Guid.NewGuid();

            _employeesByApproverService.AddEmployeeToApprover(approverId, employeeId1, "Name1");
            _employeesByApproverService.AddEmployeeToApprover(approverId, employeeId2, "Name2");

            _employeesByApproverService.RemoveEmployeeFromApprover(approverId, employeeId1);

            var result = _employeesByApproverService.GetEmployeesForApprover(approverId);
            var employee1Result = _employeeRepo.GetAll(x => x.Id == employeeId1).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Employees.Any(x => x.Id == employeeId1 && x.Name == "Name1"));
            Assert.IsTrue(result.Employees.Any(x => x.Id == employeeId2 && x.Name == "Name2"));
            Assert.AreEqual(1, result.Employees.Count);
        }

        [Test]
        public void WhenIRemoveAnApprover_ThenListIsEmpty()
        {
            Guid approverId = Guid.NewGuid();
            _approversToBeRemoved.Add(approverId);
            Guid employeeId1 = Guid.NewGuid();
            Guid employeeId2 = Guid.NewGuid();

            _employeesByApproverService.AddEmployeeToApprover(approverId, employeeId1, "Name1");
            _employeesByApproverService.AddEmployeeToApprover(approverId, employeeId2, "Name2");

            _employeesByApproverService.RemoveApprover(approverId);

            var result = _employeesByApproverService.GetEmployeesForApprover(approverId);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Employees.Any(x => x.Id == employeeId2 && x.Name == "Name2"));
            Assert.AreEqual(0, result.Employees.Count);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            foreach (var approverId in _approversToBeRemoved)
            {
                _employeesByApproverService.RemoveApprover(approverId);
            }
        }
    }
}