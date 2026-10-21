using System;
using System.Linq;
using Castle.Core;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Events;
using DaysOn.HolidaysBc.BoundedContext.Infrastructure;
using DaysOn.Common.SqlDataAccess.Repositories;

namespace DaysOn.HolidaysBc.BoundedContext.Services
{
    [CastleComponent("CalculationService", LifestyleType.Transient)]
    public class CalculationService : ICalculationService
    {
        private readonly IRepository<YearlyHolidayCalculation> _yearlyHolidayCalculationRepository;
        private readonly IEmployeeService _employeeService;

        public CalculationService(IRepository<YearlyHolidayCalculation> yearlyHolidayCalculationRepository, IEmployeeService employeeService)
        {
            _yearlyHolidayCalculationRepository = yearlyHolidayCalculationRepository;
            _employeeService = employeeService;
        }

        public YearlyHolidayCalculation GetCalculationsById(Guid id)
        {
            return _yearlyHolidayCalculationRepository.GetAll(x => x.Id == id).FirstOrDefault();
        }

        public YearlyHolidayCalculation GetCalculationsByYear(Guid employeeId, int year)
        {
            return _yearlyHolidayCalculationRepository.GetAll(x => x.EmployeeId == employeeId && x.Year == year).FirstOrDefault();
        }

        public YearlyHolidayCalculation GetCalculationsByYearCreateIfNotExists(Guid employeeId, int year)
        {
            return GetCalculationsOrCreateIfNotExists(employeeId,year);
        }

        public void UpdateNumberOfDaysLeft(Guid employeeId, int year, decimal totalHolidays)
        {
            var calculations = GetCalculationsOrCreateIfNotExists(employeeId, year);

            if (calculations != null)
            {
                calculations.UpdateNumberOfDaysLeft(totalHolidays);
                _yearlyHolidayCalculationRepository.Save();
            }
        }

        public void CarryOverDays(Guid employeeId, int fromYear, decimal numDays)
        {
            var calculations = GetCalculationsOrCreateIfNotExists(employeeId, fromYear);

            if (calculations != null)
            {
                var maxDaysCarryOver = _employeeService.GetMaxDaysCarryForwardEmployee(employeeId);
                if (maxDaysCarryOver >= numDays)
                {
                    calculations.CarryOverDays(numDays);
                    _yearlyHolidayCalculationRepository.Save();
                    DomainEvents.Raise(new UpdateDaysCarryOverEvent(employeeId, fromYear+1, numDays));
                }
                else
                {
                    // TODO this should not break, maybe allow the employee although highliting that has broken a rule.
                    throw new ArgumentException(string.Format(Error.MaxDaysToCarryForwardAre,maxDaysCarryOver));
                }
            }
        }

        public void SetDaysCarriedForwardFromPreviousYear(Guid employeeId, int year, decimal numDays)
        {
            var calculations = GetCalculationsOrCreateIfNotExists(employeeId, year);

            if (calculations != null)
            {
                calculations.SetDaysCarriedForwardFromPreviousYearSetOverDays(numDays);
                _yearlyHolidayCalculationRepository.Save();
            }
        }

        public void SetCorrection(Guid employeeId, int year, decimal numDays)
        {
            var calculations = GetCalculationsOrCreateIfNotExists(employeeId, year);

            if (calculations != null)
            {
                calculations.SetCorrection(numDays);
                _yearlyHolidayCalculationRepository.Save();
            }
        }

        private YearlyHolidayCalculation GetCalculationsOrCreateIfNotExists(Guid employeeId, int year)
        {
            var calculations = _yearlyHolidayCalculationRepository.GetAll(x => x.EmployeeId == employeeId && x.Year == year).FirstOrDefault();

            if (calculations == null)
            {
                _employeeService.CreateCalculationsForEmployeeInYear(employeeId, year);
                calculations =
                    _yearlyHolidayCalculationRepository.GetAll(x => x.EmployeeId == employeeId && x.Year == year)
                        .FirstOrDefault();
            }
            return calculations;
        }
    }
}
