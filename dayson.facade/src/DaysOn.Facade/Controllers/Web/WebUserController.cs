
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
    using DaysOn.Common.Utilities.Extension;
    using DaysOn.CompanyBc.Models.Commands;
using DaysOn.Facade.Common;
using DaysOn.Facade.Common.Dtos;
using DaysOn.Facade.Common.Dtos.WebUser;
using DaysOn.Facade.Models;
using DaysOn.Facade.Models.WebModels.User;
using DaysOn.HolidaysBc.Models.Commands;
using log4net;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;

namespace DaysOn.Facade.Controllers.Web
{
    [RoutePrefix("v1")]
    public class WebUserController : ApiControllerBase
    {
        private readonly ILog _logger;

        public WebUserController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [AllowAnonymous]
        [Route("user/teamsOverview")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TeamOverviewResponse))]
        [HttpGet]
        public async Task<IHttpActionResult> GetTeamsOverview()
        {
            _logger.LogInf("GetTeamsOverview method called");
            var token = Request.Headers.Authorization.Parameter;

            var teams = await CompanyClientWrapper.GetRichTeamsAsync(token);
            List<TeamOverviewDto> result = new List<TeamOverviewDto>();

            if (teams != null)
            {
                var approvers =
                    await (await ApproverClientWrapper.GetAllApproversAsync(token)).GetListTeamApprovers();
                var nonActiveUsers = await (await AuthClientWrapper.GetNonActiveUsers(token)).GetListGuid();

                foreach (var richTeamDto in teams)
                {
                    TeamOverviewDto team = Mapper.Map<TeamOverviewDto>(richTeamDto);

                    foreach (var member in team.TeamMembers)
                    {
                        if (approvers.Any(x => x.Id == member.Id))
                        {
                            member.HasApprovalRights = true;
                        }
                    }

                    foreach (var employeeOverviewDto in team.TeamMembers)
                    {
                        employeeOverviewDto.Status = nonActiveUsers != null && nonActiveUsers.Contains(employeeOverviewDto.Id)
                            ? EmployeeStatus.Inactive
                            : EmployeeStatus.Active;
                    }
                    result.Add(team);
                }
            }

            var role = GetRole();
            bool canAddTeams = role == "Admin" || role == "Owner";
            return Ok(new TeamOverviewResponse
            {
                CanAddNewTeams = canAddTeams,
                Teams = result
            });
        }

        [AllowAnonymous]
        [Route("user/create")]
        /* TODO :better error response on here*/
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IHttpActionResult> CreateUser(CreateEmployeeWithTeamCommand command)
        {
            _logger.LogInf("AddEmployeeWithTeam method called. Creating employee");
            AddEmployeeCommand addEmployeeCommand = command;
            
            Guid employeeId = await CompanyClientWrapper.CreateEmployeeAsync(addEmployeeCommand, Request.Headers.Authorization.Parameter);

            if (employeeId == Guid.Empty)
            {
                return InternalServerError();
            }

            _logger.LogInf("AddEmployeeWithTeam method called. Adding to the team");

            AddEmployeeToATeamCommand addEmployeeToATeamCommand = new AddEmployeeToATeamCommand()
            {
                EmployeeId = employeeId,
                TeamId = command.TeamId
            };
            var response = await CompanyClientWrapper.AddEmployeeToATeamAsync(addEmployeeToATeamCommand, Request.Headers.Authorization.Parameter);

            _logger.LogInf("AddEmployeeWithTeam completed");

            return ResponseMessage(response);
         }

        [AllowAnonymous]
        [Route("WebUser/SearchUsers")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SearchUsersResponse))]
        [HttpGet]
        public async Task<IHttpActionResult> SearchUsers([FromUri] SearchUsersRequest request)
        {
#if OFFLINE
            return Ok(await SearchUsersOffline.SearchUsers(request));
#else
            _logger.LogInf($"SearchUsers method called. request - {JsonConvertExt.SerializeObject(request)}");

            var response = await CompanyClientWrapper.SearchAsync(request, Request.Headers.Authorization.Parameter);

            _logger.LogInf("SearchUsers completed");
            return Ok(new SearchUsersResponse
            {
                Users = response.Select(u => new UserOverview
                {
                    PictureUrl = null, // TODO
                    TeamName = u.MyTeam.Name,
                    Id = u.Id,
                    CompanyId = u.Company.Id,
                    LastName = u.Lastname,
                    FirstName = u.Firstname,
                    MiddleName = u.Middlename,
                    JobTitle = u.JobTitle,
                    TeamId = u.MyTeam.Id,
                    ApproverId = null, // TODO may be not necessary
                    Email = u.EmailAddress,
                    IsApprover = false, // TODO may be not necessary??
                    Phone = u.PhoneNumber,
                    Type = (UserType) Enum.Parse(typeof(UserType), u.Role),
                }).ToList()
            });
#endif
        }

        [AllowAnonymous]
        [Route("WebUser/GetCurrentUserDetails")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GetCurrentUserDetailsResponse))]
        [HttpGet]
        public async Task<IHttpActionResult> GetCurrentUserDetails()
        {
#if OFFLINE
            return Ok(await GetCurrentUserDetailsOffline.GetCurrentUserDetails(null));
#else
            var employeeDetails = (await GlobalRulesClientWrapper.GetMyGlobalRules(Request.Headers.Authorization.Parameter)).GetEmployeeGlobalRules(); ;
            var employee = await CompanyClientWrapper.GetMyDetailsEmployeeAsync(Request.Headers.Authorization.Parameter);
            var isApprover = (await (await ApproverClientWrapper.GetAllApproversAsync(Request.Headers.Authorization.Parameter)).GetListTeamApprovers()).Any(x => x.Id == employee.Id);
            var user = new UserDetails
            {
                Id = employee.Id,
                TeamId = employee.MyTeam.Id,
                CompanyId = employee.Company.Id,
                TeamName = employee.MyTeam.Name,
                Phone = employee.PhoneNumber,
                Email = employee.EmailAddress,
                ApproverId = employeeDetails.Approver?.Id,
                LastName = employee.Lastname,
                FirstName = employee.Firstname,
                MiddleName = employee.Middlename,
                IsApprover = isApprover,
                JobTitle = employee.JobTitle,
                Type = (UserType) Enum.Parse(typeof(UserType), employee.Role ?? "User"),
                Rules = new UserRules
                {
                    MinDaysAdvanceNoticeHolidays = employeeDetails.MinDaysAdvanceNoticeHolidays,
                    HolidayEntitlement = employeeDetails.HolidayEntitlement,
                    MaxConsecutiveDaysOff = employeeDetails.MaxConsecutiveDaysOff,
                    MaxDaysCarriedOverNextYear = employeeDetails.MaxDaysCarriedOverNextYear,
                    UserId = employee.Id,
                    WorkingDays = WebCompanyMapper.GetWorkingDays(employeeDetails.WorkingDaysOfTheWeek),
                },
                ApproverName = employeeDetails.Approver?.Name,
                PictureUrl = employee.PictureUrl
            };

            return await Task.FromResult(Ok(
                new GetUserDetailsResponse
                {
                    User = user
                }));
#endif
        }

        [AllowAnonymous]
        [Route("WebUser/GetUserDetails")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GetUserDetailsResponse))]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserDetails([FromUri] GetUserDetailsRequest request)
        {
#if OFFLINE
            return Ok(await GetUserDetailsOffline.GetUserDetails(request));
#else

            var employeeDetails = (await GlobalRulesClientWrapper.GetEmployeeGlobalRules(request.UserId, Request.Headers.Authorization.Parameter)).GetEmployeeGlobalRules(); ;
            var employee = await CompanyClientWrapper.GetEmployeeAsync(request.UserId, Request.Headers.Authorization.Parameter);
            var isApprover = (await (await ApproverClientWrapper.GetAllApproversAsync(Request.Headers.Authorization.Parameter)).GetListTeamApprovers()).Any(x => x.Id == employee.Id);

            return await Task.FromResult(Ok(
                new GetUserDetailsResponse
                {
                    User = new UserDetails()
                    {
                        Id = employee.Id,
                        TeamId = employee.MyTeam.Id,
                        CompanyId = employee.Company.Id,
                        TeamName= employee.MyTeam.Name, 
                        Phone = employee.PhoneNumber,
                        Email = employee.EmailAddress,
                        ApproverId = employeeDetails.Approver?.Id,
                        LastName = employee.Lastname,
                        FirstName = employee.Firstname,
                        MiddleName = employee.Middlename,
                        IsApprover = isApprover,
                        JobTitle = employee.JobTitle,
                        Type = (UserType)Enum.Parse(typeof(UserType), employee.Role),
                        Rules = new UserRules
                        {
                            MinDaysAdvanceNoticeHolidays = employeeDetails.MinDaysAdvanceNoticeHolidays,
                            HolidayEntitlement = employeeDetails.HolidayEntitlement,
                            MaxConsecutiveDaysOff = employeeDetails.MaxConsecutiveDaysOff,
                            MaxDaysCarriedOverNextYear = employeeDetails.MaxDaysCarriedOverNextYear,
                            UserId = employee.Id,
                            WorkingDays = WebCompanyMapper.GetWorkingDays(employeeDetails.WorkingDaysOfTheWeek),
                        },
                        ApproverName= employeeDetails.Approver?.Name,
                        PictureUrl = employee.PictureUrl
                    }

                }));
#endif
        }

        [AllowAnonymous]
        [Route("WebUser/UpdateUser")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UpdateUserResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
#if OFFLINE
            return Ok(await UpdateUserOffline.UpdateUser(request));
#else
            Guid correlationId = Guid.NewGuid();
            _logger.LogInf("WebUser/UpdateUser method called. Updating employee");
            UpdateEmployeeCommand updateEmployeeCommand = new UpdateEmployeeCommand()
            {
                DateOfBirth = new DateTime(1980, 1, 1),
                MiddleName = request.MiddleName,
                OthersJson = "{}",
                Lastname = request.LastName,
                CorrelationId = correlationId,
                EmailAddress = request.Email,
                Firstname = request.FirstName,
                JobTitle = request.JobTitle,
                PhoneNumber = request.Phone,
                PictureUrl = "",
                Id = request.UserId,
                Role = request.Type == UserType.User ? "User" : "Administrator"
            };

            var updateResponse = await CompanyClientWrapper.UpdateEmployeeDetails(updateEmployeeCommand, Request.Headers.Authorization.Parameter);

            if (updateResponse.StatusCode != HttpStatusCode.OK)
            {
                return InternalServerError();
            }

            _logger.LogInf("WebUser/UpdateUser method called. Adding to the team");

            if (request.TeamId != Guid.Empty)
            {
                AddEmployeeToATeamCommand addEmployeeToATeamCommand = new AddEmployeeToATeamCommand()
                {
                    EmployeeId = request.UserId,
                    TeamId = request.TeamId
                };
                var response = await CompanyClientWrapper.AddEmployeeToATeamAsync(addEmployeeToATeamCommand, Request.Headers.Authorization.Parameter);
            }

            _logger.LogInf("WebUser/UpdateUser method called. Setting approver status");
            if (request.IsApprover)
            {
                var response = await ApproverClientWrapper.AddApprover(request.UserId, Request.Headers.Authorization.Parameter);
            }
            else
            {
                var response = await ApproverClientWrapper.RemoveApprover(request.UserId, Request.Headers.Authorization.Parameter);
            }

            _logger.LogInf("WebUser/UpdateUser method called. Setting approver to employee");
            if (request.ApproverId.HasValue && request.ApproverId.Value != Guid.Empty)
            {
                AddEmployeeToApproverCommand command = new AddEmployeeToApproverCommand()
                {
                    EmployeeId = request.UserId,
                    ApproverId = request.ApproverId.Value
                };
                var response = await ApproverClientWrapper.AddEmployeeToApproverAsync(command, Request.Headers.Authorization.Parameter);
            }
            else
            {
                AddEmployeeToApproverCommand command = new AddEmployeeToApproverCommand()
                {
                    EmployeeId = request.UserId,
                    ApproverId = Guid.Empty
                };
                var response = await ApproverClientWrapper.RemoveEmployeeFromApprover(command, Request.Headers.Authorization.Parameter);
            }

            var updateUserDetails = await GlobalRulesClientWrapper.SetEmployeeGlobalRules(request.UserId,
                WebCompanyMapper.GetGlobalRulesDto(request.Rules),
                Request.Headers.Authorization.Parameter);

            _logger.LogInf("WebUser/Update completed");

            return Ok(new UpdateUserResponse
                {
                    User = new UserDetails
                        {
                            Id = request.UserId,
                            TeamId = request.TeamId,
                            ApproverId = request.ApproverId,
                            Phone = request.Phone,
                            LastName = request.LastName,
                            Email = request.Email,
                            MiddleName = request.MiddleName,
                            IsApprover = request.IsApprover,
                            FirstName = request.FirstName,
                            Type = request.Type,
                            JobTitle = request.JobTitle,
                            Rules = request.Rules,
                            ApproverName = "", // is a hasle retrieve it ,You should have it in the UI 
                            CompanyId = request.Rules.CompanyId.Value,
                            TeamName = "", // is a hasle retrieve it ,You should have it in the UI
                    }
                }
            );
            #endif
        }

        [AllowAnonymous]
        [Route("WebUser/CreateUser")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CreateUserResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
#if OFFLINE
            return Ok(await CreateUserOffline.CreateUser(request));
#else
            Guid correlationId = Guid.NewGuid();
            _logger.LogInf("WebUser/CreateUser method called. Creating employee");
            AddEmployeeCommand addEmployeeCommand = new AddEmployeeCommand()
            {
                DateOfBirth = new DateTime(1980, 1, 1),
                MiddleName = request.MiddleName,
                OthersJson = "{}",
                Lastname = request.LastName,
                CorrelationId = correlationId,
                EmailAddress = request.Email,
                Firstname = request.FirstName,
                JobTitle = request.JobTitle,
                PhoneNumber = request.Phone,
                PictureUrl = "",
            };

            Guid employeeId = await CompanyClientWrapper.CreateEmployeeAsync(addEmployeeCommand, Request.Headers.Authorization.Parameter);

            if (employeeId == Guid.Empty)
            {
                return InternalServerError();
            }

            _logger.LogInf("WebUser/CreateUser method called. Adding to the team");

            if (request.TeamId != Guid.Empty)
            {
                AddEmployeeToATeamCommand addEmployeeToATeamCommand = new AddEmployeeToATeamCommand()
                {
                    EmployeeId = employeeId,
                    TeamId = request.TeamId
                };
                var response = await CompanyClientWrapper.AddEmployeeToATeamAsync(addEmployeeToATeamCommand, Request.Headers.Authorization.Parameter);
            }

            _logger.LogInf("WebUser/CreateUser method called. Adding approver status");
            if (request.IsApprover)
            {
                var response = await ApproverClientWrapper.AddApprover(employeeId, Request.Headers.Authorization.Parameter);
            }

            _logger.LogInf("WebUser/CreateUser method called. Adding approver status");
            if (request.ApproverId.HasValue && request.ApproverId.Value != Guid.Empty)
            {
                AddEmployeeToApproverCommand command = new AddEmployeeToApproverCommand()
                {
                    EmployeeId = employeeId,
                    ApproverId = request.ApproverId.Value
                };
                var response = await ApproverClientWrapper.AddEmployeeToApproverAsync(command, Request.Headers.Authorization.Parameter);
            }

            var createUserDetails = await GlobalRulesClientWrapper.SetEmployeeGlobalRules(employeeId,
                WebCompanyMapper.GetGlobalRulesDto(request.Rules),
                Request.Headers.Authorization.Parameter);
            _logger.LogInf("WebUser/CreateUser completed");

            var user = new UserOverview
            {
                Id = employeeId,
                TeamId = request.TeamId,
                ApproverId = request.ApproverId,
                Phone = request.Phone,
                LastName = request.LastName,
                Email = request.Email,
                MiddleName = request.MiddleName,
                IsApprover = request.IsApprover,
                FirstName = request.FirstName,
                Type = request.Type,
                JobTitle = request.JobTitle,
                CompanyId = Guid.Empty//You should already know , I can get it from the token if you need it.
            };

            return await Task.FromResult(Ok(user));
#endif
        }
    }
}
