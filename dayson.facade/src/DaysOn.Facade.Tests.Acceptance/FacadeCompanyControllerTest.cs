using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.Facade.Common;
using DaysOn.Facade.Tests.Acceptance.Helper;
using DaysOn.HolidaysBc.Models.Commands;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Dtos;
using DaysOn.Facade.Models;
using DaysOn.HolidaysBc.Models.Dtos;
using Newtonsoft.Json;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class FacadeCompanyControllerTest
    {
        [Test]
        public async Task WhenICallGetTeamOverview_ThenICanRetrieveTheRightData()
        {
            var companyName = NameHelper.GenerateCompanyName();
            var token = await CompanyFactory.CreateCompanyAsync(companyName);

            var teamName = NameHelper.GenerateTeamName();

            var teamId = await CompanyClientWrapper.CreateTeamAsync(teamName, token.access_token);

            Assert.AreNotEqual(Guid.Empty, teamId);

            var teams = (await CompanyClientWrapper.GetTeamsAsync(token.access_token)).ToList();

            Guid userId1;
            Guid userId2;
            Guid userId3;
            Guid userId4;

            userId1 = await EmployeeFactory.CreateEmployeeAsync(companyName, token);
            userId2 = await EmployeeFactory.CreateEmployeeAsync(companyName, token);
            userId3 = await EmployeeFactory.CreateEmployeeAsync(companyName, token);
            userId4 = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "Admin");

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
            AddEmployeeToATeamCommand addToTeam3 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId3,
                TeamId = teamId
            };

            AddEmployeeToATeamCommand addToTeam4 = new AddEmployeeToATeamCommand
            {
                EmployeeId = userId4,
                TeamId = teamId
            };
            var response1 = await CompanyClientWrapper.AddEmployeeToATeamAsync(addToTeam1, token.access_token);
            var response2 = await CompanyClientWrapper.AddEmployeeToATeamAsync(addToTeam2, token.access_token);
            var response3 = await CompanyClientWrapper.AddEmployeeToATeamAsync(addToTeam3, token.access_token);
            var response4 = await CompanyClientWrapper.AddEmployeeToATeamAsync(addToTeam4, token.access_token);
            var teamApproverResponse = await ApproverClientWrapper.AddApprover(userId2, token.access_token);

            Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response4.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, teamApproverResponse.StatusCode);

            var teamOverviewResult = await FacadeClientWrapper.GetTeamsOverview(token.access_token);

            Assert.AreEqual(HttpStatusCode.OK, teamOverviewResult.StatusCode);

            string response = await teamOverviewResult.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TeamOverviewResult>(response);

            Assert.AreEqual(2, result.Teams.Count);
            Assert.AreEqual(4, result.Teams.First(x => x.Id == teamId).TeamMembers.Count);
            Assert.IsTrue(result.Teams.First(x => x.Id == teamId).TeamMembers.Any(x => x.Id == userId2 && x.HasApprovalRights));
            Assert.AreEqual(1, result.Teams.First(x => x.Id == teamId).TeamMembers.Count(x => x.HasApprovalRights));         
        }

        [Test]
        public void WhenICallGetTeamOverviewForAExistingCompany_ThenICanRetrieveTheRightData()
        {
            var token =
                "REPLACE_WITH_YOUR_VALUE";
            var teamOverviewResult = FacadeClientWrapper.GetTeamsOverview(token).Result;

            Assert.AreEqual(HttpStatusCode.OK, teamOverviewResult.StatusCode);

            var result = JsonConvert.DeserializeObject<TeamOverviewResult>(teamOverviewResult.Content.ReadAsStringAsync().Result);

        }

        [Test]
        public async Task WhenICallSearchForAExistingCompany_ThenICanRetrieveTheRightData()
        {
            var token =
                "REPLACE_WITH_YOUR_VALUE";
            var teamId = new Guid("e8a57bb8-0125-43e8-a77f-f6be00c57368");
            var listEmployee = await CompanyClientWrapper.SearchAsync(new SearchUsersRequest()
            {
                TeamIds = new List<Guid>() {teamId},
                Text = "Don"

            }, token);

            Assert.AreEqual(1, listEmployee.Count);
        }

        private DateTime Now()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day);
        }
    }

    public class TeamOverviewResult
    {
        public bool CanAddNewTeams { get; set; }
        public List<TeamOverviewDto> Teams { get; set; }
    }
}
