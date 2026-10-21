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
using DaysOn.Common.Events.Dtos;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class FacadeHolidayAdminControllerTest
    {
       private Guid _employeeId;
       private TokenResponse _tokenUser;
       private TokenResponse _tokenAdmin;
       private Guid _holidayId;

        [SetUp]
        public void Setup()
        {
            SetupAsync().Wait();
        }

        private async Task SetupAsync()
        {
            var companyName = NameHelper.GenerateCompanyName();
            _tokenAdmin = await CompanyFactory.CreateCompanyAsync(companyName);

            string employeeUserEmail = NameHelper.CreateEmail("test1", companyName);
            _employeeId = await EmployeeFactory.CreateEmployeeAsync(companyName, _tokenAdmin, "User", employeeUserEmail);
            Thread.Sleep(150);
            Assert.AreNotEqual(Guid.Empty, _employeeId);

            _tokenUser = await AuthClientWrapper.GetTokenAsync(employeeUserEmail, _employeeId.GetHashCode().ToString());

            _holidayId = Guid.NewGuid();
        }

        [Test]
        public async Task WhenAnAdminCancelAHolidayRequest_ThenCanSeeTheStatus()
        {
            var command = new AddHolidayCommand()
            {
                Id = _holidayId,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = _employeeId,
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Marbella"
            };

           await FacadeClientWrapper.AdminCreateHolidayAsync(command, _tokenAdmin.access_token);

            var response = await FacadeClientWrapper.AdminCancelHolidayAsync(new EmployeeHolidayCommand
            {
                HolidayId = _holidayId,
                EmployeeId = _employeeId
            }, _tokenAdmin.access_token);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task WhenAnAdminApproveAHolidayRequest_ThenCanSeeTheStatus()
        {
            var command = new AddHolidayCommand()
            {
                Id = _holidayId,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = _employeeId,
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Marbella"
            };

            await FacadeClientWrapper.AdminCreateHolidayAsync(command, _tokenAdmin.access_token);
            
            var response = await FacadeClientWrapper.AdminApprovetHolidayAsync(new EmployeeHolidayCommand {
                EmployeeId = _employeeId,
                HolidayId = _holidayId
            }, _tokenAdmin.access_token);

            var result = await (await FacadeClientWrapper.AdminGetHolidayAsync(_employeeId, _holidayId, _tokenAdmin.access_token)).GetHolidayRequestAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(Status.Aproved, result.Status);
        }

        [Test]
        public async Task WhenAnAdminCreateAnApprovedHoliday_ThenCanSeeTheStatus()
        {
            var command = new AddHolidayCommand()
            {
                Id = _holidayId,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = _employeeId,
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Marbella",
                NumberOfDays = 4,
                Status = Status.Aproved
            };

            await FacadeClientWrapper.AdminCreateHolidayAsync(command, _tokenAdmin.access_token);

            var result =await (await FacadeClientWrapper.AdminGetHolidayAsync(_employeeId, _holidayId, _tokenAdmin.access_token)).GetHolidayRequestAsync();
            
            Assert.AreEqual(Status.Aproved, result.Status);
        }

        [Test]
        public async Task WhenAnAdminRejectAHolidayRequest_ThenCanSeeTheStatus()
        {
            var command = new AddHolidayCommand()
            {
                Id = _holidayId,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = _employeeId,
                From = Now().AddDays(50),
                To = Now().AddDays(55),
                Notes = "Best holidays in Marbella",
                NumberOfDays = 4
            };

            await FacadeClientWrapper.AdminCreateHolidayAsync(command, _tokenAdmin.access_token);

            var response = await FacadeClientWrapper.AdminRejectHolidayAsync(new EmployeeHolidayCommand
            {
                EmployeeId = _employeeId,
                HolidayId = _holidayId
            }, _tokenAdmin.access_token);

            var result = await (await FacadeClientWrapper.AdminGetHolidayAsync(_employeeId, _holidayId, _tokenAdmin.access_token)).GetHolidayRequestAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(Status.Rejected, result.Status);
        }

        [Test]
        public async Task WhenAnUserCreateHoliday_ThenAdminReceiveTheHolidayInformation()
        {
            Guid holidayId = Guid.NewGuid();
            var command = new AddHolidayCommand()
            {
                Id = holidayId,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = _employeeId,
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Granada",
            };

            await FacadeClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);

            Thread.Sleep(250);
            var result = await (await FacadeClientWrapper.AdminGetHolidayAsync(_employeeId, holidayId, _tokenAdmin.access_token)).GetHolidayRequestAsync();

            Assert.AreEqual(command.Id, result.Id);
            Assert.AreEqual(command.EmployeeId, result.EmployeeId);
            Assert.AreEqual(command.From.Year, result.Year);
        }

        [Test]
        public async Task WhenAnAdminRequestAllHolidaysPerEmployeeAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var commands = GetListHolidayCommands();
            foreach (var command in commands)
            {
                await FacadeClientWrapper.AdminCreateHolidayAsync(command, _tokenAdmin.access_token,0);
            }
            Thread.Sleep(300);
            var result = await (await FacadeClientWrapper.AdminGetHolidaysAsync(_employeeId, _tokenAdmin.access_token)).GetListHolidayRequestAsync();

            Assert.AreEqual(commands.Count, result.Count);
            Assert.IsTrue(result.Any(x => x.Id == commands[0].Id));
            Assert.IsTrue(result.Any(x => x.Id == commands[1].Id));
            Assert.IsTrue(result.Any(x => x.Id == commands[1].Id));
            Assert.IsTrue(result.Any(x => x.EmployeeId == commands[0].EmployeeId));
            Assert.IsTrue(result.Any(x => x.EmployeeId == commands[1].EmployeeId));
            Assert.IsTrue(result.Any(x => x.EmployeeId == commands[2].EmployeeId));
        }

        [Test]
        public async Task WhenAnAdminRequestHolidaysPerEmployeeByYearAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var commands = GetListHolidayCommands2();
            foreach (var command in commands)
            {
                await FacadeClientWrapper.AdminCreateHolidayAsync(command, _tokenAdmin.access_token, 20);
            }
            Thread.Sleep(250);

            var result = await (await FacadeClientWrapper.AdminGetHolidaysByYearAsync(_employeeId, 2020, _tokenAdmin.access_token)).GetListHolidayRequestAsync();

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
