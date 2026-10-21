using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.Facade.Common;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading;
using DaysOn.CompanyBc.Models.Dtos;
using DaysOn.Facade.Tests.Acceptance.Helper;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    class FacadeControllerTest
    {
        [Test]
        public async Task WhenIAddANewUserToTheCompany_ThenILogInWithItsCredentialsAndSeeMyDetails()
        {
            Console.WriteLine("Facade={0}", ConfigurationManager.AppSettings["FacadeServer"]);
            var companyName = NameHelper.GenerateCompanyName();
            var token = await CompanyFactory.CreateCompanyAsync(companyName);

            var employeeUserName = NameHelper.GenerateEmployeeName();
            var employeeUserEmail = NameHelper.CreateEmail(employeeUserName, companyName);

            AddEmployeeCommand cmd = new AddEmployeeCommand
            {
                CorrelationId = Guid.NewGuid(),
                EmailAddress = employeeUserEmail,
                Firstname = employeeUserName,
                DateOfBirth = new DateTime(1900,1,1),
                MiddleName = "",
                Lastname = "Bolson"
            };

            var userId = await FacadeClientWrapper.CreateEmployeeAsync(cmd, token.access_token);

            Assert.AreNotEqual(Guid.Empty, userId);

            var tokenUser = await AuthClientWrapper.GetTokenAsync(employeeUserEmail, userId.GetHashCode().ToString());

            Assert.IsNotEmpty(tokenUser.access_token);

            var myDetails = await FacadeClientWrapper.GetMyDetailsAsync(tokenUser.access_token);

            Assert.AreEqual(employeeUserEmail, myDetails.EmailAddress);
        }

        [Test]
        public async Task WhenICreateACompany_ThenICanRetrieveIt()
        {
            var companyName = NameHelper.GenerateCompanyName();
            var ownerName = NameHelper.GenerateEmployeeName();
            var employeeEmail = NameHelper.CreateEmail(ownerName, companyName);
            CreateCompanyCommand command = new CreateCompanyCommand()
            {
                CompanyName = companyName,
                AdminEmailAddress = employeeEmail,
                MiddleName = "B",
                FirstName = ownerName,
                LastName = "Bolson",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
            };
            var token = await FacadeClientWrapper.CreateCompanyAsync(command);

            var company = await FacadeClientWrapper.GetMyCompanyAsync(token.access_token);
            Assert.AreEqual(1, company.Employees.Count);
            Assert.AreEqual(1, company.Teams.Count);
        }

        [Test]
        public async Task WhenICreateACompany_ThenICanRetrieveAdminDetails()
        {
            var companyName = NameHelper.GenerateCompanyName();
            var ownerName = NameHelper.GenerateEmployeeName();
            var employeeEmail = NameHelper.CreateEmail(ownerName, companyName);
            CreateCompanyCommand command = new CreateCompanyCommand()
            {
                CompanyName = companyName,
                AdminEmailAddress = employeeEmail,
                MiddleName = "B",
                FirstName = ownerName,
                LastName = "Bolson",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
            };
            var token = await FacadeClientWrapper.CreateCompanyAsync(command);

            var details = await FacadeClientWrapper.GetMyDetailsAsync(token.access_token);
            Assert.AreEqual(command.FirstName, details.Firstname);
            Assert.AreEqual(command.MiddleName, details.Middlename);
            Assert.AreEqual(command.LastName, details.Lastname);
            Assert.AreEqual(command.AdminEmailAddress, details.EmailAddress);
            Assert.AreEqual(command.PhoneNumber, details.PhoneNumber);
        }

        [Test]
        public async Task WhenICreateATeam_ThenICanRetrieveit()
        {
            Console.WriteLine("Facade={0}", ConfigurationManager.AppSettings["FacadeServer"]);
            var companyName = NameHelper.GenerateCompanyName();
            var token = await CompanyFactory.CreateCompanyAsync(companyName);

            var teamId = await FacadeClientWrapper.CreateTeamAsync("A Team", token.access_token);

            Guid userId1 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User");
            Guid userId2 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User");

            var response1 = await AddEmployeeToATeam(userId1, teamId, token);
            var response2 = await AddEmployeeToATeam(userId2, teamId, token);

            Assert.AreNotEqual(Guid.Empty, teamId);

            var simpleTeams = await FacadeClientWrapper.GetSimpleTeamsAsync(token.access_token);
            Assert.AreEqual(2, simpleTeams.Teams.Count);
            Assert.IsTrue(simpleTeams.Teams.Any(x => x.Id == teamId));
            Assert.IsTrue(simpleTeams.Teams.Any(x => x.Name == "A Team"));
        }

        [Test]
        public async Task WhenICreateATeam_ThenICanRemoveItWithTheAdminUser()
        {
            var companyName = NameHelper.GenerateCompanyName();
            var token = await CompanyFactory.CreateCompanyAsync(companyName);

            var teamAId =await CompanyClientWrapper.CreateTeamAsync("A Team", token.access_token);
            var teamBId =await CompanyClientWrapper.CreateTeamAsync("B Team", token.access_token);
            Assert.AreNotEqual(Guid.Empty, teamAId);
            Assert.AreNotEqual(Guid.Empty, teamBId);
            var company = await FacadeClientWrapper.GetMyCompanyAsync(token.access_token);

            Assert.IsTrue(company.Teams.Any(x => x.Name == "A Team"));
            Assert.AreEqual(3, company.Teams.Count);

            var result = await CompanyClientWrapper.RemoveTeamAsync(teamAId, token.access_token);
            company = await FacadeClientWrapper.GetMyCompanyAsync(token.access_token);

            Assert.IsTrue(company.Teams.Any(x => x.Name == "B Team"));
            Assert.AreEqual(2, company.Teams.Count);
        }

        [Test]
        public async Task WhenIRemoveANewEmployeeFromATeam_ThenICanRetrieveIt()
        {
            var companyName = NameHelper.GenerateCompanyName();
            var token = await CompanyFactory.CreateCompanyAsync(companyName);

            var teamName = NameHelper.GenerateTeamName();

            var teamId = await FacadeClientWrapper.CreateTeamAsync(teamName, token.access_token);

            Assert.AreNotEqual(Guid.Empty, teamId);

            var teams = (await FacadeClientWrapper.GetTeamsAsync(token.access_token)).ToList();

            Guid userId1 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User");
            Guid userId2 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User");

            var response1 = await AddEmployeeToATeam(userId1, teamId, token);
            var response2 = await AddEmployeeToATeam(userId2, teamId, token);

            Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);

            var team = await FacadeClientWrapper.GetTeamAsync(teamId, token.access_token);
            Assert.AreEqual(teamName, team.Name);
            Assert.AreEqual(2, team.TeamMembers.Count);

            RemoveEmployeeFromATeamCommand removeFromTeam = new RemoveEmployeeFromATeamCommand
            {
                EmployeeId = userId1,
                TeamId = teamId
            };

            var response3 = await FacadeClientWrapper.RemoveEmployeeFromATeamCommand(removeFromTeam, token.access_token);
            Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);

            team = await FacadeClientWrapper.GetTeamAsync(teamId, token.access_token);
            Assert.AreEqual(teamName, team.Name);
            Assert.AreEqual(1, team.TeamMembers.Count);
        }

        [Test]
        public async Task WhenIRemoveATeam_EmployeesWithinThatTeamDoesntBelongToAnyOtherTeam()
        {
            var companyName = NameHelper.GenerateCompanyName();
            var token = await CompanyFactory.CreateCompanyAsync(companyName);

            Thread.Sleep(100);
            var teamAId = await FacadeClientWrapper.CreateTeamAsync("A Team", token.access_token);
            var teamBId = await FacadeClientWrapper.CreateTeamAsync("B Team", token.access_token);
            Assert.AreNotEqual(Guid.Empty, teamAId);
            Assert.AreNotEqual(Guid.Empty, teamBId);

            Guid userId1 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User");
            Guid userId2 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User");

            var response1 = await AddEmployeeToATeam(userId1, teamAId, token);
            var response2 = await AddEmployeeToATeam(userId2, teamAId, token);

            Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
            var company = await FacadeClientWrapper.GetMyCompanyAsync(token.access_token);

            Assert.IsTrue(company.Teams.Any(x => x.Name == "A Team"));
            Assert.AreEqual(3, company.Teams.Count);
            Assert.AreEqual(3, company.Employees.Count);

            var result = await FacadeClientWrapper.RemoveTeamAsync(teamAId, token.access_token);
            company = await FacadeClientWrapper.GetMyCompanyAsync(token.access_token);
            
            Assert.AreEqual(2, company.Teams.Count);
            Assert.IsTrue(company.Teams.Any(x => x.Name == "B Team"));
            Assert.AreEqual(3, company.Employees.Count);

            var employee = await FacadeClientWrapper.GetEmployeeAsync(userId1, token.access_token);
            Assert.IsNull(employee.MyTeam);
            Assert.AreEqual(companyName, employee.Company.Name);
        }

        [Test]
        public async Task WhenIAddOrRemoveAManager_TheTeamShowsTheRigthValues()
        {
            var companyName = NameHelper.GenerateCompanyName();
            var token = await CompanyFactory.CreateCompanyAsync(companyName);

            var teamAId = await FacadeClientWrapper.CreateTeamAsync("A Team", token.access_token);
            var teamBId = await FacadeClientWrapper.CreateTeamAsync("B Team", token.access_token);
            Assert.AreNotEqual(Guid.Empty, teamAId);
            Assert.AreNotEqual(Guid.Empty, teamBId);

            Guid userId1 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User");
            Guid userId2 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User");

            var result = await FacadeClientWrapper.AddManager(new AddManagerCommand()
            {
                EmployeeId = userId1,
                TeamId = teamAId
            }, token.access_token);
            result = await FacadeClientWrapper.AddManager(new AddManagerCommand()
            {
                EmployeeId = userId1,
                TeamId = teamBId
            }, token.access_token);

            var teams = (await FacadeClientWrapper.GetTeamsAsync(token.access_token)).ToList();

            Assert.AreEqual(3, teams.Count);
            Assert.AreEqual(userId1, teams.First(x => x.Id == teamAId).Manager.Id);
            Assert.AreEqual(userId1, teams.First(x => x.Id == teamBId).Manager.Id);

            var employee = await FacadeClientWrapper.GetEmployeeAsync(userId1, token.access_token);
            Assert.AreEqual(2, employee.ManagerOf.Count);
            Assert.IsTrue(employee.ManagerOf.Any(x => x.Id == teamAId));
            Assert.IsTrue(employee.ManagerOf.Any(x => x.Id == teamBId));

            result = await FacadeClientWrapper.RemoveManager(new RemoveManagerCommand()
            {
                TeamId = teamBId
            }, token.access_token);

            employee = await FacadeClientWrapper.GetEmployeeAsync(userId1, token.access_token);
            Assert.AreEqual(1, employee.ManagerOf.Count);
            Assert.IsTrue(employee.ManagerOf.Any(x => x.Id == teamAId));
            Assert.IsFalse(employee.ManagerOf.Any(x => x.Id == teamBId));

            teams = (await FacadeClientWrapper.GetTeamsAsync(token.access_token)).ToList();

            Assert.AreEqual(3, teams.Count);
            Assert.AreEqual(userId1, teams.First(x=> x.Id == teamAId).Manager.Id);
            Assert.IsNull(teams.First(x => x.Id == teamBId).Manager);
        }

        [Test]
        public async Task WhenIAddAManager_TheRoleOfTheUserIsManager()
        {
            var companyName = NameHelper.GenerateCompanyName();
            var token = await CompanyFactory.CreateCompanyAsync(companyName);

            var teamAId = await FacadeClientWrapper.CreateTeamAsync("A Team", token.access_token);
            Assert.AreNotEqual(Guid.Empty, teamAId);

            Guid userId1 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User");
            
            var result = await FacadeClientWrapper.AddManager(new AddManagerCommand()
            {
                EmployeeId = userId1,
                TeamId = teamAId
            }, token.access_token);

            var teams = (await FacadeClientWrapper.GetTeamsAsync(token.access_token)).ToList();

            Assert.AreEqual(2, teams.Count);
            Assert.AreEqual(userId1, teams.First(x =>x.Id == teamAId).Manager.Id);

            var employee = await FacadeClientWrapper.GetEmployeeAsync(userId1, token.access_token);
            var managerToken = await AuthClientWrapper.GetTokenAsync(employee.EmailAddress, employee.Id.GetHashCode().ToString());
            var role = GetClaims(managerToken, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            Assert.IsTrue(role.Any(x => x == "Manager"));
        }

        [Test]
        public async Task WhenIRemoveAManager_TheRoleOfTheUserIsNotManager()
        {
            var companyName = NameHelper.GenerateCompanyName();
            var token = await CompanyFactory.CreateCompanyAsync(companyName);

            var teamAId = await FacadeClientWrapper.CreateTeamAsync("A Team", token.access_token);
            Assert.AreNotEqual(Guid.Empty, teamAId);

            Guid userId1 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User");

            var result = await FacadeClientWrapper.AddManager(new AddManagerCommand()
            {
                EmployeeId = userId1,
                TeamId = teamAId
            }, token.access_token);

            var teams = (await FacadeClientWrapper.GetTeamsAsync(token.access_token)).ToList();

            Assert.AreEqual(2, teams.Count);
            Assert.AreEqual(userId1, teams.First(x =>x.Id == teamAId).Manager.Id);

            var employee = await FacadeClientWrapper.GetEmployeeAsync(userId1, token.access_token);

            result = await FacadeClientWrapper.RemoveManager(new RemoveManagerCommand()
            {
                TeamId = teamAId
            }, token.access_token);

            var managerToken = await AuthClientWrapper.GetTokenAsync(employee.EmailAddress, userId1.GetHashCode().ToString());
            var role = GetClaims(managerToken, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            Assert.Contains("User",role);
            Assert.IsFalse(role.Contains("Manager"));
        }
        
        [Test]
        public async Task WhenIAddOrRemoveATemporalManager_TheTeamShowsTheRigthValues()
        {
            var companyName = NameHelper.GenerateCompanyName();
            var token = await CompanyFactory.CreateCompanyAsync(companyName);

            var teamAId = await FacadeClientWrapper.CreateTeamAsync("A Team", token.access_token);
            var teamBId = await FacadeClientWrapper.CreateTeamAsync("B Team", token.access_token);
            Assert.AreNotEqual(Guid.Empty, teamAId);
            Assert.AreNotEqual(Guid.Empty, teamBId);

            Guid userId1 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User");
            Guid userId2 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User");

            var result = await FacadeClientWrapper.AddTemporalManager(new AddTemporalManagerCommand()
            {
                EmployeeId = userId1,
                TeamId = teamAId
            }, token.access_token);
            result = await FacadeClientWrapper.AddTemporalManager(new AddTemporalManagerCommand()
            {
                EmployeeId = userId2,
                TeamId = teamBId
            }, token.access_token);
            result = await FacadeClientWrapper.AddTemporalManager(new AddTemporalManagerCommand()
            {
                EmployeeId = userId1,
                TeamId = teamBId
            }, token.access_token);

            var teams = (await FacadeClientWrapper.GetTeamsAsync(token.access_token)).ToList();
            
            Assert.AreEqual(3, teams.Count);
            Assert.AreEqual(userId1, teams.First(x => x.Id == teamAId).TemporalManager.Id);
            Assert.AreEqual(userId1, teams.First(x => x.Id == teamBId).TemporalManager.Id);

            var employee = await FacadeClientWrapper.GetEmployeeAsync(userId1, token.access_token);
            Assert.AreEqual(2, employee.TemporalManagerOf.Count);
            Assert.IsTrue(employee.TemporalManagerOf.Any(x => x.Id == teamAId));
            Assert.IsTrue(employee.TemporalManagerOf.Any(x => x.Id == teamBId));

            var employee2 = await FacadeClientWrapper.GetEmployeeAsync(userId2, token.access_token);
            Assert.AreEqual(0, employee2.TemporalManagerOf.Count);

            result = await FacadeClientWrapper.RemoveTemporalManager(new RemoveTemporalManagerCommand()
            {
                TeamId = teamBId
            }, token.access_token);

            employee = await FacadeClientWrapper.GetEmployeeAsync(userId1, token.access_token);
            Assert.AreEqual(1, employee.TemporalManagerOf.Count);
            Assert.IsTrue(employee.TemporalManagerOf.Any(x => x.Id == teamAId));
            Assert.IsFalse(employee.TemporalManagerOf.Any(x => x.Id == teamBId));


            teams = (await FacadeClientWrapper.GetTeamsAsync(token.access_token)).ToList();

            Assert.AreEqual(3, teams.Count);
            Assert.AreEqual(userId1, teams.First(x => x.Id == teamAId).TemporalManager.Id);
            Assert.IsNull(teams.First(x => x.Id == teamBId).TemporalManager);
        }

        private static async Task<HttpResponseMessage> AddEmployeeToATeam(Guid userId1, Guid teamId, TokenResponse token)
        {
            AddEmployeeToATeamCommand addToTeam1 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId1,
                TeamId = teamId
            };

            var response1 = await FacadeClientWrapper.AddEmployeeToATeam(addToTeam1, token.access_token);
            return response1;
        }

        private static List<string> GetClaims(TokenResponse managerToken, string claim)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokens = handler.ReadToken(managerToken.access_token) as JwtSecurityToken;
            return tokens?.Claims.Where(x => x.Type == claim).Select(x => x.Value).ToList();
        }
    }
}
