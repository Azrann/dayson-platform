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
    [CastleComponent(typeof(IMessageHandler<CompanyCreatedBusEvent>))]
    public class CompanyCreatedBusEventHandler : IMessageHandler<CompanyCreatedBusEvent>
    {
        private readonly ICompanyService _companyService;
        private readonly IEmployeeService _employeeService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CompanyCreatedBusEventHandler(ICompanyService companyService, IEmployeeService employeeService)
        {
            _companyService = companyService;
            _employeeService = employeeService;
        }

        public bool Handle(CompanyCreatedBusEvent message)
        {
            _logger.LogInf($"[CompanyCreatedBusEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(message)}]");

            try
            {
                _companyService.AddCompany(new Company(message.CompanyId)
                {
                    GlobalRules = new GlobalRules(Guid.NewGuid(), message.CompanyId),
                    CreatedOn = DateTime.UtcNow,
                    Name = message.Name,
                });

                _employeeService.AddEmployeeWithCompanyRules(new Employee(message.OwnerId)
                {
                    CreatedOn = DateTime.UtcNow,
                    Name = $"{message.FirstName} {message.LastName}",
                    Email = message.AdminEmailAddress,
                    HolidayRequests = new List<HolidayRequest>(),
                    LastUpdated = DateTime.UtcNow,
                    MaxDaysCarriedOverNextYear = 5,
                    HolidayEntitlement = 20,
                    CompanyId = message.CompanyId,
                    YearStartDay = new DateTime(2000, 1, 1)
                }, message.CompanyId);
                _logger.LogInf($"[CompanyCreatedBusEventHandler] completed");
                return true;
            }
            catch (Exception ex)
            {
                string innerExceptionText = "empty";
                if (ex.InnerException != null)
                {
                    innerExceptionText = JsonConvertExt.SerializeObject(ex.InnerException);
                }
                _logger.LogErr(
                    $"[CompanyCreatedBusEventHandler] Error during registration with Model=[{JsonConvertExt.SerializeObject(message)}]. Error=[{ex.Message}] .Stack=[{ex.StackTrace}] .InnerException=[{innerExceptionText}]");
                return false;
            }
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"[CompanyCreatedBusEventHandler] ExceptionThrown=[{JsonConvertExt.SerializeObject(ex)}]");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}
