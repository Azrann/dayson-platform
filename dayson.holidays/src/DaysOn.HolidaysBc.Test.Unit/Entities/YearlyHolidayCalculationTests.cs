using System;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using NUnit.Framework;

namespace DaysOn.HolidaysBc.Test.Unit.Entities
{
    [TestFixture]
    public class YearlyHolidayCalculationTests
    {
        [Test, Sequential]
        public void WhenIUpdateNumberOfDaysLeft_ThenICalculationsAreMadeCorrectly([Values(25,23,30)]int numDays,
            [Values(2,-2,-5)]decimal correction, [Values(2,0,5)]decimal daysCarriedForwardFromPreviousYear, [Values(1,0,0)]decimal daysCarriedForwardToNextYear, [Values(5.5,15,30)]decimal numHolidays, [Values(22.5,6,0)]decimal result)
        {
            YearlyHolidayCalculation calculation = new YearlyHolidayCalculation(Guid.NewGuid())
            {
                NumberOfDays = numDays,
                Correction = correction,
                DaysCarriedForwardFromPreviousYear = daysCarriedForwardFromPreviousYear,
                DaysCarriedForwardToNextYear = daysCarriedForwardToNextYear
            };

            calculation.UpdateNumberOfDaysLeft(numHolidays);

            Assert.AreEqual(result, calculation.NumberOfDaysLeft);
        }

        [Test]
        public void WhenICarryOverDays_DaysCarriedForwardGetUpdated()
        {
            YearlyHolidayCalculation calculation = new YearlyHolidayCalculation(Guid.NewGuid())
            {
                NumberOfDays = 25,
                Correction = 2,
                DaysCarriedForwardFromPreviousYear = 0,
                DaysCarriedForwardToNextYear = 0,
                TotalHolidays = 15
            };
            calculation.UpdateNumberOfDaysLeft(15);
            calculation.CarryOverDays(4);

            Assert.AreEqual(4, calculation.DaysCarriedForwardToNextYear);
        }

        [Test]
        public void WhenICarryOverDays_NumDaysLeftGetUpdated()
        {
            YearlyHolidayCalculation calculation = new YearlyHolidayCalculation(Guid.NewGuid())
            {
                NumberOfDays = 25,
                Correction = 2,
                DaysCarriedForwardFromPreviousYear = 0,
                DaysCarriedForwardToNextYear = 0,
                TotalHolidays = 15
            };
            calculation.UpdateNumberOfDaysLeft(15);
            calculation.CarryOverDays(4);

            Assert.AreEqual(8, calculation.NumberOfDaysLeft);
        }

        [Test]
        public void WhenICarryOverDaysMoreThanNumberOfDaysLeft_ExceptionIsThrown()
        {
            YearlyHolidayCalculation calculation = new YearlyHolidayCalculation(Guid.NewGuid())
            {
                NumberOfDays = 25,
                Correction = -5,
                DaysCarriedForwardFromPreviousYear = 0,
                DaysCarriedForwardToNextYear = 0,
                TotalHolidays = 17
            };
            
            Assert.That(() => calculation.CarryOverDays(4),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("NumberOfDaysLeft: 3 are smaller than the number of days you want to carry over 4."));
        }


        [Test]
        public void WhenISetDaysCarriedOverFromPreviousYear_CalculationsGetUpdated()
        {
            YearlyHolidayCalculation calculation = new YearlyHolidayCalculation(Guid.NewGuid())
            {
                NumberOfDays = 25,
                Correction = 2,
                DaysCarriedForwardFromPreviousYear = 0,
                DaysCarriedForwardToNextYear = 0,
                TotalHolidays = 15
            };
            calculation.SetDaysCarriedForwardFromPreviousYearSetOverDays(4);

            Assert.AreEqual(4, calculation.DaysCarriedForwardFromPreviousYear);
        }

        [Test]
        public void WhenISetDaysCarriedOverFromPreviousYear_NumDaysLeftGetUpdated()
        {
            YearlyHolidayCalculation calculation = new YearlyHolidayCalculation(Guid.NewGuid())
            {
                NumberOfDays = 25,
                Correction = 2,
                DaysCarriedForwardFromPreviousYear = 0,
                DaysCarriedForwardToNextYear = 0,
                TotalHolidays = 15
            };
            calculation.UpdateNumberOfDaysLeft(15);
            calculation.SetDaysCarriedForwardFromPreviousYearSetOverDays(4);

            Assert.AreEqual(16, calculation.NumberOfDaysLeft);
        }

        [Test,Sequential]
        public void WhenISetCorrection_NumDaysLeftGetUpdated([Values(4,-4)] decimal correction, [Values(14,6)] decimal result)
        {
            YearlyHolidayCalculation calculation = new YearlyHolidayCalculation(Guid.NewGuid())
            {
                NumberOfDays = 25,
                Correction = 0,
                DaysCarriedForwardFromPreviousYear = 0,
                DaysCarriedForwardToNextYear = 0,
                TotalHolidays = 15
            };
            calculation.SetCorrection(correction);

            Assert.AreEqual(result, calculation.NumberOfDaysLeft);
        }

        [Test]
        public void WhenISetCorrection_CalculationsGetUpdated()
        {
            YearlyHolidayCalculation calculation = new YearlyHolidayCalculation(Guid.NewGuid())
            {
                NumberOfDays = 25,
                Correction = 2,
                DaysCarriedForwardFromPreviousYear = 0,
                DaysCarriedForwardToNextYear = 0,
                TotalHolidays = 15
            };
            calculation.SetCorrection(3);

            Assert.AreEqual(3, calculation.Correction);
        }
    }
}