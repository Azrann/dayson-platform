using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Auth.Domain.Services;
using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Auth.Models.CommandResult;
using DaysOn.Auth.Web.Validators;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Events.Events;
using DaysOn.Common.Utilities.Extension;
using FluentValidation;
using Newtonsoft.Json;

namespace DaysOn.Auth.Web.Controllers
{
    [RoutePrefix(("v1"))]
    public class PasswordController : ApiControllerBase
    {
        private readonly IResetPasswordService _resetPasswordService;
        private readonly IServiceBus _serviceBus;

        public PasswordController(IResetPasswordService resetPasswordService, IServiceBus serviceBus,
            INonActiveUsersService nonActiveUsersService)
        {
            _resetPasswordService = resetPasswordService;
            _serviceBus = serviceBus;
        }

        [AllowAnonymous]
        [Route("user/password/resetcode")]
        [HttpPost]
        public async Task<IHttpActionResult> ResetCode([FromBody] ResetPasswordCodeCommand command)
        {
            _logger.LogInf($"[ResetCode] invoked with Model=[{JsonConvertExt.SerializeObject(command)}] {GetCorrelationId()}");
            IValidator<ResetPasswordCodeCommand> validator = new ResetPasswordCodeCommandValidator();

            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
            {
                _logger.LogInf(
                    $"[ResetCode] Validation failed with Model=[{JsonConvertExt.SerializeObject(command)}] {GetCorrelationId()}");
                return ResponseMessage(
                    HttpStatusCode.BadRequest,
                    new CommandResult
                    {
                        Succeeded = false,
                        Errors = validationResult.Errors.Select(x => x.ErrorMessage)
                    });
            }

            var resetPasswordCodeId = Guid.NewGuid();

            await _resetPasswordService.CreateResetPasswordCodeAsync(resetPasswordCodeId, command.Email);

            _serviceBus.Publish(new SendEmailEvent
            {
                EmailTo = command.Email,
                Subject = "HRPlan restoring access.",
                BodyHtml =
                    $@"You have been created an account at HRPlan, set up your account in the following link <a href=""http://www.dayson-ui.gearhost.com/password/{
                        resetPasswordCodeId.ToString()
                    }"">click here</a>"
            });
            return Ok();
        }

        [AllowAnonymous]
        [Route("user/password/reset")]
        [HttpPost]
        public async Task<IHttpActionResult> Reset([FromBody] ResetPasswordCommand command)
        {
            _logger.LogInf($"[Reset] invoked with Model=[{JsonConvertExt.SerializeObject(command)}] {GetCorrelationId()}");
            IValidator<ResetPasswordCommand> validator = new ResetPasswordCommandValidator();

            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
            {
                _logger.LogInf(
                    $"[Reset] Validation failed with Model=[{JsonConvertExt.SerializeObject(command)}] {GetCorrelationId()}");
                return ResponseMessage(
                    HttpStatusCode.BadRequest,
                    new CommandResult
                    {
                        Succeeded = false,
                        Errors = validationResult.Errors.Select(x => x.ErrorMessage)
                    });
            }

            await _resetPasswordService.ResetPasswordAsync(command);

            return Ok();
        }
    }
}