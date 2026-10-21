using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.Facade.Common;
using DaysOn.Facade.Tests.Acceptance.Helper;
using DaysOn.HolidaysBc.Models.Dtos;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading;
using System.Collections.Generic;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class FacadeGlobalRulesControllerTest
    {
        private TokenResponse _tokenAdmin;
        private Guid _employeeId;
        private string _employeeEmail;

        [SetUp]
        public void Setup()
        {
            SetupAsync().Wait();
        }

        private async Task SetupAsync()
        {
            var companyName = NameHelper.GenerateCompanyName();
            _tokenAdmin = await CompanyFactory.CreateCompanyAsync(companyName);

            _employeeEmail = EmployeeFactory.CreateEmployee(companyName, _tokenAdmin,out _employeeId, "User");
            Thread.Sleep(150);
        }

        [Test]
        public async Task WhenIRequestGlobalRulesWithAdminUser_ICanRetrieveTheGlobalRules()
        {
            var globalRules = (await FacadeClientWrapper.GetCompanyGlobalRulesAsync(_tokenAdmin.access_token)).GetGlobalRuleDto();

            Assert.IsNotNull(globalRules);
            Assert.AreEqual(25, globalRules.DefaultNumberOfHolidays);
            Assert.AreEqual("gb", globalRules.CountryId);
        }

        [Test]
        public async Task WhenIUpdateGlobalRulesWithAdminUser_ICanRetrieveTheGlobalRules()
        {
            var newGlobalRules = new GlobalRulesDto
            {
                DefaultNumberOfHolidays = 10,
                MaxConsecutiveDaysOff = 1,
                MaxDaysCarriedOverNextYear = 2,
                MinDaysAdvanceNoticeHolidays = 3,
                YearStartDay = new DateTime(2000,3,3),
                WorkingDaysOfTheWeek = Factory.GetWorkingDays(),
                CountryId = "es",
                OpenDuringBankHolidays = true
            };

            (await FacadeClientWrapper.UpdateCompanyGlobalRulesAsync(newGlobalRules, _tokenAdmin.access_token)).GetGlobalRuleDto();

            var globalRules = (await FacadeClientWrapper.GetCompanyGlobalRulesAsync(_tokenAdmin.access_token)).GetGlobalRuleDto();

            Assert.IsNotNull(globalRules);
            Assert.AreEqual(10, globalRules.DefaultNumberOfHolidays);
            Assert.AreEqual(1, globalRules.MaxConsecutiveDaysOff);
            Assert.AreEqual(2, globalRules.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(3, globalRules.MinDaysAdvanceNoticeHolidays);
            Assert.AreEqual(3, globalRules.YearStartDay.Day);
            Assert.AreEqual(3, globalRules.YearStartDay.Month);
            Assert.IsTrue(globalRules.WorkingDaysOfTheWeek[DayOfWeek.Monday]);
            Assert.IsTrue(globalRules.WorkingDaysOfTheWeek[DayOfWeek.Tuesday]);
            Assert.IsTrue(globalRules.WorkingDaysOfTheWeek[DayOfWeek.Wednesday]);
            Assert.IsTrue(globalRules.WorkingDaysOfTheWeek[DayOfWeek.Thursday]);
            Assert.IsTrue(globalRules.WorkingDaysOfTheWeek[DayOfWeek.Friday]);
            Assert.IsFalse(globalRules.WorkingDaysOfTheWeek[DayOfWeek.Saturday]);
            Assert.IsFalse(globalRules.WorkingDaysOfTheWeek[DayOfWeek.Sunday]);
            Assert.AreEqual("es", globalRules.CountryId);
            Assert.IsTrue(globalRules.OpenDuringBankHolidays);
        }

        [Test]
        public async Task WhenIUpdateEmployeeGlobalRules_ThenICanRetrieveThem()
        {
            var employeeNewGlobalRules = new GlobalRulesDto()
            {
                MaxDaysCarriedOverNextYear = 3,
                DefaultNumberOfHolidays = 30,
                MaxConsecutiveDaysOff = 15,
                MinDaysAdvanceNoticeHolidays = 5,
                YearStartDay = new DateTime(2000, 3, 3),
                WorkingDaysOfTheWeek = Factory.GetWorkingDays(),
            };

            var response = (await FacadeClientWrapper.SetEmployeeGlobalRules(_employeeId, employeeNewGlobalRules, _tokenAdmin.access_token));

            var employeeGlobalRules = (await FacadeClientWrapper.GetEmployeeGlobalRules(_employeeId, _tokenAdmin.access_token)).GetEmployeeGlobalRules();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(3, employeeGlobalRules.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(30, employeeGlobalRules.HolidayEntitlement);
            Assert.AreEqual(15, employeeGlobalRules.MaxConsecutiveDaysOff);
            Assert.AreEqual(5, employeeGlobalRules.MinDaysAdvanceNoticeHolidays);
            Assert.AreEqual(3, employeeGlobalRules.YearStartDay.Day);
            Assert.AreEqual(3, employeeGlobalRules.YearStartDay.Month);
            Assert.IsTrue(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Monday]);
            Assert.IsTrue(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Tuesday]);
            Assert.IsTrue(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Wednesday]);
            Assert.IsTrue(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Thursday]);
            Assert.IsTrue(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Friday]);
            Assert.IsFalse(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Saturday]);
            Assert.IsFalse(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Sunday]);
        }

        [Test]
        public async Task ICanGetMyGlobalRules()
        {
            var tokenUser = await AuthClientWrapper.GetTokenAsync(_employeeEmail, _employeeId.GetHashCode().ToString());

            var employeeNewGlobalRules = new GlobalRulesDto()
            {
                MaxDaysCarriedOverNextYear = 3,
                DefaultNumberOfHolidays = 30,
                MaxConsecutiveDaysOff = 15,
                MinDaysAdvanceNoticeHolidays = 5,
                YearStartDay = new DateTime(2000, 3, 3),
                WorkingDaysOfTheWeek = Factory.GetWorkingDays(),
            };

            var response = (await FacadeClientWrapper.SetEmployeeGlobalRules(_employeeId, employeeNewGlobalRules, _tokenAdmin.access_token));

            var employeeGlobalRules = (await FacadeClientWrapper.GetMyGlobalRules(tokenUser.access_token)).GetEmployeeGlobalRules();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(3, employeeGlobalRules.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(30, employeeGlobalRules.HolidayEntitlement);
            Assert.AreEqual(15, employeeGlobalRules.MaxConsecutiveDaysOff);
            Assert.AreEqual(5, employeeGlobalRules.MinDaysAdvanceNoticeHolidays);
            Assert.AreEqual(3, employeeGlobalRules.YearStartDay.Day);
            Assert.AreEqual(3, employeeGlobalRules.YearStartDay.Month);
            Assert.IsTrue(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Monday]);
            Assert.IsTrue(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Tuesday]);
            Assert.IsTrue(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Wednesday]);
            Assert.IsTrue(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Thursday]);
            Assert.IsTrue(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Friday]);
            Assert.IsFalse(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Saturday]);
            Assert.IsFalse(employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Sunday]);
        }
        
    }
}
