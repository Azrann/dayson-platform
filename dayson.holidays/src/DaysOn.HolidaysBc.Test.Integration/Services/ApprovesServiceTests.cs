using System;
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
    public class ApprovesServiceTests
    {
        private IApproverService _approverService;
        private IEmployeeService _employeeService;
        
        [SetUp]
        public void Initialize()
        {
            Repository<Employee> employeeRepositorio = new Repository<Employee>(new HolidaysContext());
            DapperNoSql<EmployeesByApprover> employeeByApproverNoSql = new DapperNoSql<EmployeesByApprover>();
            DapperNoSql<OpenClosedDates> openClosedDatesNoSql = new DapperNoSql<OpenClosedDates>();

            employeeByApproverNoSql.CreateTableIfNotExists(NoSqlTables.EmployeesByApprover);
            IEmployeesByApproverService employeeByApproverService = new EmployeesByApproverService(employeeByApproverNoSql);
            ICompanyService companyService = new CompanyService(new Repository<Company>(new HolidaysContext()), openClosedDatesNoSql);
            _employeeService = new EmployeeService(employeeRepositorio, companyService);

            _approverService = new ApproverService(_employeeService, employeeByApproverService);
        }

        [Test]
        public void WhenIAddAnEmployeeToAnApprover_ThenICanRetrieveIt()
        {
            Guid companyId = Guid.NewGuid();

            Guid employeeId1 = CreateEmployee("Name1",companyId);
            Guid employeeId2 = CreateEmployee("Name2", companyId);
            Guid employeeId3 = CreateEmployee("Name3", companyId);
            Guid approverId = CreateEmployee("Approver1", companyId);
            
            _approverService.AddApproverToAnEmployee(companyId, approverId, employeeId1);
            _approverService.AddApproverToAnEmployee(companyId, approverId, employeeId2);
            _approverService.AddApproverToAnEmployee(companyId, approverId, employeeId3);

            var result = _approverService.GetEmployeesForApprover(companyId, approverId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Employees.Any(x => x.Id == employeeId1 && x.Name == "Name1"));
            Assert.IsTrue(result.Employees.Any(x => x.Id == employeeId2 && x.Name == "Name2"));
            Assert.IsTrue(result.Employees.Any(x => x.Id == employeeId3 && x.Name == "Name3"));

            Assert.AreEqual(3, result.Employees.Count);
        }

        [Test]
        public void WhenIRetrieveAnEmployee_ThenICanSeeItsApprover()
        {
            Guid companyId = Guid.NewGuid();

            Guid employeeId1 = CreateEmployee("Name1", companyId);
            Guid approverId = CreateEmployee("Approver1", companyId);

            _approverService.AddApproverToAnEmployee(companyId, approverId, employeeId1);

            var result = _employeeService.GetEmployeeWith(employeeId1, "Approver");

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Approver);
            Assert.AreEqual("Approver1", result.Approver.Name);
            Assert.AreEqual(approverId, result.Approver.Id);
        }

        [Test]
        public void WhenIRemoveAnEmployeeFromAnApprover_ThenICannotRetrieveIt()
        {
            Guid companyId = Guid.NewGuid();

            Guid employeeId1 = CreateEmployee("Name1", companyId);
            Guid employeeId2 = CreateEmployee("Name2", companyId);
            Guid employeeId3 = CreateEmployee("Name3", companyId);
            Guid approverId = CreateEmployee("Approver1", companyId);

            _approverService.AddApproverToAnEmployee(companyId, approverId, employeeId1);
            _approverService.AddApproverToAnEmployee(companyId, approverId, employeeId2);
            _approverService.AddApproverToAnEmployee(companyId, approverId, employeeId3);

            _approverService.RemoveApproverToAnEmployee(companyId, employeeId1);

            var result = _approverService.GetEmployeesForApprover(companyId, approverId);
            var employee1 = _employeeService.GetEmployeeWith(employeeId1, "Approver");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Employees.Any(x => x.Id == employeeId2 && x.Name == "Name2"));
            Assert.IsTrue(result.Employees.Any(x => x.Id == employeeId3 && x.Name == "Name3"));
            Assert.IsNull(employee1.Approver);
            Assert.AreEqual(2, result.Employees.Count);
        }

        [Test]
        public void WhenIRemoveAnApprover_ThenListIsEmpty()
        {
            Guid companyId = Guid.NewGuid();

            Guid employeeId1 = CreateEmployee("Name1", companyId);
            Guid employeeId2 = CreateEmployee("Name2", companyId);
            Guid employeeId3 = CreateEmployee("Name3", companyId);
            Guid approverId = CreateEmployee("Approver1", companyId);

            _approverService.AddApprover(companyId, approverId);
            _approverService.AddApproverToAnEmployee(companyId, approverId, employeeId1);
            _approverService.AddApproverToAnEmployee(companyId, approverId, employeeId2);
            _approverService.AddApproverToAnEmployee(companyId, approverId, employeeId3);

            _approverService.RemoveApprover(companyId, approverId);
            var approverafter = _employeeService.GetEmployee(approverId);

            var employee1 = _employeeService.GetEmployeeWith(employeeId1, "Approver");
            var employee2 = _employeeService.GetEmployeeWith(employeeId2, "Approver");
            var employee3 = _employeeService.GetEmployeeWith(employeeId3, "Approver");

            var result = _approverService.GetEmployeesForApprover(companyId, approverId);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Employees.Count);
            Assert.IsNull(employee1.Approver);
            Assert.IsNull(employee2.Approver);
            Assert.IsNull(employee3.Approver);
            Assert.IsFalse(approverafter.IsApprover);
        }

        [Test]
        public void ICanAddANewApprover()
        {
            Guid companyId = Guid.NewGuid();
            Guid approverId = CreateEmployee("Approver1", companyId);

            _approverService.AddApprover(companyId, approverId);
            var approver = _employeeService.GetEmployee(approverId);

            Assert.IsTrue(approver.IsApprover);
        }

        [Test]
        public void WhenIAddAnApproverAndTheEmployeeAlreadyHadOne_ThePreviousApproverGetDeleted()
        {
            Guid companyId = Guid.NewGuid();

            Guid employeeId1 = CreateEmployee("Name1", companyId);
            Guid approverId = CreateEmployee("Approver1", companyId);
            Guid approverId2 = CreateEmployee("Approver2", companyId);

            _approverService.AddApprover(companyId, approverId);
            _approverService.AddApprover(companyId, approverId2);

            _approverService.AddApproverToAnEmployee(companyId, approverId, employeeId1);
            _approverService.AddApproverToAnEmployee(companyId, approverId2, employeeId1);

            var employee1 = _employeeService.GetEmployeeWith(employeeId1, "Approver");
           
            var result1 = _approverService.GetEmployeesForApprover(companyId, approverId);
            var result2 = _approverService.GetEmployeesForApprover(companyId, approverId2);

            Assert.IsNotNull(employee1.Approver);
            Assert.AreEqual(approverId2, employee1.Approver.Id);
            Assert.AreEqual(0, result1.Employees.Count);
            Assert.AreEqual(1, result2.Employees.Count);
            Assert.AreEqual(employeeId1, result2.Employees[0].Id);
        }

        private Guid CreateEmployee(string name, Guid companyId)
        {
            Guid employeeId = Guid.NewGuid();
            _employeeService.AddEmployee(new Employee(employeeId)
            {
                CompanyId = companyId,
                Name = name,
                CreatedOn = DateTime.Now,
                LastUpdated = DateTime.Now,
                YearStartDay = DateTime.Now,
            });

            return employeeId;
        }
    }
}