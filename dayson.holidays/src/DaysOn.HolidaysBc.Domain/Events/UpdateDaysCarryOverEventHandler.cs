using System.Reflection;
using DaysOn.Common.Domain.DomainEvents;
using DaysOn.Common.Utilities.Extension;
using DaysOn.HolidaysBc.BoundedContext.Services;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.HolidaysBc.BoundedContext.Events
{
    public class UpdateDaysCarryOverEventHandler : IEventHandler<UpdateDaysCarryOverEvent>
    {
        private readonly ICalculationService _calculationService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UpdateDaysCarryOverEventHandler(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        public void Handle(UpdateDaysCarryOverEvent domainEvent)
        {
            _logger.LogInf($"UpdateDaysCarryOverEventHandler Called with model [{JsonConvertExt.SerializeObject(domainEvent)}].");

            _calculationService.SetDaysCarriedForwardFromPreviousYear(domainEvent.EmployeeId, domainEvent.Year, domainEvent.Numdays);
        }
    }
}
