using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.Facade.Common;
using DaysOn.Facade.Tests.Acceptance.Helper;
using DaysOn.HolidaysBc.Models.Commands;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class HolidayBrainControllerTest
    {
       private Guid _employeeId;
       private TokenResponse _tokenUser;

        [SetUp]
        public void Setup()
        {
            SetupAsync().Wait();
        }

        private async Task SetupAsync()
        {
            var companyName = NameHelper.GenerateCompanyName();
            var _tokenAdmin = await CompanyFactory.CreateCompanyAsync(companyName);

            string employeeUserEmail = NameHelper.CreateEmail("employee1", companyName);
            _employeeId = await EmployeeFactory.CreateEmployeeAsync(companyName, _tokenAdmin, null, employeeUserEmail);

            Thread.Sleep(100);
            Assert.AreNotEqual(Guid.Empty, _employeeId);

            _tokenUser = AuthClientWrapper.GetTokenAsync(employeeUserEmail, _employeeId.GetHashCode().ToString()).Result;
        }

        [Test]
        public async Task WhenIRequestNumberOfBusinessDays_IReceive200OkResponse()
        {
            var subscriberId = new Guid("79e265ec-b842-481d-8c7b-0d35319cb785");
            var command = new GetBusinessDaysCommand
            {
                Id = Guid.NewGuid(),
                CorrelationId = Guid.NewGuid(),
                SubscriberId = subscriberId,
                From = DateTime.Now.AddDays(30),
                To = DateTime.Now.AddDays(35)                
            };

            var response = (await HolidayBrainClientWrapper.GetBusinessDaysCommandAsync(command, _tokenUser.access_token));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
