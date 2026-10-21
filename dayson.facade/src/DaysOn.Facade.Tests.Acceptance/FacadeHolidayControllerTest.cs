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

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class FacadeHolidayControllerTest
    {
        private Guid _employeeId;
        private Guid _holidayId;
        private TokenResponse _tokenUser;

        [SetUp]
        public void Setup()
        {
            SetupAsync().Wait();
        }

        private async Task SetupAsync()
        {
            var companyName = NameHelper.GenerateCompanyName();
            var token = await CompanyFactory.CreateCompanyAsync(companyName);

            string employeeUserEmail = NameHelper.CreateEmail("test1", companyName);
            _employeeId = await EmployeeFactory.CreateEmployeeAsync(companyName, token, "User", employeeUserEmail);
            Thread.Sleep(150);
            Assert.AreNotEqual(Guid.Empty, _employeeId);

            _tokenUser = await AuthClientWrapper.GetTokenAsync(employeeUserEmail, _employeeId.GetHashCode().ToString());
            _holidayId = Guid.NewGuid();

            Thread.Sleep(150);
        }

        [Test]
        public async Task WhenICancelAHolidayRequest_ThenCanSeeTheStatus()
        {
            var command = new AddHolidayCommand()
            {
                Id = _holidayId,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = _employeeId,
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Marbella",
                NumberOfDays = 4
            };

            await FacadeClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);
            Thread.Sleep(250);

            var response = await FacadeClientWrapper.CancelHolidayAsync(new EmployeeHolidayCommand
            {
                HolidayId = _holidayId,
                EmployeeId = _employeeId
            }, _tokenUser.access_token);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task WhenIGetAHolidayByIdAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var command = new AddHolidayCommand()
            {
                Id = _holidayId,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = _employeeId,
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Granada",
            };

            await FacadeClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);
            Thread.Sleep(200);

            var result = await (await FacadeClientWrapper.GetHolidayAsync(_employeeId, _holidayId, _tokenUser.access_token)).GetHolidayRequestAsync();

            Assert.AreEqual(command.Id, result.Id);
            Assert.AreEqual(command.EmployeeId, result.EmployeeId);
            Assert.AreEqual(command.From.Year, result.Year);
        }

        [Test]
        public async Task  WhenIRequestAllHolidaysPerEmployeeAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var commands = GetListHolidayCommands();
            foreach (var command in commands)
            {
                await FacadeClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);
            }

            Thread.Sleep(300);

            var result = await (await FacadeClientWrapper.GetHolidaysAsync(_employeeId, _tokenUser.access_token)).GetListHolidayRequestAsync();

            Assert.AreEqual(commands.Count, result.Count);
            Assert.IsTrue(result.Any(x => x.Id == commands[0].Id));
            Assert.IsTrue(result.Any(x => x.Id == commands[1].Id));
            Assert.IsTrue(result.Any(x => x.Id == commands[2].Id));
            Assert.IsTrue(result.Any(x => x.EmployeeId == commands[0].EmployeeId));
            Assert.IsTrue(result.Any(x => x.EmployeeId == commands[1].EmployeeId));
            Assert.IsTrue(result.Any(x => x.EmployeeId == commands[2].EmployeeId));
        }

        [Test]
        public async Task WhenIRequestHolidaysPerEmployeeByYearAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var commands = GetListHolidayCommands2();
            foreach (var command in commands)
            {
                await FacadeClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);
            }

            Thread.Sleep(400);
            var result = await (await FacadeClientWrapper.GetHolidaysByYearAsync(_employeeId, 2020, _tokenUser.access_token)).GetListHolidayRequestAsync();

            Assert.AreEqual(2, result.Count);
        }

        [TearDown]
        public void CleanUp()
        {
            
        }

        private List<AddHolidayCommand> GetListHolidayCommands()
        {
            return new List<AddHolidayCommand>()
            {
                new AddHolidayCommand()
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = Guid.NewGuid(),
                    EmployeeId = _employeeId,
                    From = Now().AddDays(30),
                    To = Now().AddDays(35),
                    Notes = "Best holidays in Granada 5",
                    NumberOfDays = 5
                },
                new AddHolidayCommand()
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = Guid.NewGuid(),
                    EmployeeId = _employeeId,
                    From = Now().AddDays(10),
                    To = Now().AddDays(18),
                    Notes = "Best holidays in Granada 8",
                    NumberOfDays = 8
                },
                new AddHolidayCommand()
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = Guid.NewGuid(),
                    EmployeeId = _employeeId,
                    From = Now().AddDays(50),
                    To = Now().AddDays(52),
                    Notes = "Best holidays in Granada 2",
                    NumberOfDays = 2
                }
            };
        }

        private List<AddHolidayCommand> GetListHolidayCommands2()
        {
            return new List<AddHolidayCommand>()
            {
                new AddHolidayCommand()
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = Guid.NewGuid(),
                    EmployeeId = _employeeId,
                    From = Now().AddDays(30),
                    To = Now().AddDays(35),
                    Notes = "Best holidays in Granada 5",
                    NumberOfDays = 5
                },
                new AddHolidayCommand()
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = Guid.NewGuid(),
                    EmployeeId = _employeeId,
                    From = new DateTime(2020,2,2),
                    To = new DateTime(2020,2,10),
                    Notes = "Best holidays in Granada 8",
                    NumberOfDays = 8
                },
                new AddHolidayCommand()
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = Guid.NewGuid(),
                    EmployeeId = _employeeId,
                    From = new DateTime(2020,3,2),
                    To = new DateTime(2020,3, 4),
                    Notes = "Best holidays in Granada 2",
                    NumberOfDays = 2
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
