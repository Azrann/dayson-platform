using System;
using System.Net;
using DaysOn.Facade.Common;
using DaysOn.Facade.Tests.Acceptance.Helper;
using DaysOn.HolidaysBc.Models.Dtos;
using NUnit.Framework;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class GlobalRulesControllerTest
    {
        private Guid _employeeId;
        private TokenResponse _tokenUser;
        private TokenResponse _tokenAdmin;

        [SetUp]
        public void Setup()
        {
            SetupAsync().Wait();
        }

        private async Task SetupAsync()
        {
            var companyName = NameHelper.GenerateCompanyName();
            _tokenAdmin = await CompanyFactory.CreateCompanyAsync(companyName);

            string employeeUserEmail = NameHelper.CreateEmail("employee1", companyName);
            _employeeId = await EmployeeFactory.CreateEmployeeAsync(companyName, _tokenAdmin, null, employeeUserEmail);

             Thread.Sleep(100);
            Assert.AreNotEqual(Guid.Empty, _employeeId);

            _tokenUser = AuthClientWrapper.GetTokenAsync(employeeUserEmail, _employeeId.GetHashCode().ToString()).Result;
        }

        [Test]
        public async Task WhenIRequestGlobalRulesWithAdminUser_ICanRetrieveTheGlobalRules()
        {
            var globalRules = (await GlobalRulesClientWrapper.GetCompanyGlobalRulesAsync(_tokenAdmin.access_token)).GetGlobalRuleDto();

            Assert.IsNotNull(globalRules);
            Assert.AreEqual(25, globalRules.DefaultNumberOfHolidays);
            Assert.AreEqual(365, globalRules.MaxConsecutiveDaysOff);
            Assert.AreEqual(1, globalRules.YearStartDay.Day);
            Assert.AreEqual(1, globalRules.YearStartDay.Month);
            Assert.AreEqual(true, globalRules.WorkingDaysOfTheWeek[DayOfWeek.Monday]);
            Assert.AreEqual(false, globalRules.WorkingDaysOfTheWeek[DayOfWeek.Sunday]);
        }

        [Test]
        public async Task WhenIRequestGlobalRulesWithNormalUser_IGetPermisionError()
        {
            var globalRules = (await GlobalRulesClientWrapper.GetCompanyGlobalRulesAsync(_tokenUser.access_token));

            Assert.AreEqual(HttpStatusCode.Unauthorized, globalRules.StatusCode);
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
                YearStartDay = new DateTime(2000, 2, 2),
                WorkingDaysOfTheWeek = SetWeekendWorking()
            };

            (await GlobalRulesClientWrapper.UpdateCompanyGlobalRulesAsync(newGlobalRules, _tokenAdmin.access_token)).GetGlobalRuleDto();

            var globalRules = (await GlobalRulesClientWrapper.GetCompanyGlobalRulesAsync(_tokenAdmin.access_token)).GetGlobalRuleDto();

            Assert.IsNotNull(globalRules);
            Assert.AreEqual(10, globalRules.DefaultNumberOfHolidays);
            Assert.AreEqual(1, globalRules.MaxConsecutiveDaysOff);
            Assert.AreEqual(2, globalRules.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(3, globalRules.MinDaysAdvanceNoticeHolidays);
            Assert.AreEqual(2, globalRules.YearStartDay.Day);
            Assert.AreEqual(2, globalRules.YearStartDay.Month);
            Assert.AreEqual(false, globalRules.WorkingDaysOfTheWeek[DayOfWeek.Monday]);
            Assert.AreEqual(true, globalRules.WorkingDaysOfTheWeek[DayOfWeek.Sunday]);
        }

        [TearDown]
        public void CleanUp()
        {
            
        }

        private static Dictionary<DayOfWeek, bool> SetWeekendWorking()
        {
            Dictionary<DayOfWeek, bool> result = new Dictionary<DayOfWeek, bool>();
            result.Add(DayOfWeek.Monday,false);
            result.Add(DayOfWeek.Tuesday, false);
            result.Add(DayOfWeek.Wednesday, false);
            result.Add(DayOfWeek.Thursday, false);
            result.Add(DayOfWeek.Friday, false);
            result.Add(DayOfWeek.Saturday, true);
            result.Add(DayOfWeek.Sunday, true);
            return result;
        }
    }
}
