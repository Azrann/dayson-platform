using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Events;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.Common.Domain.Ioc;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.HolidaysBc.Test.Common;
using NUnit.Framework;
using DaysOn.Common.DataAccess;

namespace DaysOn.HolidaysBc.Test.Integration.Services
{
    [TestFixture]
    public class HolidayServiceTests
    {
        private HolidayService _holidayService;
        private CompanyFactory _factory;
        private ICompanyService _companyService;
        private IEmployeeService _employeeService;
        private ICalculationService _calculationService;
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
            _holidayService = new HolidayService(new Repository<HolidayRequest>(context), new Repository<Employee>(context),new HolidayRepo(), new ApproverService(_employeeService,new EmployeesByApproverService(new DapperNoSql<EmployeesByApprover>())), new EmployeeRepo());

            _calculationService = new CalculationService(new Repository<YearlyHolidayCalculation>(context2), _employeeService);

            var handler = new UpdateCalculationsEventHandler(new HolidayService(new Repository<HolidayRequest>(context), new Repository<Employee>(context2), null, null, null), _calculationService);
            ContainerManager.Container.Register(Component.For<IEventHandler<UpdateCalculationsEvent>>().Instance(handler).LifestyleTransient());
        }

        [Test]
        public async Task HolidayServiceTest()
        {
            var employee = _factory.CreateEmployee();
            _employeeService.AddEmployee(employee);

            await _holidayService.AddHolidayRequest(_factory.CreateHolidayRequest(4), employee.Id);

            var employeeResult = _employeeService.GetEmployee(employee.Id);

            Assert.AreEqual(1, employeeResult.HolidayRequests.Count);

            _employeesToRemove.Add(employeeResult);
        }

        [Test]
        public async Task WhenIAddAHolidayRequestICanRetrieveIt()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);

            var holidayRequest = _factory.CreateHolidayRequest();

            await _holidayService.AddHolidayRequest(holidayRequest, employee.Id);

            employee = _employeeService.GetEmployee(employee.Id);

            Assert.AreEqual(1, employee.HolidayRequests.Count);
            Assert.AreEqual(holidayRequest.Id, employee.HolidayRequests[0].Id);
            Assert.AreEqual(employee.Id, holidayRequest.Employee.Id);
            Assert.AreEqual(employee.Id, holidayRequest.EmployeeId);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenIAddAHolidayRequestICanRetrieveItUsingHolidayService()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);

            var holidayRequest = _factory.CreateHolidayRequest();

            await _holidayService.AddHolidayRequest(holidayRequest, employee.Id);

            var result = await _holidayService.GetHolidayRequest(holidayRequest.Id, employee.Id);

            Assert.AreEqual(holidayRequest.Id, result.Id);
            Assert.AreEqual(employee.Id, result.Employee.Id);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenIAddMoreThanOneHolidayRequestICanRetrieveThemAllUsingHolidayService()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);

            var holidayRequest1 = _factory.CreateHolidayRequest();
            var holidayRequest2 = _factory.CreateHolidayRequest(3, 10);
            var holidayRequest3 = _factory.CreateHolidayRequest(2, 20);
            var holidayRequest4 = _factory.CreateHolidayRequest(2, 365);

            await _holidayService.AddHolidayRequest(holidayRequest1, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest2, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest3, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest4, employee.Id);

            var result = await _holidayService.GetAllHolidayRequests(employee.Id);

            Assert.AreEqual(4, result.Count);
            Assert.IsTrue(result.Any(x => x.Id == holidayRequest1.Id));
            Assert.IsTrue(result.Any(x => x.Id == holidayRequest2.Id));
            Assert.IsTrue(result.Any(x => x.Id == holidayRequest3.Id));
            Assert.IsTrue(result.Any(x => x.Id == holidayRequest4.Id));

            _employeesToRemove.Add(employee);
        }


        [Test]
        public async Task WhenIAddMoreThanOneHolidayRequestICanRetrieveThemAllByYearUsingHolidayService()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);

            var holidayRequest1 = _factory.CreateHolidayRequest();
            var holidayRequest2 = _factory.CreateHolidayRequest(3, 10);
            var holidayRequest3 = _factory.CreateHolidayRequest(2, 20);
            var holidayRequest4 = _factory.CreateHolidayRequest(2, 365);

            List<HolidayRequest> tempList = new List<HolidayRequest>()
            {
                holidayRequest1,
                holidayRequest2,
                holidayRequest3,
                holidayRequest4
            };

            await _holidayService.AddHolidayRequest(holidayRequest1, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest2, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest3, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest4, employee.Id);

            var result = await _holidayService.GetHolidaysRequestsByYear(employee.Id, holidayRequest1.Year);

            Assert.AreEqual(tempList.Count(x => x.Year == holidayRequest1.Year), result.Count);
           
            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenITryToAddHolidayRequestPassingNullAnExceptionIsThrown()
        {
            Employee employee = _factory.CreateEmployee();
            Assert.That( async () => await _holidayService.AddHolidayRequest(null, employee.Id),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Argument HolidayRequest is null."));
            
        }

        [Test]
        public async Task WhenIAddAHolidayRequestTwoTimesOnlyIsAddedOnce()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);

            var holidayRequest = _factory.CreateHolidayRequest();

            await _holidayService.AddHolidayRequest(holidayRequest, employee.Id);

            employee = _employeeService.GetEmployee(employee.Id);

            var countOnTeamMembers = employee.HolidayRequests.Count(x => x.Id == holidayRequest.Id);
            Assert.AreEqual(1, countOnTeamMembers);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenIAddAHolidayRequestCalculationsAreUpdated()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);
            var holidayRequest1 = _factory.CreateHolidayRequest(5);
            var holidayRequest2 = _factory.CreateHolidayRequest(3);
            await _holidayService.AddHolidayRequest(holidayRequest1, employee.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            var calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);

            Assert.AreEqual(20, calculations.NumberOfDaysLeft);
            await _holidayService.AddHolidayRequest(holidayRequest2, employee.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);
            Assert.AreEqual(17, calculations.NumberOfDaysLeft);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenITryToRemoveAHolidayRequestThatDoesNotExistsNothingHappens()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);
            var holidayRequest = _factory.CreateHolidayRequest();

            await _holidayService.AddHolidayRequest(holidayRequest, employee.Id);

            await _holidayService.RemoveHolidayRequest(Guid.NewGuid(), employee.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            Assert.AreEqual(1, employee.HolidayRequests.Count);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenITryToRemoveAHolidayRequestItGetsRemoved()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);
            var holidayRequest = _factory.CreateHolidayRequest();
            var holidayRequest1 = _factory.CreateHolidayRequest();

            await _holidayService.AddHolidayRequest(holidayRequest, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest1, employee.Id);

            await _holidayService.RemoveHolidayRequest(holidayRequest.Id, employee.Id);

             employee = _employeeService.GetEmployee(employee.Id);
            Assert.AreEqual(1, employee.HolidayRequests.Count);
            Assert.IsNull(holidayRequest.Employee);
            Assert.AreEqual(employee.Id, holidayRequest1.Employee.Id);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenITryToRemoveAHolidayRequestPassingNullNothingHappens()
        {
            var employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);
            var holidayRequest = _factory.CreateHolidayRequest();
            var holidayRequest2 = _factory.CreateHolidayRequest();

            await _holidayService.AddHolidayRequest(holidayRequest, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest2, employee.Id);

            await _holidayService.RemoveHolidayRequest(Guid.Empty, employee.Id);
            employee = _employeeService.GetEmployee(employee.Id);
            Assert.AreEqual(2, employee.HolidayRequests.Count);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenICancelAHolidayRequestCalculationsAreUpdated()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);
            var holidayRequest1 = _factory.CreateHolidayRequest(5);
            var holidayRequest2 = _factory.CreateHolidayRequest(3);
            await _holidayService.AddHolidayRequest(holidayRequest1, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest2, employee.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            var calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);

            Assert.AreEqual(17, calculations.NumberOfDaysLeft);

            await _holidayService.CancelHolidayRequest(holidayRequest1.Id, employee.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);
            Assert.AreEqual(22, calculations.NumberOfDaysLeft);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenIRejectAHolidayRequestCalculationsAreUpdated()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);
            var holidayRequest1 = _factory.CreateHolidayRequest(7);
            var holidayRequest2 = _factory.CreateHolidayRequest(3);
            await _holidayService.AddHolidayRequest(holidayRequest1, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest2, employee.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            var calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);

            Assert.AreEqual(15, calculations.NumberOfDaysLeft);

            await _holidayService.RejectHolidayRequest(holidayRequest1.Id, employee.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);
            Assert.AreEqual(22, calculations.NumberOfDaysLeft);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenIAproveAHolidayRequestCalculationsRemainsTheSame()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);
            var holidayRequest1 = _factory.CreateHolidayRequest(7);
            await _holidayService.AddHolidayRequest(holidayRequest1, employee.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            var calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);

            Assert.AreEqual(18, calculations.NumberOfDaysLeft);

            await _holidayService.ApprovedHolidayRequest(holidayRequest1.Id, employee.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);
            Assert.AreEqual(18, calculations.NumberOfDaysLeft);

            _employeesToRemove.Add(employee);
        }


        [Test]
        public async Task WhenIChangeStatusToApproveAHolidayRequestCalculationsRemainsTheSame()
        {
            Employee employee = _factory.CreateEmployee();
            employee.CompanyId = _company.Id;
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);
            Employee employee2 = _factory.CreateEmployee();
            employee2.CompanyId = _company.Id;
            _employeeService.AddEmployeeWithCompanyRules(employee2, _company.Id);
            var holidayRequest1 = _factory.CreateHolidayRequest(7);
            await _holidayService.AddHolidayRequest(holidayRequest1, employee.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            var calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);

            Assert.AreEqual(18, calculations.NumberOfDaysLeft);

            await _holidayService.AdminChangeHolidayStatus(holidayRequest1.Id, Status.Aproved, employee2.Id, _company.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);
            Assert.AreEqual(18, calculations.NumberOfDaysLeft);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenIChangeStatusToRejectAHolidayRequestCalculationsAreUpdated()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);
            var holidayRequest1 = _factory.CreateHolidayRequest(7);
            var holidayRequest2 = _factory.CreateHolidayRequest(3);
            await _holidayService.AddHolidayRequest(holidayRequest1, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest2, employee.Id);
            Employee employee2 = _factory.CreateEmployee();
            employee2.CompanyId = _company.Id;
            _employeeService.AddEmployeeWithCompanyRules(employee2, _company.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            var calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);

            Assert.AreEqual(15, calculations.NumberOfDaysLeft);

            await _holidayService.AdminChangeHolidayStatus(holidayRequest1.Id, Status.Rejected, employee2.Id, _company.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);
            Assert.AreEqual(22, calculations.NumberOfDaysLeft);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenIChangeStatusToCancelAHolidayRequestCalculationsAreUpdated()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);
            var holidayRequest1 = _factory.CreateHolidayRequest(7);
            var holidayRequest2 = _factory.CreateHolidayRequest(3);
            await _holidayService.AddHolidayRequest(holidayRequest1, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest2, employee.Id);
            Employee employee2 = _factory.CreateEmployee();
            employee2.CompanyId = _company.Id;
            _employeeService.AddEmployeeWithCompanyRules(employee2, _company.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            var calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);

            Assert.AreEqual(15, calculations.NumberOfDaysLeft);

            await _holidayService.AdminChangeHolidayStatus(holidayRequest1.Id, Status.Canceled, employee2.Id, _company.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);
            Assert.AreEqual(22, calculations.NumberOfDaysLeft);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public async Task WhenIChangeStatusToCancelHolidayIsUpdated()
        {
            Employee employee = _factory.CreateEmployee();
            _employeeService.AddEmployeeWithCompanyRules(employee, _company.Id);
            var holidayRequest1 = _factory.CreateHolidayRequest(7);
            var holidayRequest2 = _factory.CreateHolidayRequest(3);
            await _holidayService.AddHolidayRequest(holidayRequest1, employee.Id);
            await _holidayService.AddHolidayRequest(holidayRequest2, employee.Id);
            Employee employee2 = _factory.CreateEmployee();
            employee2.CompanyId = _company.Id;
            _employeeService.AddEmployeeWithCompanyRules(employee2, _company.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            var calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);

            Assert.AreEqual(15, calculations.NumberOfDaysLeft);

            await _holidayService.AdminChangeHolidayStatus(holidayRequest1.Id, Status.Canceled, employee2.Id, _company.Id);

            employee = _employeeService.GetEmployee(employee.Id);
            calculations = employee.GetCalculationInYear(holidayRequest1.From.Year);
            Assert.AreEqual(22, calculations.NumberOfDaysLeft);

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