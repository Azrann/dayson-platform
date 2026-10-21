using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DaysOn.Common.Utilities.Extension;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.Facade.Common;
using DaysOn.Facade.Common.Dtos.WebTeam;
using DaysOn.Facade.Models;
using DaysOn.HolidaysBc.Models.Dtos;
using log4net;
using Swashbuckle.Swagger.Annotations;
using IdNameDto = DaysOn.CompanyBc.Models.Dtos.IdNameDto;
using TeamDto = DaysOn.CompanyBc.Models.Dtos.TeamDto;

namespace DaysOn.Facade.Controllers
{
    [RoutePrefix(("v1"))]
    public class CompanyController : ApiControllerBase 
    {
        private const string GetMyCompanyUri = "v1/company";
        private ILog _logger;

        public CompanyController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [AllowAnonymous]
        [Route("company/all")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<CompanyDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> GetCompanies()
        {
            _logger.LogInf("GetCompanies method called");
            var token = Request.Headers.Authorization.Parameter;
            var baseUrl = ConfigurationManager.AppSettings[BaseUrl.CompanyServer.ToString()];
            var result = await HttpClientHelper.GetAsync(baseUrl, "v1/company/all", token);

            return ResponseMessage(result);
        }

        [AllowAnonymous]
        [Route("company")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TokenResponse))]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IHttpActionResult> Create(CreateCompanyCommand command)
        {
#if OFFLINE
            var handler = new JwtSecurityTokenHandler();

            //create symmetrickey
            var buffer = new byte[64];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(buffer);
            }
            var secretString = Convert.ToBase64String(buffer);

            //create jwt
            JwtSecurityToken token = handler.CreateToken(
                issuer: "issuer",
                audience: "audience",
                expires: DateTime.UtcNow.AddHours(10),
                subject: new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, "User")
                }),
                signingCredentials: new SigningCredentials(new InMemorySymmetricSecurityKey(buffer), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha512Digest));

            return Ok(new
            {
                access_token = handler.WriteToken(token)
            });
#else
            _logger.LogInf("CreateCompanyCommand method called");
            CheckCommandIds(command);

            Guid companyId = await CompanyClientWrapper.CreateCompanyAsync(command);

            if (companyId != Guid.Empty)
            {
                _logger.LogInf($"CreateCompanyCommand created company {companyId}");

                var tokenResponse =
 await AuthClientWrapper.GetTokenAsync(command.AdminEmailAddress, command.OwnerId.GetHashCode().ToString());

                if (tokenResponse.error == null && !string.IsNullOrEmpty(tokenResponse.access_token))
                {
                    return Ok(tokenResponse);
                }
                else
                {
                    _logger.LogErr($"CreateCompanyCommand logging failed for company:{companyId} and email:{command.AdminEmailAddress}");
                }
            }
            else
            {
                _logger.LogErr($"CreateCompanyCommand failed to create company for user:{command.AdminEmailAddress}");
            }

            return InternalServerError();
#endif
        }

        private static void CheckCommandIds(CreateCompanyCommand command)
        {
            if (command.CorrelationId == Guid.Empty)
            {
                command.CorrelationId = Guid.NewGuid();
            }

            if (command.OwnerId == Guid.Empty)
            {
                command.OwnerId = Guid.NewGuid();
            }

            if (command.CompanyId == Guid.Empty)
            {
                command.CompanyId = Guid.NewGuid();
            }
        }

        [AllowAnonymous]
        [Route("company/team")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Guid))]
        [HttpPost]
        public async Task<IHttpActionResult> CreateTeam(CreateTeamCommand command)
        {
            _logger.LogInf("CreateTeamCommand method called");
            Guid teamId = await CompanyClientWrapper.CreateTeamAsync(command.Name, Request.Headers.Authorization.Parameter);

            if (teamId != Guid.Empty)
            {
                return Ok(teamId);
            }

            return InternalServerError();
        }

        [AllowAnonymous]
        [Route("company/team/update")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Guid))]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateTeam(UpdateTeamRequest command)
        {
            _logger.LogInf("UpdateTeamCommand method called");
            Guid teamId = await CompanyClientWrapper.UpdateTeamAsync(command.TeamId, command.Name, Request.Headers.Authorization.Parameter);

            if (teamId != Guid.Empty)
            {
                return Ok(teamId);
            }

            return InternalServerError();
        }

        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK)]
        [Route("company/team/remove")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveTeam([FromBody] Guid teamId)
        {
            _logger.LogInf("RemoveTeam method called");
            return ResponseMessage(await CompanyClientWrapper.RemoveTeamAsync(teamId, Request.Headers.Authorization.Parameter));
        }

        [AllowAnonymous]
        [Route("company/team/{teamId}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TeamDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetTeamById([FromUri] Guid teamId)
        {
            _logger.LogInf("GetTeamById method called");
            TeamDto team = await CompanyClientWrapper.GetTeamAsync(teamId, Request.Headers.Authorization.Parameter);

            return Ok(team);
        }

        [AllowAnonymous]
        [Route("company/team")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TeamDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetMyTeam()
        {
            _logger.LogInf("GetMyTeam method called");
            TeamDto team = await CompanyClientWrapper.GetMyTeamAsync(Request.Headers.Authorization.Parameter);

            return Ok(team);
        }

        [AllowAnonymous]
        [Route("company/teams")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<TeamDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> GetTeamsForCompany()
        {
            _logger.LogInf("GetTeamsForCompany method called");
            IEnumerable<TeamDto> teams = await CompanyClientWrapper.GetTeamsAsync(Request.Headers.Authorization.Parameter);

            return Ok(teams);
        }

        [AllowAnonymous]
        [Route("company/team/all")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<IdNameDto>))]
        [HttpGet]
        public async Task<IHttpActionResult> GetSimpleTeamsForCompany()
        {
            _logger.LogInf("GetSimpleTeamsForCompany method called");
            IEnumerable<IdNameDto> simpleTeams = await CompanyClientWrapper.GetSimpleTeamsAsync(Request.Headers.Authorization.Parameter);

            return Ok( new { Teams = simpleTeams});
        }

        [AllowAnonymous]
        [Route("company")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMyCompany()
        {
            _logger.LogInf("GetMyCompanyAsync method called");

            var baseUrl = ConfigurationManager.AppSettings[BaseUrl.CompanyServer.ToString()];
            var result = await HttpClientHelper.GetAsync(baseUrl, GetMyCompanyUri, GetTokenFromRequest());

            return ResponseMessage(result);
        }

        [AllowAnonymous]
        [Route("company/employee/add")]
        [Obsolete("use v1/user/create instead that will create and add employee to a team")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateEmployee(AddEmployeeCommand command)
        {
            _logger.LogInf("AddEmployeeCommand method called");
            Guid employeeId = await CompanyClientWrapper.CreateEmployeeAsync(command, Request.Headers.Authorization.Parameter);

            if (employeeId != Guid.Empty)
            {
                return Ok(employeeId);
            }

            return InternalServerError();
        }

        [AllowAnonymous]
        [Route("company/employee")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EmployeeDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetMyDetails()
        {
            _logger.LogInf("GetMyDetails method called");
            var employee = await CompanyClientWrapper.GetMyDetailsEmployeeAsync(Request.Headers.Authorization.Parameter);

            return Ok(employee);
        }

        [AllowAnonymous]
        [Route("company/employee/{employeeId}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EmployeeDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployee([FromUri] Guid employeeId)
        {
            _logger.LogInf("GetEmployee method called");
            var employee = await CompanyClientWrapper.GetEmployeeAsync(employeeId, Request.Headers.Authorization.Parameter);

            return Ok(employee);
        }

        [AllowAnonymous]
        [Route("company/team/addEmployee")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> AddEmployeeToATeam([FromBody] AddEmployeeToATeamCommand command)
        {
            _logger.LogInf("AddEmployeeToATeamAsync method called");
            var response = await CompanyClientWrapper.AddEmployeeToATeamAsync(command, Request.Headers.Authorization.Parameter);

            return ResponseMessage(response);
        }

        [AllowAnonymous]
        [Route("company/employee/update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateDetails([FromBody] UpdateEmployeeCommand command)
        {
            _logger.LogInf("UpdateDetails method called");
            var response = await CompanyClientWrapper.UpdateEmployeeDetails(command, Request.Headers.Authorization.Parameter);

            return ResponseMessage(response);
        }

        [AllowAnonymous]
        [Route("company/employee/updateMyDetails")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateMyDetails([FromBody] UpdateEmployeeCommand command)
        {
            _logger.LogInf("UpdateMyDetails method called");
            var response = await CompanyClientWrapper.UpdateMyDetails(command, Request.Headers.Authorization.Parameter);

            return ResponseMessage(response);
        }

        [AllowAnonymous]
        [Route("company/team/removeEmployee")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveEmployeeFromATeam([FromBody] RemoveEmployeeFromATeamCommand command)
        {
            _logger.LogInf("RemoveEmployeeToATeam method called");
            var response = await CompanyClientWrapper.RemoveEmployeeFromATeamCommand(command, Request.Headers.Authorization.Parameter);

            return ResponseMessage(response);
        }

        [AllowAnonymous]
        [Route("company/team/addManager")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> AddManager([FromBody] AddManagerCommand command)
        {
            _logger.LogInf("AddManager method called");
            var response = await CompanyClientWrapper.AddManager(command, Request.Headers.Authorization.Parameter);

            return ResponseMessage(response);
        }

        [AllowAnonymous]
        [Route("company/team/addTemporalManager")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> AddTemporalManager([FromBody] AddTemporalManagerCommand command)
        {
            _logger.LogInf("AddTemporalManager method called");
            var response = await CompanyClientWrapper.AddTemporalManager(command, Request.Headers.Authorization.Parameter);

            return ResponseMessage(response);
        }

        [AllowAnonymous]
        [Route("company/team/removeManager")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveManager([FromBody] RemoveManagerCommand command)
        {
            _logger.LogInf("RemoveManager method called");
            var response = await CompanyClientWrapper.RemoveManager(command, Request.Headers.Authorization.Parameter);

            return ResponseMessage(response);
        }

        [AllowAnonymous]
        [Route("company/team/removeTemporalManager")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveTemporalManager([FromBody] RemoveTemporalManagerCommand command)
        {
            _logger.LogInf("RemoveTemporalManager method called");
            var response = await CompanyClientWrapper.RemoveTemporalManager(command, Request.Headers.Authorization.Parameter);

            return ResponseMessage(response);
        }
    }

    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string Role { get; set; }
        public string PictureUrl { get; set; }
        public string OthersJson { get; set; }
        public IdNameDto Company { get; set; }
        public IdNameDto MyTeam { get; set; }
    }
}
