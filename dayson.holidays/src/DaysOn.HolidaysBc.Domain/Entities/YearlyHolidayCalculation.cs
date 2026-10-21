using System;
using DaysOn.Common.SqlDataAccess;
using DaysOn.HolidaysBc.BoundedContext.Infrastructure;

namespace DaysOn.HolidaysBc.BoundedContext.Entities
{
    public class YearlyHolidayCalculation : Entity<Guid>
    {
        public YearlyHolidayCalculation(Guid id) : base(id)
        {
        }

        protected YearlyHolidayCalculation()
            : base()
        {
        }

        public int Year { get; set; }
        public decimal DaysCarriedForwardFromPreviousYear { get; set; }
        public decimal DaysCarriedForwardToNextYear { get; set; }
        public decimal TotalHolidays { get; set; }

        public decimal Correction { get; set; }
        public int NumberOfDays { get; set; }
        public decimal NumberOfDaysLeft { get; set; }

        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public void UpdateNumberOfDaysLeft(decimal totalHolidays)
        {
            TotalHolidays = totalHolidays;
            NumberOfDaysLeft = NumberOfDays - totalHolidays + Correction + DaysCarriedForwardFromPreviousYear - DaysCarriedForwardToNextYear;
        }

        public void UpdateNumberOfDaysLeft()
        {
            NumberOfDaysLeft = NumberOfDays - TotalHolidays + Correction + DaysCarriedForwardFromPreviousYear - DaysCarriedForwardToNextYear;
        }

        public void CarryOverDays(decimal numDays)
        {
            UpdateNumberOfDaysLeft();
            if (NumberOfDaysLeft >= numDays)
            {
                DaysCarriedForwardToNextYear = numDays;
                UpdateNumberOfDaysLeft();
            }
            else
            {
                throw new ArgumentException(string.Format(Error.CannotCarryOverDaysBecuaseNotEnoughNumberOfDaysLeft,NumberOfDaysLeft, numDays));
            }
        }

        public void SetDaysCarriedForwardFromPreviousYearSetOverDays(decimal numDays)
        {
            DaysCarriedForwardFromPreviousYear = numDays;
            UpdateNumberOfDaysLeft();
        }

        public void SetCorrection(decimal correction)
        {
            Correction = correction;
            UpdateNumberOfDaysLeft();
        }
    }
}