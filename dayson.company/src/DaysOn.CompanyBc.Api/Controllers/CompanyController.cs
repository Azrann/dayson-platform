using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DaysOn.Common.AzureBus;
using DaysOn.Common.Events.Events;
using DaysOn.Common.Utilities.Extension;
using DaysOn.CompanyBc.Api.App_Start;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.BoundedContext.Events;
using DaysOn.CompanyBc.BoundedContext.Services;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.CompanyBc.Models.Dtos;
using DaysOn.Common.Domain.DomainEvents;
using log4net;

namespace DaysOn.CompanyBc.Api.Controllers
{
    [RoutePrefix("v1")]
    public class CompanyController : ApiControllerBase 
    {
        private readonly ICompanyService _companyService;
        private readonly ITeamService _teamService;
        private readonly IServiceBus _bus;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ISearchService _searchService;

        public CompanyController(ICompanyService companyService, ITeamService teamService, IServiceBus bus, ISearchService searchService)
        {
            _companyService = companyService;
            _teamService = teamService;
            _bus = bus;
            _searchService = searchService;
        }

        [Authorize(Roles = "MegaAdmin")]
        [Route("company/all")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            _logger.LogInf("GetAll Company method called");
            IEnumerable<Company> result = _companyService.GetAll();
            var companies = result.Select(AutoMapperBase.Mapper.Map<CompanyDto>);
            return Ok(companies);
        }

        [AllowAnonymous]
        [Route("company")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(CreateCompanyCommand command)
        {
            _logger.LogInf("Create Company method called");

            var companyId = command.CompanyId == Guid.Empty ? Guid.NewGuid():command.CompanyId;
            Company company = new Company(companyId)
            {
                Name = command.CompanyName
            };

            var ownerId = command.OwnerId == Guid.Empty ? Guid.NewGuid() : command.OwnerId;
            company.AddEmployee(new Employee(ownerId)
            {
                EmailAddress = command.AdminEmailAddress,
                Firstname = command.FirstName,
                Middlename = command.MiddleName,
                Lastname      = command.LastName,
                PhoneNumber = command.PhoneNumber,
                CreatedOn = DateTime.UtcNow,
                Role = "Owner",
                DateOfBirth = new DateTime(1900,1,1,1,1,1),
            });

            _companyService.AddCompany(company);
            DomainEvents.Raise(new CompanyCreatedEvent(companyId, command.CompanyName, command.AdminEmailAddress, ownerId, command.FirstName, command.LastName));
            _logger.LogInf($"Create Company succeed {companyId} with owner {ownerId}");

            var teamId = Guid.NewGuid();
            Team seniorTeam = new Team(teamId)
            {
                Name = "HR Owner"
            };
            _companyService.AddTeam(seniorTeam,companyId);
            DomainEvents.Raise(new TeamCreatedEvent(teamId, companyId, seniorTeam.Name, command.CorrelationId, ownerId));
            _logger.LogInf($"Create Company first team created succeed {teamId} for company {companyId}");

            _teamService.AddTeamMember(ownerId, teamId);
            _bus.Publish(new EmployeeAddedToTeamEvent
            {
                EmployeeId = ownerId,
                TeamId = teamId,
                CorrelationId = command.CorrelationId
            });

            _logger.LogInf($"Create Company first employee {ownerId} added to team {teamId} for company {companyId}");

            return Ok(companyId);
        }

        [Authorize(Roles = "Owner")]
        [Route("company")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCompany()
        {
            _logger.LogInf("GetCompany method called");
            Guid companyId = GetCompanyId();
            var company = _companyService.GetAll().FirstOrDefault(x => x.Id.ToString() == companyId.ToString());
            return Ok(AutoMapperBase.Mapper.Map<CompanyDto>(company));
        }

        [Authorize]
        [Route("company/mycompany")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMyCompany()
        {
            _logger.LogInf("GetMyCompany method called");
            Guid companyId = GetCompanyId();
            var company = _companyService.GetAll().FirstOrDefault(x => x.Id.ToString() == companyId.ToString());
            return Ok(new MyCompanyDto()
            {
                Id = company.Id,
                Name = company.Name,
                OwnerId = company.Employees.First(x => x.Role == "Owner").Id
            });
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/teams")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTeamsAsync()
        {
            Guid companyId = GetCompanyId();
            var teams = _companyService.GetTeams(companyId).Select(AutoMapperBase.Mapper.Map<TeamDto>);
            return Ok(teams);
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/team/all")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSimpleTeamsAsync()
        {
            Guid companyId = GetCompanyId();
            var teams = _companyService.GetTeams(companyId).Select(AutoMapperBase.Mapper.Map<IdNameDto>);
            return Ok(teams);
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/richteams")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRichTeamsAsync()
        {
            _logger.LogInf("GetRichTeamsAsync method called");
            Guid companyId = GetCompanyId();
            var teams = _companyService.GetTeams(companyId).Select(AutoMapperBase.Mapper.Map<RichTeamDto>);
            return Ok(teams);
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/team/{teamId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTeamAsync(Guid teamId)
        {
            Guid companyId = GetCompanyId();
            var team = _companyService.GetTeam(teamId, companyId);
            return Ok(AutoMapperBase.Mapper.Map<TeamDto>(team));
        }

        [Authorize]
        [Route("company/team")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMyTeamAsync()
        {
            Guid companyId = GetCompanyId();
            Guid teamId = GetTeamId();
            if (companyId == Guid.Empty || teamId == Guid.Empty)
            {
                return NotFound();
            }
            var team = _companyService.GetTeam(teamId, companyId);
            return Ok(AutoMapperBase.Mapper.Map<TeamDto>(team));
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/team")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateTeam([FromBody] CreateTeamCommand command)
        {
            Guid teamId = Guid.NewGuid();

            Guid companyId = GetCompanyId();
            var newTeam = new Team(teamId)
            {
                Name = command.Name
            };
            _companyService.AddTeam(newTeam, companyId);

            if (teamId != Guid.Empty)
            {
                DomainEvents.Raise(new TeamCreatedEvent(teamId,companyId,command.Name,command.CorrelationId,GetEmployeeId()));
            }
            return Ok(teamId);
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/team/update")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateTeam([FromBody] UpdateTeamCommand command)
        {
            Guid teamId = command.Id;
            Guid companyId = GetCompanyId();
            var newTeam = new Team(teamId)
            {
                Name = command.Name
            };
            _companyService.UpdateTeam(newTeam, companyId);

            if (teamId != Guid.Empty)
            {
                //TODO react to this event in other services.
                DomainEvents.Raise(new TeamUpdatedEvent(teamId, companyId, command.Name, command.CorrelationId, GetEmployeeId()));
            }
            return Ok(teamId);
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/team/remove")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveTeam([FromBody] Guid teamId)
        {
            Guid companyId = GetCompanyId();
            _companyService.RemoveTeam(teamId, companyId);

            DomainEvents.Raise(new TeamRemovedEvent(teamId, companyId, Guid.NewGuid(), GetEmployeeId()));
            return Ok();
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/employee/add")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateEmployee([FromBody] AddEmployeeCommand command)
        {
            if (command.Role == "Owner")
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Cannot add Owner user." };
                throw new HttpResponseException(msg);
            }
            Guid companyId = GetCompanyId();

            Guid userId = Guid.NewGuid();
            var newEmployee = new Employee(userId)
            {
                Firstname = command.Firstname,
                Lastname = command.Lastname,
                PhoneNumber = command.PhoneNumber,
                EmailAddress = command.EmailAddress,
                JobTitle= command.JobTitle,
                PictureUrl = command.PictureUrl,
                Role = command.Role,
                Middlename = command.MiddleName,
                DateOfBirth = command.DateOfBirth,
                OthersJson = command.OthersJson
            };
            _companyService.AddEmployee(newEmployee, companyId);

            return Ok(userId);
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/employee/update")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateEmployee([FromBody] UpdateEmployeeCommand command)
        {
            if (command.Role == "Owner")
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Cannot add Owner user." };
                throw new HttpResponseException(msg);
            }
            Guid companyId = GetCompanyId();

            var updatedEmployee = new Employee(command.Id)
            {
                Firstname = command.Firstname,
                Lastname = command.Lastname,
                PhoneNumber = command.PhoneNumber,
                EmailAddress = command.EmailAddress,
                JobTitle = command.JobTitle,
                PictureUrl = command.PictureUrl,
                Role = command.Role,
                Middlename = command.MiddleName,
                DateOfBirth = command.DateOfBirth,
                OthersJson = command.OthersJson
            };
            _companyService.UpdateEmployee(updatedEmployee, companyId);

            return Ok(command.Id);
        }

        [Authorize]
        [Route("company/employee/updateMyDetails")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateMyDetails([FromBody] UpdateEmployeeCommand command)
        {
            Guid companyId = GetCompanyId();
            Guid employeeId = GetEmployeeId();
            var updatedEmployee = new Employee(employeeId)
            {
                Firstname = command.Firstname,
                Lastname = command.Lastname,
                PhoneNumber = command.PhoneNumber,
                EmailAddress = command.EmailAddress,
                JobTitle = command.JobTitle,
                PictureUrl = command.PictureUrl,
                Middlename = command.MiddleName,
                DateOfBirth = command.DateOfBirth,
                OthersJson = command.OthersJson
            };
            _companyService.UpdateEmployee(updatedEmployee, companyId);

            return Ok(employeeId);
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/employee/{employeeId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployee([FromUri] Guid employeeId)
        {
            Guid companyId = GetCompanyId();

            var result = _companyService.GetEmployee(employeeId, companyId);

            return Ok(AutoMapperBase.Mapper.Map<EmployeeDto>(result));
        }

        [Authorize]
        [Route("company/employee")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMyDetails()
        {
            Guid companyId = GetCompanyId();
            Guid employeeId = GetEmployeeId();

            var result = _companyService.GetEmployee(employeeId, companyId);

            return Ok(AutoMapperBase.Mapper.Map<EmployeeDto>(result));
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/team/addEmployee")]
        [HttpPost]
        public async Task<IHttpActionResult> AddEmployeeToATeam([FromBody] AddEmployeeToATeamCommand command)
        {
            Guid companyId = GetCompanyId();

            if (_companyService.GetTeam(command.TeamId, companyId) != null)
            {
                _teamService.AddTeamMember(command.EmployeeId, command.TeamId);

                _bus.Publish(new EmployeeAddedToTeamEvent
                {
                    EmployeeId = command.EmployeeId,
                    TeamId = command.TeamId,
                    CorrelationId = Guid.NewGuid()
                });
            }
            
            return Ok();
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/team/removeEmployee")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveEmployeeToATeam([FromBody] RemoveEmployeeFromATeamCommand command)
        {
            Guid companyId = GetCompanyId();

            if (_companyService.GetTeam(command.TeamId, companyId) != null)
            {
                _teamService.RemoveTeamMember(command.EmployeeId, command.TeamId);

                _bus.Publish(new EmployeeRemovedFromTeamEvent
                {
                    EmployeeId = command.EmployeeId,
                    TeamId = command.TeamId,
                    CorrelationId = Guid.NewGuid()
                });
            }

            return Ok();
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/team/addManager")]
        [HttpPost]
        public async Task<IHttpActionResult> AddManager([FromBody] AddManagerCommand command)
        {
            Guid companyId = GetCompanyId();

            if (_companyService.GetTeam(command.TeamId, companyId) != null)
            {
                _teamService.SetManager(command.EmployeeId, command.TeamId);

                _bus.Publish(new ManagerAddedToTeamEvent
                {
                    CorrelationId = Guid.NewGuid(),
                    EmployeeId = command.EmployeeId,
                    TeamId = command.TeamId
                });
            }
            return Ok();
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/team/addTemporalManager")]
        [HttpPost]
        public async Task<IHttpActionResult> AddTemporalManager([FromBody] AddTemporalManagerCommand command)
        {
            Guid companyId = GetCompanyId();

            if (_companyService.GetTeam(command.TeamId, companyId) != null)
            {
                _teamService.SetTemporalManager(command.EmployeeId, command.TeamId);

                _bus.Publish(new TemporalManagerAddedToTeamEvent
                {
                    CorrelationId = Guid.NewGuid(),
                    EmployeeId = command.EmployeeId,
                    TeamId = command.TeamId
                });
            }

            return Ok();
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/team/removeManager")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveManager([FromBody] RemoveManagerCommand command)
        {
            Guid companyId = GetCompanyId();

            if (_companyService.GetTeam(command.TeamId, companyId) != null)
            {
                var managerId =_teamService.RemoveManager(command.TeamId);

                if (managerId != Guid.Empty)
                {
                    _bus.Publish(new ManagerRemovedFromTeamEvent()
                    {
                        CorrelationId = Guid.NewGuid(),
                        TeamId = command.TeamId,
                        EmployeeId = managerId
                    });
                }
            }

            return Ok();
        }

        [Authorize(Roles = "Admin,Owner")]
        [Route("company/team/removeTemporalManager")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveTemporalManager([FromBody] RemoveTemporalManagerCommand command)
        {
            Guid companyId = GetCompanyId();

            if (_companyService.GetTeam(command.TeamId, companyId) != null)
            {
                var temporalManagerId =  _teamService.RemoveTemporalManager(command.TeamId);

                if (temporalManagerId != Guid.Empty)
                {
                    _bus.Publish(new TemporalManagerRemovedFromTeamEvent()
                    {
                        CorrelationId = Guid.NewGuid(),
                        TeamId = command.TeamId,
                        EmployeeId = temporalManagerId
                    });
                }
            }

            return Ok();
        }

        [Authorize]
        [Route("company/search")]
        [HttpPost]
        public async Task<IHttpActionResult> SearchUsers([FromBody] SearchUsersRequest request)
        {
            Guid companyId = GetCompanyId();
            var result = _searchService.Search(request.TeamIds, request.Text, companyId);
            return Ok(result);
        }
    }
}
