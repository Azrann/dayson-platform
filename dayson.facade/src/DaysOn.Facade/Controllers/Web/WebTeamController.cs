using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.CompanyBc.Models.Dtos;
using DaysOn.Facade.Common;
using DaysOn.Facade.Common.Dtos.WebTeam;
using DaysOn.Facade.Models.WebModels.Team;
using DaysOn.Facade.Models.WebModels.User;
using log4net;
using Swashbuckle.Swagger.Annotations;
using DaysOn.Common.Utilities.Extension;

namespace DaysOn.Facade.Controllers.Web
{
    [RoutePrefix("v1")]
    public class WebTeamController : ApiControllerBase
    {
        private readonly ILog _logger;

        public WebTeamController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [AllowAnonymous]
        [Route("WebTeam/Ping")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> Ping()
        {
            _logger.LogDeb("WebTeam.Ping invoked");

            return await Task.FromResult<IHttpActionResult>(Ok("WebTeam service is up and running!"));
        }

        [AllowAnonymous]
        [Route("WebTeam/CreateTeam")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CreateTeamResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> CreateTeam([FromBody] CreateTeamRequest request)
        {
#if OFFLINE
            return Ok(await CreateTeamOffline.CreateTeam(request));
#else
            _logger.LogInf("WebTeam/CreateTeam method called");
            Guid teamId = await CompanyClientWrapper.CreateTeamAsync(request.Name, Request.Headers.Authorization.Parameter);

            if (teamId != Guid.Empty)
            {
                return Ok(new CreateTeamResponse()
                {
                    Team = new TeamOverview()
                    {
                        Id = teamId,
                        Name = request.Name,
                        MemberCount = 0
                    }
                });
            }

            return InternalServerError();
#endif
        }

        [AllowAnonymous]
        [Route("WebTeam/GetTeams")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GetTeamsResponse))]
        [HttpGet]
        public async Task<IHttpActionResult> GetTeams([FromUri] GetTeamsRequest request)
        {
#if OFFLINE
            return Ok(await GetTeamsOffline.GetTeams(request));
#else
            _logger.LogInf("GetTeamsForCompany method called");
            GetTeamsResponse response = new GetTeamsResponse()
            {
                Teams = new List<TeamOverview>()
            };
            IEnumerable<TeamDto> teams = await CompanyClientWrapper.GetTeamsAsync(Request.Headers.Authorization.Parameter);
            foreach (var teamDto in teams)
            {
                response.Teams.Add(new TeamOverview
                {
                    CompanyId = Guid.Empty,
                    Id = teamDto.Id,
                    Name = teamDto.Name,
                    MemberCount = teamDto.TeamMembers.Count
                });
            }
            return Ok(response);
#endif
        }

        [AllowAnonymous]
        [Route("WebTeam/GetTeamApprovers")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GetTeamApproversResponse))]
        [HttpGet]
        public async Task<IHttpActionResult> GetTeamApprovers([FromUri] GetTeamApproversRequest request)
        {
#if OFFLINE
            return Ok(await GetTeamApproversOffline.GetTeamApprovers(request));
#else
            _logger.LogInf("[WebTeam/GetTeamApprovers] method called");
            var response = new GetTeamApproversResponse()
            {
                Approvers = new List<ApproverOverview>()
            };
            var result = await (await ApproverClientWrapper.GetAllApproversAsync(Request.Headers.Authorization.Parameter)).GetListTeamApprovers();
            foreach (var approver in result)
            {
                response.Approvers.Add(new ApproverOverview()
                {
                    ApproverId = approver.Id,
                    FirstName = approver.Name //TODO at the moment I don't have all the fields you need but for a dropdown that should be enough
                });
            }

            return Ok(response);
#endif
        }

        [AllowAnonymous]
        [Route("WebTeam/UpdateTeam")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UpdateTeamResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateTeam([FromBody] UpdateTeamRequest request)
        {
#if OFFLINE
            return Ok(await UpdateTeamOffline.UpdateTeam(request));
#else
            _logger.LogInf("WebTeam/CreateTeam method called");
            Guid teamId = await CompanyClientWrapper.UpdateTeamAsync(request.TeamId, request.Name, Request.Headers.Authorization.Parameter);

            if (teamId != Guid.Empty)
            {
                return Ok(new UpdateTeamResponse()
                {
                    Team = new TeamOverview()
                    {
                        Id = teamId,
                        Name = request.Name,
                        MemberCount = 0
                    }
                });
            }

            return InternalServerError();
#endif
        }
    }
}
