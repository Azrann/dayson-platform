using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Services;
using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Auth.Models.CommandResult;
using DaysOn.Auth.Web.Helper;
using DaysOn.Auth.Web.Validators;
using DaysOn.Common;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Events.Events;
using DaysOn.Common.Utilities.Extension;
using FluentValidation;
using Newtonsoft.Json.Linq;
using ApiErrors = DaysOn.Auth.Domain.ApiErrors;

namespace DaysOn.Auth.Web.Controllers
{   
    [RoutePrefix("v1")]
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IActivationService _activationService;
        private readonly IServiceBus _serviceBus;
        private readonly INonActiveUsersService _nonActiveUsersService;
        private readonly IUserIdService _userIdService;
        private readonly IPubnubMessageBus _pubnubMessageBus;

        public AuthenticationController(IAuthService authService, IActivationService activationService, IServiceBus serviceBus,
            INonActiveUsersService nonActiveUsersService, IUserIdService userIdService, IPubnubMessageBus pubnubMessageBus)
        {
            _authService = authService;
            _activationService = activationService;
            _serviceBus = serviceBus;
            _nonActiveUsersService = nonActiveUsersService;
            _userIdService = userIdService;
            _pubnubMessageBus = pubnubMessageBus;
            
        }

        [Route("oauth")]
        [AllowAnonymous]
        public IHttpActionResult Auth(LoginCommand command)
        {
            _logger.LogInf($"[Auth] method called with ApiCommand {command}");
            var tokenResponse = ClientPostHelper.GetToken(command);

            var retryCount = 1;

            while (retryCount < 4 && !IsValidToken(tokenResponse.access_token))
            {
                Thread.Sleep(500);
                _logger.LogInf($"[Auth] retry getting token number {retryCount} {command}");
                tokenResponse = ClientPostHelper.GetToken(command);
                retryCount++;
            }

            if (IsValidToken(tokenResponse.access_token))
            {
                string channelId = _userIdService.GetUserIdByEmail(command.Username);

                if (command.Password == new Guid(channelId).GetHashCode().ToString())
                {
                    _serviceBus.Publish(new UserLoggedFirstTimeEvent
                    {
                        CorrelationId = command.CorrelationId,
                        Email = command.Username
                    });
                }

                // TODO: Grant here access to TeamChannel and CompanyChannel, when user change teams it also grant to the new teamchannel
                _pubnubMessageBus.SetSecretKey().Start();
                _pubnubMessageBus.Grant(tokenResponse.access_token, channelId, (result, status) => {
                    if (status.Error)
                    {
                        _logger.LogErr($"Failed granting access to channel in PubNub {status.ErrorData.Information} Erro=[{JsonConvertExt.SerializeObject(status.ErrorData.Throwable)}]");
                    }
                });
            }
            
            return Ok(tokenResponse);
        }

        private bool IsValidToken(string access_token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(access_token))
            {
                var tokenS = handler.ReadToken(access_token) as JwtSecurityToken;

                if (tokenS.Claims.Any(claim =>
                    claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"))
                {
                    var role = tokenS.Claims.First(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
                    if (role == "MegaAdmin")
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }

                if (tokenS.Claims.Any(claim => claim.Type == "Company"))
                {
                    var companyId = tokenS.Claims.First(claim => claim.Type == "Company").Value;
                    if (!string.IsNullOrEmpty(companyId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        [Authorize(Roles = "Admin, Owner, MegaAdmin")]
        [Route("user/register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterUserCommand model)
        {
            _logger.LogInf($"[Register] invoked with Model=[{model}]");
            IValidator<RegisterUserCommand> validator = new UserRegistrationValidator();

            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                _logger.LogInf($"[Register] Validation failed with Model=[{model}]");
                return ResponseMessage(
                    HttpStatusCode.BadRequest,
                    new RegisterUserCommandResult
                    {
                        Succeeded = false,
                        Errors = validationResult.Errors.Select(x => x.ErrorMessage)
                    });
            }


            var result = await _authService.RegisterUserAsync(model.Email, "bobthebuilder123!", GetCompanyFromClaim(), model.Role, model.EmployeeId == Guid.Empty ? "" : model.EmployeeId.ToString() );

            if (result == null)
            {
                _logger.LogInf($"[Register] Unknown error during registration with Model=[{model}].");
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    _logger.LogInf(
                        $"[Register] Errors during registration with Model=[{model}]. Errors=[{string.Join(",", result.Errors)}]");
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    _logger.LogInf($"[Register] Unknown error during registration with Model=[{model}].");
                    return ResponseMessage(
                        HttpStatusCode.BadRequest,
                        new RegisterUserCommandResult
                        {
                            Succeeded = result.Succeeded,
                            Errors = new List<string> { ApiErrors.UnknownError }
                        });
                }

                return ResponseMessage(
                    HttpStatusCode.BadRequest,
                    new RegisterUserCommandResult
                    {
                        Succeeded = result.Succeeded,
                        Errors = result.Errors.Select(x => x)
                    });
            }

            return ResponseMessage(HttpStatusCode.OK, result);
        }

        [AllowAnonymous]
        [Route("user/activate")]
        [HttpPost]
        public async Task<IHttpActionResult> Activate([FromBody] ActivateUserCommand model)
        {
            _logger.LogInf($"[Activate] invoked with Model=[{model}]");
            IValidator<ActivateUserCommand> validator = new UserActivationValidator();

            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                _logger.LogInf($"[Activate] Validation failed with Model=[{model}]");
                return ResponseMessage(
                    HttpStatusCode.BadRequest,
                    new RegisterUserCommandResult
                    {
                        Succeeded = false,
                        Errors = validationResult.Errors.Select(x => x.ErrorMessage)
                    });
            }

            await _activationService.ActivateAsync(model.ActivationCode);

            return Ok();
        }

        [Authorize]
        [Route("user/nonactive/all")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsersStatus()
        {
            _logger.LogInf("[GetUsersStatus] invoked");
            var companyId = GetCompanyFromClaim();

            if (string.IsNullOrEmpty(companyId) || companyId == Guid.Empty.ToString())
            {
                _logger.Warn($"[GetUsersStatus] companyId is null called.");
                return BadRequest();
            }

            var result = _nonActiveUsersService.GetInactiveUsers(companyId);

            return Ok(result);
        }
    }
}
