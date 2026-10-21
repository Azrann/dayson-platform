using NUnit.Framework;
using System;
using System.Linq;

namespace DaysOn.HolidaysBc.BoundedContext.Entities.Tests
{
    [TestFixture]
    public class OpenClosedDatesTests
    {
        [Test]
        public void WhenICreateOpenCloseDate_ThenListsAreNotNull()
        {
            OpenClosedDates closedDates = new OpenClosedDates();
            Assert.IsNotNull(closedDates.ClosedDays);
            Assert.IsNotNull(closedDates.ClosedHalfDays);
        }

        [Test]
        public void WhenIAddToClosedDatesIGetTheRightResult()
        {
            var date1 = new DateTime(2015, 10, 1);
            var date2 = new DateTime(2015, 10, 2);
            var date3 = new DateTime(2010, 2, 1);
            var date4 = new DateTime(2010, 2, 2);
            OpenClosedDates closedDates1 = new OpenClosedDates();
            closedDates1.ClosedDays.Add(date1);
            closedDates1.ClosedDays.Add(date2);

            OpenClosedDates closedDates2 = new OpenClosedDates();
            closedDates2.ClosedDays.Add(date3);
            closedDates2.ClosedDays.Add(date4);

            var result = closedDates1 + closedDates2;

            Assert.AreEqual(4, result.ClosedDays.Count);
            Assert.AreEqual(0, result.ClosedHalfDays.Count);
            Assert.Contains(date1, result.ClosedDays);
            Assert.Contains(date2, result.ClosedDays);
            Assert.Contains(date3, result.ClosedDays);
            Assert.Contains(date4, result.ClosedDays);
        }

        [Test]
        public void WhenIAddToClosedHalfDatesIGetTheRightResult()
        {
            var date1 = new DateTime(2015, 10, 1);
            var date2 = new DateTime(2015, 10, 2);
            var date3 = new DateTime(2010, 2, 1);
            var date4 = new DateTime(2010, 2, 2);
            OpenClosedDates closedDates1 = new OpenClosedDates();
            closedDates1.ClosedHalfDays.Add(date1);
            closedDates1.ClosedHalfDays.Add(date2);

            OpenClosedDates closedDates2 = new OpenClosedDates();
            closedDates2.ClosedHalfDays.Add(date3);
            closedDates2.ClosedHalfDays.Add(date4);

            var result = closedDates1 + closedDates2;

            Assert.AreEqual(4, result.ClosedHalfDays.Count);
            Assert.AreEqual(0, result.ClosedDays.Count);
            Assert.Contains(date1, result.ClosedHalfDays);
            Assert.Contains(date2, result.ClosedHalfDays);
            Assert.Contains(date3, result.ClosedHalfDays);
            Assert.Contains(date4, result.ClosedHalfDays);
        }

        [Test]
        public void WhenISetYearForClosedHalfDays_ThenIGetTheRightResult()
        {
            var date1 = new DateTime(2015, 10, 1);
            var date2 = new DateTime(2015, 10, 2);
            var date3 = new DateTime(2010, 2, 1);
            var date4 = new DateTime(2010, 2, 2);
            OpenClosedDates closedDates1 = new OpenClosedDates();
            closedDates1.ClosedHalfDays.Add(date1);
            closedDates1.ClosedHalfDays.Add(date2);
            closedDates1.ClosedHalfDays.Add(date3);
            closedDates1.ClosedHalfDays.Add(date4);

            closedDates1.SetYear(2015);

            Assert.AreEqual(4, closedDates1.ClosedHalfDays.Count);
            Assert.Contains(date1, closedDates1.ClosedHalfDays);
            Assert.Contains(date2, closedDates1.ClosedHalfDays);
            Assert.IsEmpty(closedDates1.ClosedHalfDays.Where(x => x.Year != 2015));
        }

        [Test]
        public void WhenISetYearForClosedDays_ThenIGetTheRightResult()
        {
            var date1 = new DateTime(2015, 10, 1);
            var date2 = new DateTime(2015, 10, 2);
            var date3 = new DateTime(2010, 2, 1);
            var date4 = new DateTime(2010, 2, 2);
            OpenClosedDates closedDates1 = new OpenClosedDates();
            closedDates1.ClosedDays.Add(date1);
            closedDates1.ClosedDays.Add(date2);
            closedDates1.ClosedDays.Add(date3);
            closedDates1.ClosedDays.Add(date4);

            closedDates1.SetYear(2015);

            Assert.AreEqual(4, closedDates1.ClosedDays.Count);
            Assert.Contains(date1, closedDates1.ClosedDays);
            Assert.Contains(date2, closedDates1.ClosedDays);
            Assert.IsEmpty(closedDates1.ClosedDays.Where(x => x.Year != 2015));
        }
    }
}