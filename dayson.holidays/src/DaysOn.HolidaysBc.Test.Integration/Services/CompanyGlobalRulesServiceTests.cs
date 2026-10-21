using NUnit.Framework;
using System;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Infrastructure;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.HolidaysBc.Test.Common;
using DaysOn.Common.DataAccess;

namespace DaysOn.HolidaysBc.BoundedContext.Services.Tests
{
    [TestFixture]
    public class CompanyGlobalRulesServiceTests
    {
        private CompanyFactory _factory;
        private ICompanyService _companyService;
        private ICompanyGlobalRulesService _companyGlobalRulesService;
        private Company _company;

        [OneTimeSetUp]
        public void Initialize()
        {
            _factory = new CompanyFactory();
            var context = new HolidaysContext();
            var context2= new HolidaysContext();

            _companyService = new CompanyService(new Repository<Company>(context), new DapperNoSql<OpenClosedDates>());
            _companyGlobalRulesService = new CompanyGlobalRulesService(new Repository<GlobalRules>(context2));
            _company = _factory.CreateCompany();
            _companyService.AddCompany(_company);
        }

        [Test]
        public void WhenICreateGlobalRulesICanRetrieveIt()
        {
            var result = _companyGlobalRulesService.GetGlobalRules(_company.Id);
            Assert.IsNotNull(result);
        }

        [Test]
        public void WhenIUpdateGlobalRulesICanRetrieveThem()
        {
            var globalRules = _factory.CreateGlobalRules(_company.Id);
            globalRules.DefaultNumberOfHolidays = 0;
            globalRules.MaxConsecutiveDaysOff = 1;
            globalRules.MaxDaysCarriedOverNextYear = 2;
            globalRules.MinDaysAdvanceNoticeHolidays = 3;
            globalRules.SetWorkingDay(DayOfWeek.Sunday);
            globalRules.YearStartDay = new DateTime(2000, 2, 2);

            _companyGlobalRulesService.UpdateGlobalRules(_company.Id, globalRules);

            var result = _companyGlobalRulesService.GetGlobalRules(_company.Id);

            Assert.AreEqual(0, result.DefaultNumberOfHolidays);
            Assert.AreEqual(1, result.MaxConsecutiveDaysOff);
            Assert.AreEqual(2, result.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(3, result.MinDaysAdvanceNoticeHolidays);
            Assert.AreEqual(true, result.WorkingDaysOfTheWeek[DayOfWeek.Sunday]);
            Assert.AreEqual(2, result.YearStartDay.Day);
            Assert.AreEqual(2, result.YearStartDay.Month);
        }

        [Test]
        public void WhenITryToUpdateAGlobalRulesForACompanyThatDoesNotExistsIWeCreateADefaultRule()
        {
            var globalRules = _factory.CreateGlobalRules(_company.Id);
            _companyGlobalRulesService.UpdateGlobalRules(Guid.NewGuid(), globalRules);
        }

        [Test]
        public void WhenITryToUpdateAGlobalRulesForACompanyThatExistsWithANullObjectIGetException()
        {
            var globalRules = _factory.CreateGlobalRules(_company.Id);

            Assert.That(() =>
                _companyGlobalRulesService.UpdateGlobalRules(_company.Id, null),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo(Error.GlobalRulesIsNull));
        }

        [Test]
        public void WhenITryToGetAGlobalRulesForACompanyThatDoesNotExistsIGetException()
        {
            var result = _companyGlobalRulesService.GetGlobalRules(new Guid("76136a0c-4816-42de-842d-eec53ed70bc9"));

            Assert.AreEqual(25, result.DefaultNumberOfHolidays);
            Assert.AreEqual(365, result.MaxConsecutiveDaysOff);
            Assert.AreEqual(5, result.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(0, result.MinDaysAdvanceNoticeHolidays);
            Assert.AreEqual(false, result.WorkingDaysOfTheWeek[DayOfWeek.Sunday]);
            Assert.AreEqual(false, result.WorkingDaysOfTheWeek[DayOfWeek.Saturday]);
            Assert.AreEqual(true, result.WorkingDaysOfTheWeek[DayOfWeek.Monday]);
            Assert.AreEqual(true, result.WorkingDaysOfTheWeek[DayOfWeek.Tuesday]);
            Assert.AreEqual(true, result.WorkingDaysOfTheWeek[DayOfWeek.Wednesday]);
            Assert.AreEqual(true, result.WorkingDaysOfTheWeek[DayOfWeek.Thursday]);
            Assert.AreEqual(true, result.WorkingDaysOfTheWeek[DayOfWeek.Friday]);
            Assert.AreEqual(1, result.YearStartDay.Day);
            Assert.AreEqual(1, result.YearStartDay.Month);
        }
    }
}