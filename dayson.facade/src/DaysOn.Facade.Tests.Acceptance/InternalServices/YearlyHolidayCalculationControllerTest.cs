using System;
using DaysOn.CompanyBc.Models.Commands;
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
    public class YearlyHolidayCalculationControllerTest
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
            var companyId = Guid.NewGuid();
            var companyName = NameHelper.GenerateCompanyName();
            _tokenAdmin = await CompanyFactory.CreateCompanyAsync(companyName, companyId);

            var newGlobalRules = new GlobalRulesDto
            {
                CompanyId = companyId,
                DefaultNumberOfHolidays = 15,
                MaxConsecutiveDaysOff = 1,
                MaxDaysCarriedOverNextYear = 5,
                MinDaysAdvanceNoticeHolidays = 3,
                YearStartDay = new DateTime(2000, 3, 3),
                WorkingDaysOfTheWeek = GetWorkingDays()
            };

            var result = (GlobalRulesClientWrapper.UpdateCompanyGlobalRulesAsync(newGlobalRules, _tokenAdmin.access_token)).Result.GetGlobalRuleDto();

            Thread.Sleep(100);
            string employeeUserEmail = NameHelper.CreateEmail("employee1", companyName);
            _employeeId = await EmployeeFactory.CreateEmployeeAsync(companyName, _tokenAdmin, null, employeeUserEmail);
            Thread.Sleep(200);
            Assert.AreNotEqual(Guid.Empty, _employeeId);

            _tokenUser = AuthClientWrapper.GetTokenAsync(employeeUserEmail, _employeeId.GetHashCode().ToString()).Result;
        }

        [Test]
        public async Task WhenIRequestYearlyHolidayCalculationsWithEmployee_ICanRetrieveThem()
        {
            var YearlyHolidayCalculation = (await FacadeClientWrapper.GetYearlyHolidayCalculationByEmployee(2020, _tokenUser.access_token)).
                GetYearlyHolidayCalculationDto();

            Assert.IsNotNull(YearlyHolidayCalculation);
            Assert.AreEqual(2020,YearlyHolidayCalculation.Year);
            Assert.AreEqual(15, YearlyHolidayCalculation.NumberOfDays);
            Assert.AreEqual(15, YearlyHolidayCalculation.NumberOfDaysLeft);
        }

        [Test]
        public async Task WhenAdminRequestYearlyHolidayCalculationsForEmployee_AdminCanRetrieveThem()
        {
            var YearlyHolidayCalculation = (await FacadeClientWrapper.AdminGetYearlyHolidayCalculationByEmployee(_employeeId, 2020, _tokenAdmin.access_token)).
                GetYearlyHolidayCalculationDto();

            Assert.IsNotNull(YearlyHolidayCalculation);
            Assert.AreEqual(2020, YearlyHolidayCalculation.Year);
            Assert.AreEqual(15, YearlyHolidayCalculation.NumberOfDays);
            Assert.AreEqual(15, YearlyHolidayCalculation.NumberOfDaysLeft);
        }

        [Test]
        public async Task WhenICarryOverDays_ChangesAreMadeInBothYears()
        {
            var command = new HolidaysBc.Models.Commands.CarryOverCommand
            {
                Days = 3,
                Year = 2016
            };

            await FacadeClientWrapper.CarryOverDays(command, _tokenUser.access_token);
            Thread.Sleep(200);

            var YearlyHolidayCalculation2016 = (await FacadeClientWrapper.GetYearlyHolidayCalculationByEmployee(2016, _tokenUser.access_token)).
            GetYearlyHolidayCalculationDto();
            var YearlyHolidayCalculation2017 = (await FacadeClientWrapper.GetYearlyHolidayCalculationByEmployee(2017, _tokenUser.access_token)).
            GetYearlyHolidayCalculationDto();

            Assert.IsNotNull(YearlyHolidayCalculation2016);
            Assert.AreEqual(2016, YearlyHolidayCalculation2016.Year);
            Assert.AreEqual(15, YearlyHolidayCalculation2016.NumberOfDays);
            Assert.AreEqual(12, YearlyHolidayCalculation2016.NumberOfDaysLeft);
            Assert.AreEqual(2017, YearlyHolidayCalculation2017.Year);
            Assert.AreEqual(15, YearlyHolidayCalculation2017.NumberOfDays);
            Assert.AreEqual(18, YearlyHolidayCalculation2017.NumberOfDaysLeft);
        }

        [Test]
        public async Task WhenISetCorrection_ChangesAreMadeInTotalHolidays()
        {
            var command = new HolidaysBc.Models.Commands.CorrectionDaysCommand
            {
                Days = -1.5m,
                Year = 2016,
                EmployeeId = _employeeId
            };

            await FacadeClientWrapper.CorrectionDays(command, _tokenAdmin.access_token);
            Thread.Sleep(200);

            var YearlyHolidayCalculation2016 = (await FacadeClientWrapper.GetYearlyHolidayCalculationByEmployee(2016, _tokenUser.access_token)).
            GetYearlyHolidayCalculationDto();

            Assert.IsNotNull(YearlyHolidayCalculation2016);
            Assert.AreEqual(2016, YearlyHolidayCalculation2016.Year);
            Assert.AreEqual(15, YearlyHolidayCalculation2016.NumberOfDays);
            Assert.AreEqual(13.5m, YearlyHolidayCalculation2016.NumberOfDaysLeft);
        }

        [TearDown]
        public void CleanUp()
        {
            
        }

        private Dictionary<DayOfWeek, bool> GetWorkingDays()
        {
            Dictionary<DayOfWeek, bool> daysOfTheWeek = new Dictionary<DayOfWeek, bool>();
            daysOfTheWeek[DayOfWeek.Monday] = true;
            daysOfTheWeek[DayOfWeek.Tuesday] = true;
            daysOfTheWeek[DayOfWeek.Wednesday] = true;
            daysOfTheWeek[DayOfWeek.Thursday] = true;
            daysOfTheWeek[DayOfWeek.Friday] = true;
            daysOfTheWeek[DayOfWeek.Saturday] = false;
            daysOfTheWeek[DayOfWeek.Sunday] = false;
            return daysOfTheWeek;
        }
    }
}
