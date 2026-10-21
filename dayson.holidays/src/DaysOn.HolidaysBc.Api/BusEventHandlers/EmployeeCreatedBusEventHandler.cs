using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.Core;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.HolidaysBc.Api.BusEventHandlers
{
    [CastleComponent(typeof(IMessageHandler<EmployeeCreatedBusEvent>))]
    public class EmployeeCreatedBusEventHandler : IMessageHandler<EmployeeCreatedBusEvent>
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public EmployeeCreatedBusEventHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public bool Handle(EmployeeCreatedBusEvent message)
        {
            _logger.LogInf($"[EmployeeCreatedBusEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(message)}]");

            try
            {
                _employeeService.AddEmployeeWithCompanyRules(new Employee(message.EmployeeId)
                {
                    CreatedOn = DateTime.UtcNow,
                    Name = $"{message.FirstName} {message.LastName}",
                    Email = message.EmployeeEmailAddress,
                    HolidayRequests = new List<HolidayRequest>(),
                    LastUpdated = DateTime.UtcNow,
                    MaxDaysCarriedOverNextYear = 5,
                    HolidayEntitlement = 20,
                    CompanyId = message.CompanyId,
                    YearStartDay = new DateTime(2000,1,1)
                }, message.CompanyId);
                _logger.LogInf($"[EmployeeCreatedBusEventHandler] completed with Model=[{JsonConvertExt.SerializeObject(message)}]");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogErr(
                    $"[EmployeeCreatedBusEventHandler] Error during registration with Model=[{JsonConvertExt.SerializeObject(message)}]. Error=[{ex.Message}] .Stack=[{ex.StackTrace}]");
                return false;
            }
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"[EmployeeCreatedBusEventHandler] ExceptionThrown=[{JsonConvertExt.SerializeObject(ex)}]");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}