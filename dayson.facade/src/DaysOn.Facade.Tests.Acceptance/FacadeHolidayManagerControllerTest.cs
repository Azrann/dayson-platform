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
using DaysOn.HolidaysBc.Models.Results;
using Newtonsoft.Json;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class FacadeHolidayManagerControllerTest
    {
        private Guid _employeeId;
        private Guid _managerId;
        private TokenResponse _tokenUser;
        private TokenResponse _tokenAdmin;
        private TokenResponse _tokenManager;

        private string _companyName;

        [SetUp]
        public void Setup()
        {
            SetupAsync().Wait();
        }

        private async Task SetupAsync()
        {
            _companyName = NameHelper.GenerateCompanyName();
            _tokenAdmin = await CompanyFactory.CreateCompanyAsync(_companyName);

            string managerUserEmail = NameHelper.CreateEmail("manager1", _companyName);
            _managerId = await EmployeeFactory.CreateEmployeeAsync(_companyName, _tokenAdmin, null, managerUserEmail);
            string employeeUserEmail = NameHelper.CreateEmail("user1", _companyName);
            _employeeId = await EmployeeFactory.CreateEmployeeAsync(_companyName, _tokenAdmin, null, employeeUserEmail);

            Thread.Sleep(150);
            Assert.AreNotEqual(Guid.Empty, _managerId);
            Assert.AreNotEqual(Guid.Empty, _employeeId);

            var teamId = await CompanyClientWrapper.CreateTeamAsync(NameHelper.GenerateEmployeeName() + "Team", _tokenAdmin.access_token);
            
            var responseAddEmployeeToTeam = await CompanyClientWrapper.AddEmployeeToATeamAsync(new AddEmployeeToATeamCommand()
            {
                TeamId = teamId,
                EmployeeId = _employeeId
            }, _tokenAdmin.access_token);
            var responseAddManager = await CompanyClientWrapper.AddManager(new AddManagerCommand
            {
                TeamId = teamId,
                EmployeeId = _managerId
            }, _tokenAdmin.access_token);

            Thread.Sleep(150);
            _tokenManager = await AuthClientWrapper.GetTokenAsync(managerUserEmail, _managerId.GetHashCode().ToString());
            _tokenUser = await AuthClientWrapper.GetTokenAsync(employeeUserEmail, _employeeId.GetHashCode().ToString());
        }

        [Test]
        public async Task WhenAnManagerCancelAHolidayRequest_ThenCanSeeTheStatus()
        {
            var holidayId = Guid.NewGuid();
            var command = new AddHolidayCommand()
            {
                Id = holidayId,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = _employeeId,
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Marbella"
            };
            Thread.Sleep(150);

            await FacadeClientWrapper.ManagerCreateHolidayAsync(command, _tokenManager.access_token);
            
            Thread.Sleep(250);

            var response = await FacadeClientWrapper.ManagerCancelHolidayAsync(new EmployeeHolidayCommand
            {
                HolidayId = holidayId,
                EmployeeId = _employeeId
            }, _tokenManager.access_token);

            var result = await (await FacadeClientWrapper.ManagerGetHolidayAsync(_employeeId, holidayId, _tokenManager.access_token)).GetHolidayRequestAsync();

            Assert.AreEqual(command.Id, result.Id);
            Assert.AreEqual(command.EmployeeId, result.EmployeeId);
            Assert.AreEqual(command.From.Year, result.Year);
            Assert.AreEqual(Status.Canceled, result.Status);
        }

        [Test]
        public async Task WhenAnManagerGetAHolidayByIdAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var holidayId = Guid.NewGuid();
            var command = new AddHolidayCommand()
            {
                Id = holidayId,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = _employeeId,
                From = new DateTime(2021, 3, 1),
                To = new DateTime(2021, 3, 5),
                Notes = "Best holidays in Granada",
            };

            Thread.Sleep(150);
            await FacadeClientWrapper.ManagerCreateHolidayAsync(command, _tokenManager.access_token);
            Thread.Sleep(250);

            var result = await (await FacadeClientWrapper.ManagerGetHolidayAsync(_employeeId, holidayId, _tokenManager.access_token)).GetHolidayRequestAsync();

            Assert.AreEqual(command.Id, result.Id);
            Assert.AreEqual(command.EmployeeId, result.EmployeeId);
            Assert.AreEqual(5, result.NumberOfDays);
            Assert.AreEqual(command.From.Year, result.Year);
        }

        [Test]
        public async Task WhenAnManagerGetAHolidayByIdAndEmployeeIsNotInHisTeam_ThenIReceiveAnError()
        {
            Guid employeeId2 = await EmployeeFactory.CreateEmployeeAsync(_companyName, _tokenAdmin, "User");
            var teamId2 = await CompanyClientWrapper.CreateTeamAsync(NameHelper.GenerateEmployeeName() + "Team", _tokenAdmin.access_token).ConfigureAwait(false);
            Thread.Sleep(150);
            var responseAddEmployeeToTeam = await CompanyClientWrapper.AddEmployeeToATeamAsync(new AddEmployeeToATeamCommand()
            {
                TeamId = teamId2,
                EmployeeId = employeeId2
            }, _tokenAdmin.access_token).ConfigureAwait(false);
            Guid holidayId2 = Guid.NewGuid();
            var command = new AddHolidayCommand()
            {
                Id = holidayId2,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = _employeeId,
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Granada",
            };

            await FacadeClientWrapper.AdminCreateHolidayAsync(command, _tokenAdmin.access_token).ConfigureAwait(false);

            var result = (await FacadeClientWrapper.ManagerGetHolidayAsync(employeeId2, holidayId2, _tokenManager.access_token));
            CommandResult stringResult = JsonConvert.DeserializeObject<CommandResult>(await result.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.BadRequest,result.StatusCode);
            Assert.AreEqual($"Employee with Id {employeeId2} is not in my Team.", stringResult.Errors.First());
            Assert.IsFalse(stringResult.Succeeded);
        }

        [Test]
        public async Task WhenAnUserCreateHoliday_ThenManagerReceiveTheHolidayInformation()
        {
            var holidayId = Guid.NewGuid();
            var command = new AddHolidayCommand()
            {
                Id = holidayId,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = _employeeId,
                From = new DateTime(2021, 3, 5),
                To = new DateTime(2021, 3, 8),
                Notes = "Best holidays in Granada",
            };

            await FacadeClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);
            Thread.Sleep(250);

            var result = await (await FacadeClientWrapper.ManagerGetHolidayAsync(_employeeId, holidayId, _tokenManager.access_token)).GetHolidayRequestAsync();

            Assert.AreEqual(command.Id, result.Id);
            Assert.AreEqual(command.EmployeeId, result.EmployeeId);
            Assert.AreEqual(command.From.Year, result.Year);
            Assert.AreEqual(2, result.NumberOfDays);
        }

        [Test]
        public async Task WhenAnUserCreateHoliday_ThenManagerCanApproveTheHolidayRequest()
        {
            var holidayId = Guid.NewGuid();
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

            await (await FacadeClientWrapper.ManagerApprovetHolidayAsync(new EmployeeHolidayCommand
            {
                EmployeeId = _employeeId,
                HolidayId = holidayId
            }, _tokenManager.access_token)).GetGuidAsync();
            var result = await (await FacadeClientWrapper.ManagerGetHolidayAsync(_employeeId, holidayId, _tokenManager.access_token)).GetHolidayRequestAsync();

            Assert.AreEqual(command.Id, result.Id);
            Assert.AreEqual(command.EmployeeId, result.EmployeeId);
            Assert.AreEqual(command.From.Year, result.Year);
            Assert.AreEqual(Status.Aproved, result.Status);
        }

        [Test]
        public async Task WhenAnUserCreateHoliday_ThenManagerCanRejectTheHolidayRequest()
        {
            var holidayId = Guid.NewGuid();
            var command = new AddHolidayCommand()
            {
                Id = holidayId,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = _employeeId,
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Granada",
                NumberOfDays = 5
            };

            await FacadeClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);
            Thread.Sleep(250);

            await (await FacadeClientWrapper.ManagerRejectHolidayAsync(new EmployeeHolidayCommand
            {
                EmployeeId = _employeeId,
                HolidayId = holidayId
            },_tokenManager.access_token)).GetGuidAsync();

            var result = await (await FacadeClientWrapper.ManagerGetHolidayAsync(_employeeId, holidayId, _tokenManager.access_token)).GetHolidayRequestAsync();

            Assert.AreEqual(command.Id, result.Id);
            Assert.AreEqual(command.EmployeeId, result.EmployeeId);
            Assert.AreEqual(command.From.Year, result.Year);
            Assert.AreEqual(Status.Rejected, result.Status);
        }

        [Test]
        public async Task WhenAnManagerRequestAllHolidaysPerEmployeeAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var commands = GetListHolidayCommands();
            foreach (var command in commands)
            {
                await FacadeClientWrapper.ManagerCreateHolidayAsync(command, _tokenManager.access_token);
            }

            Thread.Sleep(400);
            var result =await (await FacadeClientWrapper.ManagerGetHolidaysAsync(_employeeId, _tokenManager.access_token)).GetListHolidayRequestAsync();

            Assert.AreEqual(commands.Count, result.Count);
            Assert.IsTrue(result.Any(x => x.Id == commands[0].Id));
            Assert.IsTrue(result.Any(x => x.Id == commands[1].Id));
            Assert.IsTrue(result.Any(x => x.Id == commands[1].Id));
            Assert.IsTrue(result.Any(x => x.EmployeeId == commands[0].EmployeeId));
            Assert.IsTrue(result.Any(x => x.EmployeeId == commands[1].EmployeeId));
            Assert.IsTrue(result.Any(x => x.EmployeeId == commands[2].EmployeeId));
        }

        [Test]
        public async Task WhenAnManagerRequestHolidaysPerEmployeeByYearAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var commands = GetListHolidayCommands2();
            foreach (var command in commands)
            {
                await FacadeClientWrapper.ManagerCreateHolidayAsync(command, _tokenManager.access_token);
            }

            Thread.Sleep(400);
            var result = await (await FacadeClientWrapper.ManagerGetHolidaysByYearAsync(_employeeId, 2020, _tokenManager.access_token)).GetListHolidayRequestAsync();

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
            return new DateTime(now.Year,now.Month,now.Day);
        }
    }
}
