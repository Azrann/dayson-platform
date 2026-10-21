using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Common.Utilities.Extension;
using DaysOn.Facade.Common;
using DaysOn.Facade.Models;
using log4net;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using RegisterUserCommand = DaysOn.Facade.Common.Dtos.RegisterUserCommand;
#pragma warning disable 1591

namespace DaysOn.Facade.Controllers
{
    [RoutePrefix("v1")]
    public class AuthController : ApiController
    {
        private const string RegisterRequestUri = "v1/user/register";
        private const string UserResetPasswordCodeApi = "v1/user/password/resetcode";
        private const string UserResetPasswordApi = "v1/user/password/reset";
        private const string UserActivateApi = "v1/user/activate";
        private const string UserResetActivationCodeApi = "v1/user/activation/resetactivationcode";

        private readonly ILog _logger;
        private readonly string _authServerUrl = ConfigurationManager.AppSettings[BaseUrl.AuthServer.ToString()];

        public AuthController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
       
        [AllowAnonymous]
        [Route("auth/user/register")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public async Task<IHttpActionResult> RegisterAsync(RegisterUserCommand model)
        {
            _logger.LogInf("Register method called");

            var response = await HttpClientHelper.PostAsync(model, _authServerUrl, RegisterRequestUri);
            return ResponseMessage(response);
        }

        [AllowAnonymous]
        [Route("auth/user/activate")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public async Task<IHttpActionResult> ActivateAsync(ActivateUserCommand command)
        {
            _logger.LogInf($"[Activate] invoked with command=[{JsonConvertExt.SerializeObject(command)}]");

            return ResponseMessage(await HttpClientHelper.PostAsync(command, _authServerUrl, UserActivateApi));
        }

        [AllowAnonymous]
        [Route("auth/user/activationcode")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public async Task<IHttpActionResult> ActivationCodeAsync([FromBody] Guid userId)
        {
            _logger.LogInf(string.Format("[ActivationCode] invoked"));

            return ResponseMessage(await HttpClientHelper.PostAsync(null, _authServerUrl, UserResetActivationCodeApi));
        }

        [AllowAnonymous]
        [Route("oauth2/token")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TokenResponse))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, Type = typeof(IEnumerable<HttpResponseMessage>))]
        [HttpPost]
        public async Task<IHttpActionResult> TokenAsync(GetTokenCommand getTokenCommand)
        {
            _logger.LogInf("Token method called");

            var tokenResponse = await AuthClientWrapper.GetTokenAsync(getTokenCommand.Username, getTokenCommand.Password);

            if (tokenResponse.error == null && !string.IsNullOrEmpty(tokenResponse.access_token))
            {
                return Ok(tokenResponse);
            }

            return ResponseMessage(new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized));
        }

        [AllowAnonymous]
        [Route("auth/user/password/reset")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ResponseMessageResult))]
        [HttpPost]
        public async Task<IHttpActionResult> PasswordReset(ResetPasswordCommand command)
        {
            _logger.LogInf($"[PasswordReset] invoked with command=[{JsonConvertExt.SerializeObject(command)}]");

            return ResponseMessage(await HttpClientHelper.PostAsync(command, _authServerUrl, UserResetPasswordApi));
        }

        [AllowAnonymous]
        [Route("auth/user/password/resetcode")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ResponseMessageResult))]
        [HttpPost]
        public async Task<IHttpActionResult> PasswordResetCode(ResetPasswordCodeCommand command)
        {
            _logger.LogInf($"[PasswordResetCode] invoked with command=[{JsonConvertExt.SerializeObject(command)}]");

            return ResponseMessage(await HttpClientHelper.PostAsync(command, _authServerUrl, UserResetPasswordCodeApi));
        }
    }
}