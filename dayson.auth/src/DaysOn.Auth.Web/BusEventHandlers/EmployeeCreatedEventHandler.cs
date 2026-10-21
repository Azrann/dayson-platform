using System;
using System.Reflection;
using Castle.Core;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Services;
using DaysOn.Auth.Web.Validators;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;
using FluentValidation;
using log4net;
using Newtonsoft.Json;

namespace DaysOn.Auth.Web.BusEventHandlers
{
    [CastleComponent(typeof(IMessageHandler<EmployeeCreatedBusEvent>))]
    public class EmployeeCreatedEventHandler : IMessageHandler<EmployeeCreatedBusEvent>
    {
        private const string UserRole = "User";
        private readonly IAuthService _authService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public EmployeeCreatedEventHandler(IAuthService authService, IActivationService activationService)
        {
            _authService = authService;
        }

        public bool Handle(EmployeeCreatedBusEvent message)
        {
            _logger.LogInf($"[EmployeeCreatedEventHandler] invoked with Model=[{JsonConvertExt.SerializeObject(message)}]");

            IValidator<EmployeeCreatedBusEvent> validator = new EmployeeCreatedEventValidator();

            var validationResult = validator.Validate(message);

            if (!validationResult.IsValid)
            {
                _logger.LogInf($"[EmployeeCreatedEventHandler] Validation failed with Model=[{message}]");
                return true;
            }
            string role = string.IsNullOrEmpty(message.Role) ? UserRole : message.Role;
            var result = _authService.RegisterUserAsync(message.EmployeeEmailAddress, message.EmployeeId.GetHashCode().ToString(), message.CompanyId.ToString(), role, message.EmployeeId.ToString()).Result;

            if (result == null)
            {
                _logger.Warn($"[EmployeeCreatedEventHandler] Unknown error during registration with Model=[{message}].");
                return false;
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    _logger.Warn(
                        $"[EmployeeCreatedEventHandler] Errors during registration with Model=[{message}]. Errors=[{string.Join(",", result.Errors)}]");
                }
                return false;
            }

            return true;
        }

        public void LogError(Exception ex)
        {
            _logger.LogErr($"[EmployeeCreatedEventHandler] ExceptionThrown=[{JsonConvertExt.SerializeObject(ex)}]");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}