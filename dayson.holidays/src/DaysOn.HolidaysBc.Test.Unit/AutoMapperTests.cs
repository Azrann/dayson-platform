using System;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.Models.Dtos;
using DaysOn.HolidaysBc.Test.Common;
using NUnit.Framework;
using System.Collections.Generic;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.HolidaysBc.Api;
using DaysOn.HolidaysBc.Models.Commands;

namespace DaysOn.HolidaysBc.Test.Unit
{
    [TestFixture]
    public class AutoMapperTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test]
        public void WhenIMapAEmployee_ThenIGetTheRightDto()
        {
            Employee emp1 = new Employee(Guid.NewGuid())
            {
                MaxDaysCarriedOverNextYear = 5,
                Email = "user@example.invalid",
                HolidayEntitlement = 25,
                MaxConsecutiveDaysOff = 1,
                MinDaysAdvanceNoticeHolidays = 3,
                YearStartDay = new DateTime(2000, 2, 2),
            };
            emp1.InitializeWorkinDaysOfTheWeek(SetWeekendWorking());

            EmployeeDto dto = AutoMapperBase.GetAutoMapperBase().Map<EmployeeDto>(emp1);

            Assert.AreEqual(emp1.MaxDaysCarriedOverNextYear, dto.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(emp1.Email, dto.Email);
            Assert.AreEqual(emp1.HolidayEntitlement, dto.HolidayEntitlement);
            Assert.AreEqual(emp1.MaxConsecutiveDaysOff, dto.MaxConsecutiveDaysOff);
            Assert.AreEqual(emp1.MinDaysAdvanceNoticeHolidays, dto.MinDaysAdvanceNoticeHolidays);
            Assert.AreEqual(emp1.YearStartDay, dto.YearStartDay);
            Assert.AreEqual(emp1.WorkingDaysOfTheWeek[DayOfWeek.Monday], dto.WorkingDaysOfTheWeek[DayOfWeek.Monday]);
            Assert.AreEqual(emp1.WorkingDaysOfTheWeek[DayOfWeek.Sunday], dto.WorkingDaysOfTheWeek[DayOfWeek.Sunday]);
            Assert.AreEqual(emp1.WorkingDaysOfTheWeek[DayOfWeek.Friday], dto.WorkingDaysOfTheWeek[DayOfWeek.Friday]);
        }

        [Test]
        public void WhenIMapATeam_ThenIGetTheRightDto()
        {
            Team team1 = new Team(Guid.NewGuid())
            {
                Name = "TeamA"
            };

            Employee emp1 = new Employee(Guid.NewGuid())
            {
                MaxDaysCarriedOverNextYear = 5,
                Email = "user@example.invalid",
                HolidayEntitlement = 25
            };

            Employee emp2 = new Employee(Guid.NewGuid())
            {
                MaxDaysCarriedOverNextYear = 2,
                Email = "user@example.invalid",
                HolidayEntitlement = 21
            };

            Employee emp3 = new Employee(Guid.NewGuid())
            {
                MaxDaysCarriedOverNextYear = 1,
                Email = "user@example.invalid",
                HolidayEntitlement = 25
            };

            Employee emp4 = new Employee(Guid.NewGuid())
            {
                MaxDaysCarriedOverNextYear = 1,
                Email = "user@example.invalid",
                HolidayEntitlement = 25
            };

            TeamDto dto = AutoMapperBase.GetAutoMapperBase().Map<TeamDto>(team1);

            Assert.AreEqual(team1.Name, dto.Name);
        }

        [Test]
        public void WhenIMapAHolidayRequest_ThenIGetTheRightDto()
        {
            HolidayRequest holiday = new HolidayRequest(Guid.NewGuid())
            {
                Status = BoundedContext.Entities.Status.Aproved,
                EmployeeId = Guid.NewGuid(),
                CreatedOn = DateTime.Now.AddDays(-1),
                From = DateTime.Now.AddDays(10),
                To = DateTime.Now.AddDays(14),
                Year = DateTime.Now.Year
            };

            HolidayRequestDto dto = AutoMapperBase.GetAutoMapperBase().Map<HolidayRequestDto>(holiday);

            Assert.AreEqual(DaysOn.Common.Events.Dtos.Status.Aproved, dto.Status);
        }

        [Test]
        public void WhenIMapACompany_ThenIGetTheRightDto()
        {
            CompanyFactory factory = new CompanyFactory();
            var company = factory.CreateCompany();

            var companyDto = AutoMapperBase.GetAutoMapperBase().Map<CompanyDto>(company);
        }

        [Test]
        public void WhenIMapWorkingDays_ThenIGetTheRightWorkingDays()
        {
            var dtoWorkingDays = SetWeekendWorking();
            var workingDays = AutoMapperBase.GetAutoMapperBase().Map<Dictionary<DayOfWeek, bool>>(dtoWorkingDays);
            Assert.IsNotNull(workingDays);
        }

        [Test]
        public void WhenIMapAGlobalRuleDto_ThenIGetTheRightGlobalRules()
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

            var globalRules = AutoMapperBase.GetAutoMapperBase().Map<GlobalRules>(newGlobalRules);

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

        [Test]
        public void WhenIMapAddHolidayCommand_StatusIsTheSame()
        {
            var addHolidayCommand = new AddHolidayCommand
            {
                CorrelationId = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                Status = DaysOn.Common.Events.Dtos.Status.Canceled
            };

            var addHolidayRequestEvent = AutoMapperBase.GetAutoMapperBase().Map<AddHolidayRequestEvent>(addHolidayCommand);

            Assert.IsNotNull(addHolidayRequestEvent);
            Assert.AreEqual(DaysOn.Common.Events.Dtos.Status.Canceled, addHolidayRequestEvent.Status);
        }

        [Test]
        public void WhenIMapAddHolidayRequestCommand_StatusIsTheSame()
        {
            var addHolidayCommand = new AddHolidayRequestCommand
            {
                CorrelationId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Status = DaysOn.Common.Events.Dtos.Status.Rejected
            };

            var addHolidayRequestEvent = AutoMapperBase.GetAutoMapperBase().Map<AddHolidayRequestEvent>(addHolidayCommand);

            Assert.IsNotNull(addHolidayRequestEvent);
            Assert.AreEqual(DaysOn.Common.Events.Dtos.Status.Rejected, addHolidayRequestEvent.Status);
        }

        private static Dictionary<DayOfWeek, bool> SetWeekendWorking()
        {
            Dictionary<DayOfWeek, bool> result = new Dictionary<DayOfWeek, bool>();
            result.Add(DayOfWeek.Monday, false);
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
