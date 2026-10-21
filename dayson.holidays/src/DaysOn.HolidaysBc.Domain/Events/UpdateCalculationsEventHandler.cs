using System.Reflection;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.Common.Utilities.Extension;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.HolidaysBc.BoundedContext.Events
{
    public class UpdateCalculationsEventHandler : IEventHandler<UpdateCalculationsEvent>
    {
        private readonly IHolidayService _holidayService;
        private readonly ICalculationService _calculationService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UpdateCalculationsEventHandler(IHolidayService holidayService, ICalculationService calculationService)
        {
            _holidayService = holidayService;
            _calculationService = calculationService;
        }

        public void Handle(UpdateCalculationsEvent domainEvent)
        {
            _logger.LogInf($"UpdateCalculationsEventHandler Called with model [{JsonConvertExt.SerializeObject(domainEvent)}].");

            var totalHolidays = _holidayService.GetSumHolidaysPerYear(domainEvent.EmployeeId, domainEvent.Year);

            _calculationService.UpdateNumberOfDaysLeft(domainEvent.EmployeeId, domainEvent.Year, totalHolidays);

        }
    }
}
