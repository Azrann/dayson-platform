using System;
using System.Configuration;
using System.Linq;
using System.Net;
using DaysOn.Facade.Common;
using Newtonsoft.Json;
using NUnit.Framework;
using DaysOn.CompanyBc.Models.Commands;
using System.Threading;
using System.Threading.Tasks;
using DaysOn.Facade.Models;
using DaysOn.Facade.Tests.Acceptance.Helper;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class CompanyControllerTest
    {
        [Test]
        public async Task WhenIRequestAllCompaniesWithValidToken_ThenIReceiveAListWithAllCompanies()
        {
            const string requestUri = "v1/company/all";
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Thread.Sleep(150);
            var token = await AuthClientWrapper.GetTokenAsync();

            var content = await (await HttpClientHelper.GetAsync(baseUrl, requestUri, token.access_token)).Content.ReadAsStringAsync();

            dynamic companies = JsonConvert.DeserializeObject(content);

            Assert.That(companies[0].name.Value, Is.Not.Null.Or.Empty);
        }

        [Test]
        public async Task WhenICreateACompany_ThenICanAccessWithTheAdminUser()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token = await CreateACompany(companyName);

            var company = await FacadeClientWrapper.GetMyCompanyAsync(token.access_token);

            Assert.AreEqual(companyName, company.Name);
        }      

        [Test]
        public async Task WhenICreateACompany_ThenICanCreateATeamWithTheAdminUser()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token = await CreateACompany(companyName);

            var teamId = await CompanyClientWrapper.CreateTeamAsync("A Team", token.access_token);

            Assert.AreNotEqual(Guid.Empty, teamId);

            var company = await FacadeClientWrapper.GetMyCompanyAsync(token.access_token);
        
            Assert.IsTrue(company.Teams.Any(x => x.Name == "A Team"));
        }

        [Test]
        public async Task WhenICreateATeam_ThenICanRemoveItWithTheAdminUser()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token = await CreateACompany(companyName);

            var teamAId = await CompanyClientWrapper.CreateTeamAsync("A Team", token.access_token);
            var teamBId = await CompanyClientWrapper.CreateTeamAsync("B Team", token.access_token);
            Assert.AreNotEqual(Guid.Empty, teamAId);
            Assert.AreNotEqual(Guid.Empty, teamBId);
            var company = await FacadeClientWrapper.GetMyCompanyAsync(token.access_token);

            Guid userId = await CreateEmployeeAsync(companyName, token);

            AddEmployeeToATeamCommand cmd2 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId,
                TeamId = teamAId
            };

            var response = await CompanyClientWrapper.AddEmployeeToATeamAsync(cmd2, token.access_token);

            Assert.IsTrue(company.Teams.Any(x => x.Name == "A Team"));
            Assert.AreEqual(3, company.Teams.Count);

            var result = await CompanyClientWrapper.RemoveTeamAsync(teamAId, token.access_token);
            company = await FacadeClientWrapper.GetMyCompanyAsync(token.access_token);

            Assert.IsTrue(company.Teams.Any(x => x.Name == "B Team"));
            Assert.AreEqual(2, company.Teams.Count);
        }

        [Test]
        public async Task ICanRetrieveATeamWithMyUser()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token = await CreateACompany(companyName);

            var teamAId = await CompanyClientWrapper.CreateTeamAsync("A Team", token.access_token);
            Assert.AreNotEqual(Guid.Empty, teamAId);

            var employeeData = await EmployeeFactory.NewCreateEmployeeAsync(companyName, token, "User");
            Guid userId = employeeData.Item2;

            AddEmployeeToATeamCommand cmd2 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId,
                TeamId = teamAId
            };

            var response = await CompanyClientWrapper.AddEmployeeToATeamAsync(cmd2, token.access_token);
            Thread.Sleep(100);
            var tokenUser = await AuthClientWrapper.GetTokenAsync(employeeData.Item1, userId.GetHashCode().ToString());


            var myTeam = await CompanyClientWrapper.GetMyTeamAsync(tokenUser.access_token);

            Assert.AreEqual("A Team", myTeam.Name);
            Assert.AreEqual(1, myTeam.TeamMembers.Count);
        }

        [Test]
        public async Task WhenIAddANewUserToTheCompany_ThenILogInWithItsCredentials()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token = await CreateACompany(companyName);

            string employeeUserEmail = NameHelper.CreateEmail("test", companyName);

            Guid userId = await CreateEmployeeAsync(companyName, token, "Admin", employeeUserEmail);
            Assert.AreNotEqual(Guid.Empty, userId);

            var tokenUser = await AuthClientWrapper.GetTokenAsync(employeeUserEmail, userId.GetHashCode().ToString());
            var employeeResult = await CompanyClientWrapper.GetEmployeeAsync(userId, token.access_token);
            Assert.IsNotEmpty(tokenUser.access_token);
            Assert.AreEqual("Admin",employeeResult.Role);
        }

        [Test]
        public async Task WhenIAmLoggedIn_ThenICanUpdateMyDetails()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token = await CreateACompany(companyName);

            string employeeUserEmail = NameHelper.CreateEmail("test", companyName);

            Guid userId = await CreateEmployeeAsync(companyName, token, "User", employeeUserEmail);
            Assert.AreNotEqual(Guid.Empty, userId);

            var tokenUser = await AuthClientWrapper.GetTokenAsync(employeeUserEmail, userId.GetHashCode().ToString());
            var mydetails = await CompanyClientWrapper.GetMyDetailsEmployeeAsync(tokenUser.access_token);
            Assert.IsNotEmpty(tokenUser.access_token);

            var updateCommand = GenerateUpdateEmployeeCommand();
            var updated = await CompanyClientWrapper.UpdateMyDetails(updateCommand, tokenUser.access_token);
            var myUpdatedDetails = await CompanyClientWrapper.GetMyDetailsEmployeeAsync(tokenUser.access_token);

            Assert.AreEqual(updateCommand.Firstname, myUpdatedDetails.Firstname);
            Assert.AreEqual(updateCommand.MiddleName, myUpdatedDetails.Middlename);
            Assert.AreEqual(updateCommand.Lastname, myUpdatedDetails.Lastname);
            Assert.AreEqual(updateCommand.DateOfBirth, myUpdatedDetails.DateOfBirth);
            Assert.AreEqual(updateCommand.EmailAddress, myUpdatedDetails.EmailAddress);
            Assert.AreEqual(updateCommand.JobTitle, myUpdatedDetails.JobTitle);
            Assert.AreEqual(updateCommand.OthersJson, myUpdatedDetails.OthersJson);
            Assert.AreEqual(updateCommand.PhoneNumber, myUpdatedDetails.PhoneNumber);
            Assert.AreEqual(updateCommand.PictureUrl, myUpdatedDetails.PictureUrl);
            Assert.AreEqual("User", myUpdatedDetails.Role);
        }

        [Test]
        public async Task WhenIAddANewAdminToTheCompany_ThenICanRetrieveIt()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token = await CreateACompany(companyName);

            Guid userId;
            var firstname = NameHelper.GenerateEmployeeFirstName();
            var Lastname = NameHelper.GenerateEmployeeLastname();
            var employeeUserEmail = NameHelper.CreateEmail($"{firstname} {Lastname}", companyName);
            var jobtitle = NameHelper.GenerateJobTitle();
            var phone = NameHelper.GeneratePhoneNumber();
            var pictureUrl = NameHelper.GeneratePictureUrl();

            AddEmployeeCommand cmd = new AddEmployeeCommand
            {
                CorrelationId = Guid.NewGuid(),
                EmailAddress = employeeUserEmail,
                Firstname = firstname,
                Lastname = Lastname,
                JobTitle = jobtitle,
                PhoneNumber = phone,
                PictureUrl = pictureUrl,
                Role = "Admin",
                DateOfBirth = new DateTime(1981,9,15)
            };

            userId = await CompanyClientWrapper.CreateEmployeeAsync(cmd, token.access_token);

            Assert.AreNotEqual(Guid.Empty, userId);
            var employeeResult = await CompanyClientWrapper.GetEmployeeAsync(userId, token.access_token);

            Assert.AreEqual("Admin", employeeResult.Role);
            Assert.AreEqual(firstname, employeeResult.Firstname);
            Assert.AreEqual(Lastname, employeeResult.Lastname);
            Assert.AreEqual(jobtitle, employeeResult.JobTitle);
            Assert.AreEqual(phone, employeeResult.PhoneNumber);
            Assert.AreEqual(pictureUrl, employeeResult.PictureUrl);
        }

        [Test]
        public async Task WhenIAddANewTeamToTheCompany_ThenICanRetrieveIt()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token = await CreateACompany(companyName);

            var teamName = NameHelper.GenerateTeamName();

            var teamId = await CompanyClientWrapper.CreateTeamAsync(teamName, token.access_token);

            Assert.AreNotEqual(Guid.Empty, teamId);

            var teams = (await CompanyClientWrapper.GetTeamsAsync(token.access_token)).ToList();

            Assert.AreEqual(2, teams.Count);

            var team = await CompanyClientWrapper.GetTeamAsync(teamId, token.access_token);
            Assert.AreEqual(teamName, team.Name);
        }

        [Test]
        public async Task WhenIAddANewEmployeeToATeam_ThenICanRetrieveIt()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token2 = await CreateACompany(companyName);

            string employeeAdminEmail = NameHelper.CreateEmail("test2", companyName);
            Guid userId2 = await CreateEmployeeAsync(companyName, token2, "Admin", employeeAdminEmail);
            Thread.Sleep(100);
            var token = await  AuthClientWrapper.GetTokenAsync(employeeAdminEmail, userId2.GetHashCode().ToString());

            var teamName = NameHelper.GenerateTeamName();

            var teamId = await CompanyClientWrapper.CreateTeamAsync(teamName, token.access_token);

            Assert.AreNotEqual(Guid.Empty, teamId);

            var teams = (await CompanyClientWrapper.GetTeamsAsync(token.access_token)).ToList();

            Guid userId = await CreateEmployeeAsync(companyName, token);

            AddEmployeeToATeamCommand cmd2 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId,
                TeamId = teamId
            };


            var response = await CompanyClientWrapper.AddEmployeeToATeamAsync(cmd2, token.access_token);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var team = await CompanyClientWrapper.GetTeamAsync(teamId, token.access_token);
            Assert.AreEqual(teamName, team.Name);
            Assert.AreEqual(1, team.TeamMembers.Count);
        }

        [Test]
        public async Task WhenIRemoveANewEmployeeFromATeam_ThenICanRetrieveIt()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token = await CreateACompany(companyName);

            var teamName = NameHelper.GenerateTeamName();

            var teamId = await CompanyClientWrapper.CreateTeamAsync(teamName, token.access_token);

            Assert.AreNotEqual(Guid.Empty, teamId);

            var teams = (await CompanyClientWrapper.GetTeamsAsync(token.access_token)).ToList();

            Guid userId1 = await EmployeeFactory.CreateEmployeeAsync(companyName, token);
            Guid userId2 = await EmployeeFactory.CreateEmployeeAsync(companyName, token);
                                                                                         
            AddEmployeeToATeamCommand addToTeam1 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId1,
                TeamId = teamId
            };

            AddEmployeeToATeamCommand addToTeam2 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId2,
                TeamId = teamId
            };
            var response1 = await CompanyClientWrapper.AddEmployeeToATeamAsync(addToTeam1, token.access_token);
            var response2 = await CompanyClientWrapper.AddEmployeeToATeamAsync(addToTeam2, token.access_token);

            Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);

            var team = await CompanyClientWrapper.GetTeamAsync(teamId, token.access_token);
            Assert.AreEqual(teamName, team.Name);
            Assert.AreEqual(2, team.TeamMembers.Count);

            RemoveEmployeeFromATeamCommand removeFromTeam = new RemoveEmployeeFromATeamCommand
            {
                EmployeeId = userId1,
                TeamId = teamId
            };

            var response3 = await CompanyClientWrapper.RemoveEmployeeFromATeamCommand(removeFromTeam, token.access_token);

            team = await  CompanyClientWrapper.GetTeamAsync(teamId, token.access_token);
            Assert.AreEqual(teamName, team.Name);
            Assert.AreEqual(1, team.TeamMembers.Count);
            Assert.AreEqual(userId2, team.TeamMembers[0].Id);

            var employee = await CompanyClientWrapper.GetEmployeeAsync(userId1, token.access_token);
            Assert.IsNull(employee.MyTeam);
            Assert.AreEqual(companyName, employee.Company.Name);
        }

        [Test]
        public async Task WhenIAddANewManagerToATeamAndIRemoveTheManager_ThenTheTeamHasNoManager()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token = await CreateACompany(companyName);

            var teamName = NameHelper.GenerateTeamName();

            var teamId = await CompanyClientWrapper.CreateTeamAsync(teamName, token.access_token);

            Assert.AreNotEqual(Guid.Empty, teamId);

            var teams = (await CompanyClientWrapper.GetTeamsAsync(token.access_token)).ToList();

            Guid userId  =await CreateEmployeeAsync(companyName, token);
            Guid userId2 =await CreateEmployeeAsync(companyName, token);
            Guid userId3 =await CreateEmployeeAsync(companyName, token);
            Guid userId4 =await CreateEmployeeAsync(companyName, token);

            AddEmployeeToATeamCommand cmd2 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId,
                TeamId = teamId
            };
            AddEmployeeToATeamCommand cmd3 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId2,
                TeamId = teamId
            };
            AddEmployeeToATeamCommand cmd4 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId3,
                TeamId = teamId
            };

            var response = await CompanyClientWrapper.AddEmployeeToATeamAsync(cmd2, token.access_token);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            response = await CompanyClientWrapper.AddEmployeeToATeamAsync(cmd3, token.access_token);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            response = await CompanyClientWrapper.AddEmployeeToATeamAsync(cmd4, token.access_token);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            AddManagerCommand addManager1 = new AddManagerCommand
            {
                EmployeeId = userId4,
                TeamId = teamId
            };

            response = await CompanyClientWrapper.AddManager(addManager1, token.access_token);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var team = await CompanyClientWrapper.GetTeamAsync(teamId, token.access_token);
            var manager = await CompanyClientWrapper.GetEmployeeAsync(userId4, token.access_token);

            Assert.AreEqual(teamName, team.Name);
            Assert.AreEqual(3, team.TeamMembers.Count);
            Assert.AreEqual(userId4, team.Manager.Id);

            Assert.AreEqual(1, manager.ManagerOf.Count);
            Assert.AreEqual(teamId, manager.ManagerOf[0].Id);
            Assert.AreEqual(0, manager.TemporalManagerOf.Count);

            RemoveManagerCommand removeManager = new RemoveManagerCommand
            {
                TeamId = teamId
            };

            response = await CompanyClientWrapper.RemoveManager(removeManager, token.access_token);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            team = await CompanyClientWrapper.GetTeamAsync(teamId, token.access_token);
            manager = await CompanyClientWrapper.GetEmployeeAsync(userId4, token.access_token);

            Assert.AreEqual(teamName, team.Name);
            Assert.AreEqual(3, team.TeamMembers.Count);
            Assert.IsNull(team.Manager);

            Assert.AreEqual(0, manager.ManagerOf.Count);
            Assert.AreEqual(0, manager.TemporalManagerOf.Count);
        }

        [Test]
        public async Task WhenIAddANewTemporalManagerToATeamAndIRemoveTheTemporalManager_ThenTheTeamHasNoTemporalManager()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token = await CreateACompany(companyName);

            var teamName = NameHelper.GenerateTeamName();

            var teamId = await CompanyClientWrapper.CreateTeamAsync(teamName, token.access_token);

            Assert.AreNotEqual(Guid.Empty, teamId);

            var teams = (await CompanyClientWrapper.GetTeamsAsync(token.access_token)).ToList();

            Guid userId = await CreateEmployeeAsync(companyName, token);
            Guid userId2 =await CreateEmployeeAsync(companyName, token);
            Guid userId3 =await CreateEmployeeAsync(companyName, token);
            Guid userId4 = await CreateEmployeeAsync(companyName, token);

            AddEmployeeToATeamCommand cmd2 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId,
                TeamId = teamId
            };
            AddEmployeeToATeamCommand cmd3 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId2,
                TeamId = teamId
            };
            AddEmployeeToATeamCommand cmd4 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId3,
                TeamId = teamId
            };

            var response = await CompanyClientWrapper.AddEmployeeToATeamAsync(cmd2, token.access_token);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            response = await CompanyClientWrapper.AddEmployeeToATeamAsync(cmd3, token.access_token);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            response = await CompanyClientWrapper.AddEmployeeToATeamAsync(cmd4, token.access_token);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            AddTemporalManagerCommand addTemporalManager1 = new AddTemporalManagerCommand
            {
                EmployeeId = userId4,
                TeamId = teamId
            };

            response = await CompanyClientWrapper.AddTemporalManager(addTemporalManager1, token.access_token);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var team = await CompanyClientWrapper.GetTeamAsync(teamId, token.access_token);
            var manager = await CompanyClientWrapper.GetEmployeeAsync(userId4, token.access_token);

            Assert.AreEqual(teamName, team.Name);
            Assert.AreEqual(3, team.TeamMembers.Count);
            Assert.AreEqual(userId4, team.TemporalManager.Id);

            Assert.AreEqual(1, manager.TemporalManagerOf.Count);
            Assert.AreEqual(teamId, manager.TemporalManagerOf[0].Id);
            Assert.AreEqual(0, manager.ManagerOf.Count);

            RemoveTemporalManagerCommand removeTemporalManager = new RemoveTemporalManagerCommand
            {
                TeamId = teamId
            };

            response = await CompanyClientWrapper.RemoveTemporalManager(removeTemporalManager, token.access_token);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            team = await CompanyClientWrapper.GetTeamAsync(teamId, token.access_token);
            manager = await CompanyClientWrapper.GetEmployeeAsync(userId4, token.access_token);

            Assert.AreEqual(teamName, team.Name);
            Assert.AreEqual(3, team.TeamMembers.Count);
            Assert.IsNull(team.TemporalManager);

            Assert.AreEqual(0, manager.ManagerOf.Count);
            Assert.AreEqual(0, manager.TemporalManagerOf.Count);
        }

        [Test]
        public async Task WhenIAddAUserWithATeamICanRetrieveIt()
        {
            var companyName = NameHelper.GenerateCompanyName();
            TokenResponse token = await CreateACompany(companyName);

            var teamName = NameHelper.GenerateTeamName();
            var teamId = await CompanyClientWrapper.CreateTeamAsync(teamName, token.access_token);

            Assert.AreNotEqual(Guid.Empty, teamId);

            var firstname = NameHelper.GenerateEmployeeFirstName();
            var Lastname = NameHelper.GenerateEmployeeLastname();
            var employeeUserEmail = NameHelper.CreateEmail($"{firstname} {Lastname}", companyName);
            var jobtitle = NameHelper.GenerateJobTitle();
            var phone = NameHelper.GeneratePhoneNumber();
            var pictureUrl = NameHelper.GeneratePictureUrl();


            var response = await
                FacadeClientWrapper.AddEmployeeWithTeam(new CreateEmployeeWithTeamCommand()
                {
                    CorrelationId = Guid.NewGuid(),
                    EmailAddress = employeeUserEmail,
                    Firstname = firstname,
                    Lastname = Lastname,
                    JobTitle = jobtitle,
                    PhoneNumber = phone,
                    PictureUrl = pictureUrl,
                    Role = "User",
                    TeamId = teamId,
                    DateOfBirth = new DateTime(1981, 9, 15)
                }, token.access_token);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var team = await CompanyClientWrapper.GetTeamAsync(teamId, token.access_token);
            Assert.AreEqual(teamName, team.Name);
            Assert.AreEqual(1, team.TeamMembers.Count);
        }

        private static async Task<TokenResponse> CreateACompany(string companyName)
        {
            var ownerName = NameHelper.GenerateEmployeeName();
            var employeeEmail = NameHelper.CreateEmail(ownerName, companyName);
            CreateCompanyCommand command = new CreateCompanyCommand()
            {
                CompanyName = companyName,
                AdminEmailAddress = employeeEmail,
                FirstName = ownerName,
                MiddleName = "",
                LastName = "Bolson",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
                OwnerId = Guid.NewGuid()
            };
            await CompanyClientWrapper.CreateCompanyAsync(command);
            Thread.Sleep(100);
            var token = await AuthClientWrapper.GetTokenAsync(employeeEmail, command.OwnerId.GetHashCode().ToString());
            return token;
        }

        private static async Task<Guid> CreateEmployeeAsync(string companyName, TokenResponse token, string role = null, string email = null)
        {
            var firstname = NameHelper.GenerateEmployeeFirstName();
            var Lastname = NameHelper.GenerateEmployeeLastname();
            var employeeUserEmail = email ?? NameHelper.CreateEmail($"{firstname} {Lastname}", companyName);
            var jobtitle = NameHelper.GenerateJobTitle();
            var phone = NameHelper.GeneratePhoneNumber();
            var pictureUrl = NameHelper.GeneratePictureUrl();

            var random = new Random();
            AddEmployeeCommand cmd = new AddEmployeeCommand
            {
                CorrelationId = Guid.NewGuid(),
                EmailAddress = employeeUserEmail,
                Firstname = firstname,
                Lastname = Lastname,
                JobTitle = jobtitle,
                PhoneNumber = phone,
                PictureUrl = pictureUrl,
                Role = role,
                DateOfBirth = new DateTime(random.Next(1970, 1996), random.Next(1, 11), random.Next(1, 28), 0, 0, 0),
                OthersJson = $"{{'passport':'{GeneratePassport()}'}}"
            };

            return await CompanyClientWrapper.CreateEmployeeAsync(cmd, token.access_token);
        }

        private static string GeneratePassport()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(7);
        }

        private static UpdateEmployeeCommand GenerateUpdateEmployeeCommand()
        {
            var firstname = NameHelper.GenerateEmployeeFirstName();
            var Lastname = NameHelper.GenerateEmployeeLastname();
            var employeeUserEmail = NameHelper.CreateEmail($"{firstname} {Lastname}", "personal.com");
            var jobtitle = NameHelper.GenerateJobTitle();
            var phone = NameHelper.GeneratePhoneNumber();
            var pictureUrl = NameHelper.GeneratePictureUrl();

            var random = new Random();
            UpdateEmployeeCommand cmd = new UpdateEmployeeCommand
            {
                CorrelationId = Guid.NewGuid(),
                EmailAddress = employeeUserEmail,
                Firstname = firstname,
                Lastname = Lastname,
                JobTitle = jobtitle,
                PhoneNumber = phone,
                PictureUrl = pictureUrl,
                Role = "Admin",
                DateOfBirth = new DateTime(random.Next(1970, 1996), random.Next(1, 11), random.Next(1, 28), 0, 0, 0),
                OthersJson = $"{{'passport':'{GeneratePassport()}'}}"
            };

            return cmd;
        }
    }
}
