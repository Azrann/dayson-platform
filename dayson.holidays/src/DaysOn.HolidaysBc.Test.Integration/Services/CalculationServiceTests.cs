using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Events;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.HolidaysBc.Test.Common;
using NUnit.Framework;
using DaysOn.Common.DataAccess;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.Common.Domain.Ioc;

namespace DaysOn.HolidaysBc.Test.Integration.Services
{
    [TestFixture]
    public class CalculationServiceTests
    {
        private CompanyFactory _factory;
        private ICompanyService _companyService;
        private IEmployeeService _employeeService;
        private List<Employee> _employeesToRemove;
        private ICalculationService _calculationService;
        private Company _company;

        [OneTimeSetUp]
        public void Initialize()
        {
            _employeesToRemove = new List<Employee>();
            _factory = new CompanyFactory();
            var context = new HolidaysContext();
            var context2 = new HolidaysContext();
            var context3 = new HolidaysContext();

            _companyService = new CompanyService(new Repository<Company>(context), new DapperNoSql<OpenClosedDates>());
            _employeeService = new EmployeeService(new Repository<Employee>(context2), _companyService);

            _company = _factory.CreateCompany();
            _companyService.AddCompany(_company);

            _calculationService = new CalculationService(new Repository<YearlyHolidayCalculation>(context3), _employeeService);

            var handler = new UpdateDaysCarryOverEventHandler(_calculationService);
            ContainerManager.Container.Register(Component.For<IEventHandler<UpdateDaysCarryOverEvent>>().Instance(handler).LifestyleTransient());
        }

        [Test]
        public void WhenIGetCalculationsByYearAndDoesntExists_IGetNull()
        {
            var employee = _factory.CreateEmployee();
            _employeeService.AddEmployee(employee);

            var calculationResult = _calculationService.GetCalculationsByYear(employee.Id, 2015);

            Assert.IsNull(calculationResult);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public void WhenIGetCalculationsByYearSafetlyAndDoesntExists_ICreateThemAndReturn()
        {
            var employee = _factory.CreateEmployee();
            _employeeService.AddEmployee(employee);

            var calculationResult = _calculationService.GetCalculationsByYearCreateIfNotExists(employee.Id, 2015);

            Assert.AreEqual(24, calculationResult.NumberOfDaysLeft);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public void WhenIUpdateNumberOfDaysAndGetCalculationsByYear_IGetTheResults()
        {
            var employee = _factory.CreateEmployee();
            _employeeService.AddEmployee(employee);

            _calculationService.UpdateNumberOfDaysLeft(employee.Id, 2015, 15);
            var calculationResult = _calculationService.GetCalculationsByYear(employee.Id, 2015);

            Assert.AreEqual(9, calculationResult.NumberOfDaysLeft);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public void WhenICarryOverHolidays_IGetTheCalculationsForBothYears()
        {
            var employee = _factory.CreateEmployee();
            employee.MaxDaysCarriedOverNextYear = 3;
            _employeeService.AddEmployee(employee);

            _calculationService.UpdateNumberOfDaysLeft(employee.Id, 2015, 15);
            _calculationService.CarryOverDays(employee.Id, 2015, 3);
            var calculationResult2015 = _calculationService.GetCalculationsByYear(employee.Id, 2015);
            var calculationResult2016 = _calculationService.GetCalculationsByYear(employee.Id, 2016);

            Assert.AreEqual(6, calculationResult2015.NumberOfDaysLeft);
            Assert.AreEqual(27, calculationResult2016.NumberOfDaysLeft);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public void WhenICarryOverMoreHolidaysThanIAmAllowed_IGetException()
        {
            var employee = _factory.CreateEmployee();
            employee.MaxDaysCarriedOverNextYear = 2;
            _employeeService.AddEmployee(employee);

            _calculationService.UpdateNumberOfDaysLeft(employee.Id, 2015, 15);
            Assert.That(() => _calculationService.CarryOverDays(employee.Id, 2015, 3),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Maximum number of days to carry forward are 2"));

            _employeesToRemove.Add(employee);
        }

        [Test]
        public void WhenISetDaysCarriedForward_ICanRetrieveNumDaysLeftUpdated()
        {
            var employee = _factory.CreateEmployee();
            employee.MaxDaysCarriedOverNextYear = 3;
            _employeeService.AddEmployee(employee);

            _calculationService.UpdateNumberOfDaysLeft(employee.Id, 2015, 15);
            var calculationResulta = _calculationService.GetCalculationsByYear(employee.Id, 2015).NumberOfDaysLeft;
            _calculationService.SetDaysCarriedForwardFromPreviousYear(employee.Id, 2015, 3);
            var calculationResultb = _calculationService.GetCalculationsByYear(employee.Id, 2015).NumberOfDaysLeft;

            Assert.AreEqual(9, calculationResulta);
            Assert.AreEqual(12, calculationResultb);

            _employeesToRemove.Add(employee);
        }

        [Test]
        public void WhenISetCorrection_ICanRetrieveNumDaysLeftUpdated()
        {
            var employee = _factory.CreateEmployee();
            employee.MaxDaysCarriedOverNextYear = 3;
            _employeeService.AddEmployee(employee);

            _calculationService.UpdateNumberOfDaysLeft(employee.Id, 2015, 15);
            var calculationResulta = _calculationService.GetCalculationsByYear(employee.Id, 2015).NumberOfDaysLeft;
            _calculationService.SetCorrection(employee.Id, 2015, 3);
            var calculationResultb = _calculationService.GetCalculationsByYear(employee.Id, 2015).NumberOfDaysLeft;

            Assert.AreEqual(9, calculationResulta);
            Assert.AreEqual(12, calculationResultb);

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