using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.Facade.Common;
using DaysOn.Facade.Tests.Acceptance.Helper;
using DaysOn.HolidaysBc.Models.Commands;
using NUnit.Framework;

namespace DaysOn.Facade.Tests.Acceptance.InternalServices
{
    [TestFixture]
    public class HolidayControllerTest
    {
       private Guid _employeeId;
       private TokenResponse _tokenUser;
        private Guid _holidayId;

       [SetUp]
        public void Initialize()
        {
            var companyName = NameHelper.GenerateCompanyName();
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

            Thread.Sleep(100);
            var token = AuthClientWrapper.GetTokenAsync(employeeEmail, command.OwnerId.GetHashCode().ToString()).Result;

            var employeeUserEmail = EmployeeFactory.CreateEmployee(companyName, token, out _employeeId);

            Assert.AreNotEqual(Guid.Empty, _employeeId);
            Thread.Sleep(100);
            _tokenUser = AuthClientWrapper.GetTokenAsync(employeeUserEmail, _employeeId.GetHashCode().ToString()).Result;
            _holidayId = Guid.NewGuid();
        }

        [Test]
        public async Task WhenICancelAHolidayRequest_ThenCanSeeTheStatus()
        {
            var command = new AddHolidayRequestCommand()
            {
                Id = _holidayId,
                CorrelationId = Guid.NewGuid(),
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Marbella"
            };

            Thread.Sleep(150);
            await HolidayClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);

            Thread.Sleep(200);
            var response = await HolidayClientWrapper.CancelHolidayAsync(new EmployeeHolidayCommand
            {
                HolidayId = _holidayId,
                EmployeeId = _employeeId
            }, _tokenUser.access_token);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task WhenIGetAHolidayByIdAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var command = new AddHolidayRequestCommand
            {
                Id = _holidayId,
                CorrelationId = Guid.NewGuid(),
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Granada"
            };

            Thread.Sleep(150);
            await HolidayClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);

            Thread.Sleep(200);
            var result = (await HolidayClientWrapper.GetHolidayAsync(_employeeId, _holidayId, _tokenUser.access_token)).GetHolidayRequest();

            Assert.AreEqual(command.Id, result.Id);
            Assert.AreEqual(_employeeId, result.EmployeeId);
            Assert.AreEqual(command.From.Year, result.Year);
        }

        [Test]
        public async Task WhenIRequestAllHolidaysPerEmployeeAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var commands = GetListHolidayCommands();
            Thread.Sleep(150);
            foreach (var command in commands)
            {
                await HolidayClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);
            }

            Thread.Sleep(300);
            var result = (await HolidayClientWrapper.GetHolidaysAsync(_employeeId, _tokenUser.access_token)).GetListHolidayRequest();

            Assert.AreEqual(commands.Count, result.Count);
            Assert.IsTrue(result.Any(x => x.Id == commands[0].Id));
            Assert.IsTrue(result.Any(x => x.Id == commands[1].Id));
            Assert.IsTrue(result.Any(x => x.Id == commands[2].Id));
            Assert.IsTrue(result.Any(x => x.EmployeeId == _employeeId));
            Assert.IsTrue(result.Any(x => x.EmployeeId == _employeeId));
            Assert.IsTrue(result.Any(x => x.EmployeeId == _employeeId));
        }

        [Test]
        public async Task WhenIRequestHolidaysPerEmployeeByYearAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var commands = GetListHolidayCommands2();
            
            foreach (var command in commands)
            {
                await HolidayClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);
            }
            Thread.Sleep(400);
            var result = (await HolidayClientWrapper.GetHolidaysByYearAsync(_employeeId, 2020, _tokenUser.access_token)).GetListHolidayRequest();

            Assert.AreEqual(2, result.Count);
        }

        [TearDown]
        public void CleanUp()
        {
            
        }

        private List<AddHolidayRequestCommand> GetListHolidayCommands()
        {
            return new List<AddHolidayRequestCommand>
            {
                new AddHolidayRequestCommand
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = Guid.NewGuid(),
                    From = Now().AddDays(30),
                    To = Now().AddDays(35),
                    Notes = "Best holidays in Granada 5"
                },
                new AddHolidayRequestCommand
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = Guid.NewGuid(),
                    From = Now().AddDays(10),
                    To = Now().AddDays(18),
                    Notes = "Best holidays in Granada 8"
                },
                new AddHolidayRequestCommand
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = Guid.NewGuid(),
                    From = Now().AddDays(50),
                    To = Now().AddDays(52),
                    Notes = "Best holidays in Granada 2"
                }
            };
        }

        private List<AddHolidayRequestCommand> GetListHolidayCommands2()
        {
            return new List<AddHolidayRequestCommand>
            {
                new AddHolidayRequestCommand
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = Guid.NewGuid(),
                    From = new DateTime(2021,3,2),
                    To = new DateTime(2021,3,2),
                    Notes = "Best holidays in Granada 5"
                },
                new AddHolidayRequestCommand
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = Guid.NewGuid(),
                    From = new DateTime(2020,2,2),
                    To = new DateTime(2020,2,10),
                    Notes = "Best holidays in Granada 8"
                },
                new AddHolidayRequestCommand
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = Guid.NewGuid(),
                    From = new DateTime(2020,3,2),
                    To = new DateTime(2020,3, 4),
                    Notes = "Best holidays in Granada 2"
                }
            };
        }

        private DateTime Now()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day);
        }
    }
}
