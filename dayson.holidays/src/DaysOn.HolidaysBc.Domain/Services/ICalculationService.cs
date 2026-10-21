using System;
using DaysOn.HolidaysBc.BoundedContext.Entities;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    public interface ICalculationService
    {
        YearlyHolidayCalculation GetCalculationsById(Guid id);

        YearlyHolidayCalculation GetCalculationsByYear(Guid employeeId, int year);

        YearlyHolidayCalculation GetCalculationsByYearCreateIfNotExists(Guid employeeId, int year);

        void UpdateNumberOfDaysLeft(Guid employeeId, int year, decimal totalHolidays);

        void CarryOverDays(Guid employeeId, int fromYear, decimal numDays);

        void SetCorrection(Guid employeeId, int year, decimal numDays);

        void SetDaysCarriedForwardFromPreviousYear(Guid employeeId, int year, decimal numDays);
    }
}
