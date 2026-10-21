using NUnit.Framework;
using DaysOn.Functions.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysOn.Functions.Api.Tests
{
    [TestFixture]
    public class WorkingDaysHelperTests
    {
        [Test]
        public void WhenICallGetWorkingDays_IGetTheRightResponse()
        { 
            var result = WorkingDaysHelper.GetWorkingDays(new DateTime(2020, 5, 20), new DateTime(2020, 6, 10), DaysFactory.GetWorkingDaysOfTheWeekMonToFri());
            Assert.AreEqual(16, result);
        }

        [Test]
        public void WhenICallGetWorkingDays2_IGetTheRightResponse()
        {
            var result = WorkingDaysHelper.GetWorkingDays(new DateTime(2017, 3, 15, 1, 26, 0), new DateTime(2017, 3, 20, 1, 26, 0), DaysFactory.GetWorkingDaysOfTheWeekMonToFri());
            Assert.AreEqual(4, result);
        }

        [TestCase(8, 9, 1)]
        [TestCase(21, 23, 1)]
        [TestCase(11, 15, 4)]
        [TestCase(21, 25, 2.5)]
        [TestCase(21, 24, 2)]
        public void WhenICallGetWorkingDaysWithHalfDays_IGetTheRightResponse(int dayFrom, int dayTo, double expectedResult)
        {
            var result = WorkingDaysHelper.GetWorkingDays(new DateTime(2022, 1, 21), new DateTime(2022, 1, 23, 12, 0, 0), DaysFactory.GetWorkingDaysOfTheWeekMonToFri());
            Assert.AreEqual(1, result);
        }

        [Test]
        [TestCase(6 ,10 , 4.5)]
        [TestCase(7, 10, 3.5)]
        [TestCase(18, 19, 0)]
        public void WhenICallGetWorkingDaysWithHalfDays2_IGetTheRightResponse(int dayFrom, int dayTo, double expectedResult)
        {
            var result = WorkingDaysHelper.GetWorkingDays(new DateTime(2020, 4, dayFrom, 12, 0, 0), new DateTime(2020, 4, dayTo), DaysFactory.GetWorkingDaysOfTheWeekMonToFri());
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(9, 22, 23, 24, 25, 26, 3)]
        [TestCase(9, 18, 19, 24, 25, 26, 2)]
        [TestCase(9, 20, 21, 22, 23, 26, 5)]
        [TestCase(9, 24, 25, 18, 11, 10, 0)]
        public void WhenICallFilterWorkingDaysOfTheWeek_IGetTheRightResponse(int month, int day1, int day2, int day3, int day4, int day5, int expectedResult)
        {
            var nonWorkingDays = new List<DateTime>
            {
                new DateTime(2016,month, day1),
                new DateTime(2016,month, day2),
                new DateTime(2016,month, day3),
                new DateTime(2016,month, day4),
                new DateTime(2016,month, day5)
            };
            var result = WorkingDaysHelper.FilterWorkingDaysOfTheWeek(DaysFactory.GetWorkingDaysOfTheWeekMonToFri(), nonWorkingDays);
            Assert.AreEqual(expectedResult, result.Count);
        }

        [Test]
        [TestCase(22, 23, 24, 25, 26, 22, 26, 5)]
        [TestCase(22, 23, 24, 25, 10, 22, 26, 4)]
        [TestCase(20, 23, 24, 25, 26, 22, 26, 4)]
        [TestCase(22, 23, 24, 4, 26, 22, 26, 4)]
        [TestCase(3, 23, 24, 25, 28, 1, 28, 5)]
        [TestCase(3, 23, 24, 25, 28, 3, 28, 5)]
        public void WhenICallGetNonWorking_IGetTheRightResponse(int day1, int day2, int day3, int day4, int day5, int from, int to, int expectedResult)
        {
            var nonWorkingDays = new List<DateTime>
            {
                new DateTime(2016,9, day1),
                new DateTime(2016,9, day2),
                new DateTime(2016,9, day3),
                new DateTime(2016,9, day4),
                new DateTime(2016,9, day5)
            };
            var result = WorkingDaysHelper.GetTotalBankHolidaysBetweenDates(nonWorkingDays, new DateTime(2016, 9, from), new DateTime(2016, 9, to));
            Assert.AreEqual(expectedResult, result.Count);
        }
    }

    public class DaysFactory
    {
        public static Dictionary<DayOfWeek, bool> GetWorkingDaysOfTheWeekMonToFri()
        {
            var result = new Dictionary<DayOfWeek, bool>();
            result.Add(DayOfWeek.Monday, true);
            result.Add(DayOfWeek.Tuesday, true);
            result.Add(DayOfWeek.Wednesday, true);
            result.Add(DayOfWeek.Thursday, true);
            result.Add(DayOfWeek.Friday, true);
            result.Add(DayOfWeek.Saturday, false);
            result.Add(DayOfWeek.Sunday, false);
            return result;
        }

        public static List<DateTime> GetNonWorkingHalfDays()
        {
            return new List<DateTime>
            {
                new DateTime(2018,1, 10),
                new DateTime(2018,3, 15),
                new DateTime(2018,4, 7),
                new DateTime(2018,10, 10),
            };
        }

        public static List<DateTime> GetNonWorkingDays()
        {
            return new List<DateTime>
            {
                new DateTime(2018,2, 28),
                new DateTime(2018,3, 5),
                new DateTime(2018,3, 9),
                new DateTime(2018,3, 10),
                new DateTime(2018,4, 1),
                new DateTime(2018,9, 10),
            };
        }
    }
}