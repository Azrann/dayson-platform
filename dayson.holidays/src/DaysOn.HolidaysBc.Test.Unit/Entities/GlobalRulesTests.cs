using NUnit.Framework;
using System;

namespace DaysOn.HolidaysBc.BoundedContext.Entities.Tests
{
    [TestFixture]
    public class GlobalRulesTests
    {
        [Test]
        public void WhenICreateGlobalRules_ThenMondayToFridayIsDefault()
        {
            GlobalRules rules = new GlobalRules(Guid.NewGuid(), Guid.NewGuid());
            var workingDays = rules.WorkingDaysOfTheWeek;
            Assert.IsTrue(workingDays[DayOfWeek.Monday]);
            Assert.IsTrue(workingDays[DayOfWeek.Tuesday]);
            Assert.IsTrue(workingDays[DayOfWeek.Wednesday]);
            Assert.IsTrue(workingDays[DayOfWeek.Thursday]);
            Assert.IsTrue(workingDays[DayOfWeek.Friday]);
            Assert.IsFalse(workingDays[DayOfWeek.Saturday]);
            Assert.IsFalse(workingDays[DayOfWeek.Sunday]);
        }

        [Test]
        public void WhenICreateGlobalRules_ThenFirstOfJanuaryIDefaultStartingDate()
        {
            GlobalRules rules = new GlobalRules(Guid.NewGuid(), Guid.NewGuid());
            var startingDate = rules.YearStartDay;
            Assert.AreEqual(1, startingDate.Date.Day);
            Assert.AreEqual(1, startingDate.Date.Month);
        }

        [Test]
        public void WhenISetWorkingDay_ThenICanRetrieveIt()
        {
            GlobalRules rules = new GlobalRules(Guid.NewGuid(), Guid.NewGuid());
            
            rules.SetWorkingDay(DayOfWeek.Saturday);
            rules.SetWorkingDay(DayOfWeek.Sunday);

            var workingDays = rules.WorkingDaysOfTheWeek;
            Assert.IsTrue(workingDays[DayOfWeek.Monday]);
            Assert.IsTrue(workingDays[DayOfWeek.Tuesday]);
            Assert.IsTrue(workingDays[DayOfWeek.Wednesday]);
            Assert.IsTrue(workingDays[DayOfWeek.Thursday]);
            Assert.IsTrue(workingDays[DayOfWeek.Friday]);
            Assert.IsTrue(workingDays[DayOfWeek.Saturday]);
            Assert.IsTrue(workingDays[DayOfWeek.Sunday]);
        }

        [Test]
        public void WhenISetNonWorkingDay_ThenICanRetrieveIt()
        {
            GlobalRules rules = new GlobalRules(Guid.NewGuid(), Guid.NewGuid());
            rules.SetNonWorkingDay(DayOfWeek.Tuesday);
            rules.SetNonWorkingDay(DayOfWeek.Wednesday);
            var workingDays = rules.WorkingDaysOfTheWeek;
            Assert.IsTrue(workingDays[DayOfWeek.Monday]);
            Assert.IsFalse(workingDays[DayOfWeek.Tuesday]);
            Assert.IsFalse(workingDays[DayOfWeek.Wednesday]);
            Assert.IsTrue(workingDays[DayOfWeek.Thursday]);
            Assert.IsTrue(workingDays[DayOfWeek.Friday]);
            Assert.IsFalse(workingDays[DayOfWeek.Saturday]);
            Assert.IsFalse(workingDays[DayOfWeek.Sunday]);
        }
    }
}