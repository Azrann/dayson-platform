using System;
using DaysOn.HolidaysBc.Test.Common;
using NUnit.Framework;

namespace DaysOn.HolidaysBc.Test.Unit.Entities
{
    [TestFixture]
    public class EmployeeTests
    {
        private CompanyFactory _factory;

        [SetUp]
        public void Initialize()
        {
            _factory = new CompanyFactory();
        }

        [Test]
        public void WhenITryToApplyGlobalRulesPassingNullAnExceptionIsThrown()
        {
            var employee = _factory.CreateEmployee();
            
            Assert.That(() => employee.ApplyGlobalRules(null),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Argument GlobalRules is null."));
        }

        [Test]
        public void WhenITryToApplyGlobalRulesAllParametersAreSet()
        {
            var employee = _factory.CreateEmployee();
            var globalRules = _factory.CreateGlobalRules(Guid.NewGuid());
            employee.ApplyGlobalRules(globalRules);

            Assert.AreEqual(25, globalRules.DefaultNumberOfHolidays);
            Assert.AreEqual(10, globalRules.MaxConsecutiveDaysOff);
            Assert.AreEqual(5, globalRules.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(15, globalRules.MinDaysAdvanceNoticeHolidays);
            Assert.AreEqual(2, globalRules.YearStartDay.Day);
            Assert.AreEqual(2, globalRules.YearStartDay.Month);
            Assert.AreEqual(true, globalRules.WorkingDaysOfTheWeek[DayOfWeek.Monday]);
            Assert.AreEqual(false, globalRules.WorkingDaysOfTheWeek[DayOfWeek.Sunday]);
        }

        [Test]
        public void WhenIHaveAYearlyCalculationThenICanRetrieveIt()
        {
            var employee = _factory.CreateEmployee();
            employee.YearlyHolidayCalculations.Add(_factory.CreateYearlyHolidayCalculation());

            var result = employee.GetCalculationInYear(2000);

            Assert.IsNotNull(result);
            Assert.AreEqual(2000, result.Year);
        }

        [Test]
        public void WhenIRequesteAYearlyCalculationThatDoesNotExistsThenIGetNull()
        {
            var employee = _factory.CreateEmployee();
            employee.YearlyHolidayCalculations.Add(_factory.CreateYearlyHolidayCalculation());

            var result = employee.GetCalculationInYear(2001);

            Assert.IsNull(result);
        }

        [Test]
        public void WhenIRequesteAYearlyCalculationAndListIsEmptyThenIGetNull()
        {
            var employee = _factory.CreateEmployee();

            var result = employee.GetCalculationInYear(2001);

            Assert.IsNull(result);
        }

        [Test]
        public void WhenICreateAYearlyCalculationThenICanRetrieveIt()
        {
            var employee = _factory.CreateEmployee();
            employee.CreateCalculationInYear(2000);

            var result = employee.GetCalculationInYear(2000);

            Assert.IsNotNull(result);
            Assert.AreEqual(2000, result.Year);
        }

        [Test]
        public void WhenICreateAYearlyCalculationThenDaysLeftAndMaxDaysAreSetUsingEmployeeHolidayEntitlement()
        {
            var employee = _factory.CreateEmployee();
            employee.CreateCalculationInYear(2000);

            var result = employee.GetCalculationInYear(2000);

            Assert.IsNotNull(result);
            Assert.AreEqual(employee.HolidayEntitlement, result.NumberOfDays);
            Assert.AreEqual(employee.HolidayEntitlement, result.NumberOfDaysLeft);
        }

        [Test]
        public void WhenICreateAYearlyCalculationThenIGetTheCalculationAsAResponse()
        {
            var employee = _factory.CreateEmployee();
            var result = employee.CreateCalculationInYear(2000);

            var result2 = employee.GetCalculationInYear(2000);

            Assert.IsNotNull(result);
            Assert.AreEqual(result2, result);
        }
    }
}