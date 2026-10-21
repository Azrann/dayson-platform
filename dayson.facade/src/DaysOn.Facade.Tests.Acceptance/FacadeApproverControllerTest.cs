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
using DaysOn.HolidaysBc.Models.Dtos;
using Newtonsoft.Json;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class FacadeApproverControllerTest
    {
        private Guid _employeeId;
        private Guid _employeeId2;
        private Guid _employeeIdApprover;
        private Guid _teamId;
        private TokenResponse _tokenAdmin;

        private string companyName;

        [SetUp]
        public void Initialize()
        {
            companyName = NameHelper.GenerateCompanyName();
            var ownerName = NameHelper.GenerateEmployeeName();
            var employeeEmail = NameHelper.CreateEmail(ownerName, companyName);
            CreateCompanyCommand command = new CreateCompanyCommand()
            {
                CompanyName = companyName,
                AdminEmailAddress = employeeEmail,
                FirstName = ownerName,
                LastName = "Bolson",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
                OwnerId = Guid.NewGuid()
            };
            Guid companyId = CompanyClientWrapper.CreateCompanyAsync(command).Result;

            Thread.Sleep(150);
            _tokenAdmin = AuthClientWrapper.GetTokenAsync(employeeEmail, command.OwnerId.GetHashCode().ToString()).Result;
            if (string.IsNullOrEmpty(_tokenAdmin.access_token))
            {
                _tokenAdmin = AuthClientWrapper.GetTokenAsync(employeeEmail, command.OwnerId.GetHashCode().ToString()).Result;
            }

            var approverUserEmail = EmployeeFactory.CreateEmployee(companyName, _tokenAdmin, out _employeeIdApprover);
            var employeeUserEmail = EmployeeFactory.CreateEmployee(companyName, _tokenAdmin, out _employeeId);
            var employeeUserEmail2 = EmployeeFactory.CreateEmployee(companyName, _tokenAdmin, out _employeeId2);

            Thread.Sleep(150);
            Assert.AreNotEqual(Guid.Empty, _employeeIdApprover);
            Assert.AreNotEqual(Guid.Empty, _employeeId);


            _teamId = CompanyClientWrapper
                .CreateTeamAsync(NameHelper.GenerateEmployeeName() + "Team", _tokenAdmin.access_token).Result;

            var responseAddEmployeeToTeam = CompanyClientWrapper.AddEmployeeToATeamAsync(new AddEmployeeToATeamCommand()
            {
                TeamId = _teamId,
                EmployeeId = _employeeId
            }, _tokenAdmin.access_token).Result;

            var responseAddEmployeeAprovverToTeam = CompanyClientWrapper.AddEmployeeToATeamAsync(
                new AddEmployeeToATeamCommand()
                {
                    TeamId = _teamId,
                    EmployeeId = _employeeIdApprover
                }, _tokenAdmin.access_token).Result;

            Thread.Sleep(150);
        }

        [Test]
        public async Task ApproverWorkflow()
        {
            var emptyResponse = await FacadeClientWrapper.GetAllApproversAsync(_tokenAdmin.access_token);

            Assert.AreEqual(HttpStatusCode.OK, emptyResponse.StatusCode);

            var response = await FacadeClientWrapper.AddApprover(_employeeIdApprover, _tokenAdmin.access_token);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseGet = await FacadeClientWrapper.GetAllApproversAsync(_tokenAdmin.access_token);

            List<IdNameDto> approvers =
                JsonConvert.DeserializeObject<List<IdNameDto>>(await responseGet.Content.ReadAsStringAsync());

            Assert.AreEqual(1, approvers.Count);
            Assert.AreEqual(_employeeIdApprover, approvers[0].Id);


            await FacadeClientWrapper.AddEmployeeToApproverAsync(new AddEmployeeToApproverCommand()
            {
                ApproverId = _employeeIdApprover,
                EmployeeId = _employeeId
            }, _tokenAdmin.access_token);

            var responseEmployee = (await FacadeClientWrapper.GetEmployeeGlobalRules(_employeeId, _tokenAdmin.access_token));
            var responseString = (await responseEmployee.Content.ReadAsStringAsync());

            EmployeeDto employee = JsonConvert.DeserializeObject<EmployeeDto>(responseString);

            Assert.IsNotNull(employee.Approver);

            var responseApproverEmployee = (await FacadeClientWrapper.GetEmployeeGlobalRules(_employeeIdApprover, _tokenAdmin.access_token));
            var responseApproverString = (await responseApproverEmployee.Content.ReadAsStringAsync());

            EmployeeDto employeeApprover = JsonConvert.DeserializeObject<EmployeeDto>(responseApproverString);

            Assert.IsTrue(employeeApprover.IsApprover);

            var responseAdd = await FacadeClientWrapper.AddEmployeeToApproverAsync(new AddEmployeeToApproverCommand()
            {
                ApproverId = _employeeIdApprover,
                EmployeeId = _employeeId2
            }, _tokenAdmin.access_token);
            Assert.AreEqual(HttpStatusCode.OK, responseAdd.StatusCode);

            responseAdd = await FacadeClientWrapper.GetEmployeesForApprover(_employeeIdApprover,_tokenAdmin.access_token);
            responseString = await responseAdd.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<ApproversResponse>(responseString);

            responseAdd = await FacadeClientWrapper.RemoveEmployeeFromApproverAsync(new AddEmployeeToApproverCommand()
            {
                ApproverId = _employeeIdApprover,
                EmployeeId = _employeeId
            }, _tokenAdmin.access_token);

            responseEmployee = (await FacadeClientWrapper.GetEmployeeGlobalRules(_employeeId, _tokenAdmin.access_token));
            responseString = (await responseEmployee.Content.ReadAsStringAsync());

            employee = JsonConvert.DeserializeObject<EmployeeDto>(responseString);
            Assert.IsNull(employee.Approver);

        }
    }

    public class ApproversResponse
    {
        public List<IdNameDto> employees { get; set; }
    }
}
