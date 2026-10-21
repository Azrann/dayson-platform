using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.Facade.Common;
using DaysOn.Facade.Models.WebModels.Holiday;
using DaysOn.Facade.Tests.Acceptance.Helper;
using DaysOn.HolidaysBc.Models.Commands;
using DaysOn.HolidaysBc.Models.Dtos;
using NUnit.Framework;

namespace DaysOn.Facade.Tests.Acceptance.InternalServices
{
    [TestFixture]
    public class EmployeeControllerTest
    {
        private Guid _employeeId;
        private TokenResponse _tokenUser;
        private TokenResponse _tokenAdmin;
        private Guid holidayId;

        [SetUp]
        public void Setup()
        {
            SetupAsync().Wait();
        }

        public async Task SetupAsync()
        {
            holidayId = Guid.NewGuid();
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
            Guid companyId = await CompanyClientWrapper.CreateCompanyAsync(command);

            Thread.Sleep(150);
            _tokenAdmin = await AuthClientWrapper.GetTokenAsync(employeeEmail, command.OwnerId.GetHashCode().ToString());
            string employeeUserEmail = NameHelper.CreateEmail("test", companyName);

            _employeeId  = await CreateEmployeeAsync(companyName, _tokenAdmin, null, employeeUserEmail);
            Thread.Sleep(150);
            Assert.AreNotEqual(Guid.Empty, _employeeId);

            _tokenUser = await AuthClientWrapper.GetTokenAsync(employeeUserEmail, _employeeId.GetHashCode().ToString());
            Thread.Sleep(150);
            Assert.That(_tokenUser.access_token, Is.Not.Null.Or.Empty);
        }

        [Test]
        public async Task WhenIUpdateEmployeeGlobalRules_ThenICanRetrieveThem()
        {
            var employeeNewGlobalRules = new GlobalRulesDto
            {
                MaxDaysCarriedOverNextYear = 3,
                DefaultNumberOfHolidays = 30,
                MaxConsecutiveDaysOff = 15,
                MinDaysAdvanceNoticeHolidays = 5,
                YearStartDay = new DateTime(2000, 2, 2),
                WorkingDaysOfTheWeek = SetWeekendWorking()
            };

            Thread.Sleep(100);
            var response = (await GlobalRulesClientWrapper.SetEmployeeGlobalRules(_employeeId, employeeNewGlobalRules, _tokenAdmin.access_token));

            var employeeGlobalRules = (await GlobalRulesClientWrapper.GetEmployeeGlobalRules(_employeeId, _tokenAdmin.access_token)).GetEmployeeGlobalRules();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(3, employeeGlobalRules.MaxDaysCarriedOverNextYear);
            Assert.AreEqual(30, employeeGlobalRules.HolidayEntitlement);
            Assert.AreEqual(15, employeeGlobalRules.MaxConsecutiveDaysOff);
            Assert.AreEqual(5, employeeGlobalRules.MinDaysAdvanceNoticeHolidays);
            Assert.AreEqual(2, employeeGlobalRules.YearStartDay.Day);
            Assert.AreEqual(2, employeeGlobalRules.YearStartDay.Month);
            Assert.AreEqual(false, employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Monday]);
            Assert.AreEqual(true, employeeGlobalRules.WorkingDaysOfTheWeek[DayOfWeek.Sunday]);
        }

        [Test]
        public async Task WhenICancelAHolidayRequest_ThenCanSeeTheStatus()
        {
            var command = new AddHolidayRequestCommand
            {
                Id = holidayId,
                CorrelationId = Guid.NewGuid(),
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Marbella",
            };
            await HolidayClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);

            Thread.Sleep(250);
            var response = await HolidayClientWrapper.CancelHolidayAsync(new EmployeeHolidayCommand
            {
                HolidayId = holidayId,
                EmployeeId = _employeeId
            }, _tokenUser.access_token);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task WhenIChangeStatusToCancel_ThenCanSeeTheStatus()
        {
            var command = new AddHolidayRequestCommand
            {
                Id = holidayId,
                CorrelationId = Guid.NewGuid(),
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Marbella",
            };
            await HolidayClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);

            Thread.Sleep(250);

            var response2 = await HolidayClientWrapper.ChangeHolidayStatusAsync(new Common.Dtos.WebHoliday.ChangeHolidayStatusRequest
            {
                HolidayId = holidayId,
                Status = HolidayStatus.Canceled
            }, _tokenUser.access_token);

            var response = await HolidayClientWrapper.GetHolidayAsync(_employeeId, holidayId, _tokenUser.access_token);
            var result = response.GetHolidayRequest();
            Assert.AreEqual("Canceled", result.Status.ToString());
        }

        [Test]
        public async Task WhenIChangeStatusToCancelWithAdmin_ThenCanSeeTheStatus()
        {
            var command = new AddHolidayRequestCommand
            {
                Id = holidayId,
                CorrelationId = Guid.NewGuid(),
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Marbella",
            };
            await HolidayClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);

            Thread.Sleep(250);

            var response2 = await HolidayClientWrapper.ChangeHolidayStatusAsync(new Common.Dtos.WebHoliday.ChangeHolidayStatusRequest
            {
                HolidayId = holidayId,
                Status = HolidayStatus.Canceled
            }, _tokenAdmin.access_token);

            var response = await HolidayClientWrapper.GetHolidayAsync(_employeeId, holidayId, _tokenUser.access_token);
            var result = response.GetHolidayRequest();
            Assert.AreEqual("Canceled", result.Status.ToString());
        }

        [Test]
        public async Task WhenIGetAHolidayByIdAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var command = new AddHolidayRequestCommand
            {
                Id = holidayId,
                CorrelationId = Guid.NewGuid(),
                From = Now().AddDays(30),
                To = Now().AddDays(35),
                Notes = "Best holidays in Granada",
            };

            var createHolidayResponse = await HolidayClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);
            Thread.Sleep(250);
            var response = await HolidayClientWrapper.GetHolidayAsync(_employeeId, holidayId, _tokenUser.access_token);
            var result = response.GetHolidayRequest();

            Assert.AreEqual(command.Id, result.Id);
            Assert.AreEqual(_employeeId, result.EmployeeId);
            Assert.AreEqual(command.From.Year, result.Year);
        }

        [Test]
        public async Task WhenIRequestAllHolidaysPerEmployeeAndHolidayExists_ThenIReceiveTheHolidayInformation()
        {
            var commands = GetListHolidayCommands();
            foreach (var command in commands)
            {
                await HolidayClientWrapper.CreateHolidayAsync(command, _tokenUser.access_token);
            }
            Thread.Sleep(250);
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
            Thread.Sleep(150);
            var result = (await HolidayClientWrapper.GetHolidaysByYearAsync(_employeeId, 2020, _tokenUser.access_token)).GetListHolidayRequest();

            Assert.AreEqual(2, result.Count);
        }

        [TearDown]
        public void CleanUp()
        {
            
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
                    From = Now().AddDays(30),
                    To = Now().AddDays(35),
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

        private static Dictionary<DayOfWeek, bool> SetWeekendWorking()
        {
            Dictionary<DayOfWeek, bool> result = new Dictionary<DayOfWeek, bool>();
            result.Add(DayOfWeek.Monday, false);
            result.Add(DayOfWeek.Tuesday, false);
            result.Add(DayOfWeek.Wednesday, false);
            result.Add(DayOfWeek.Thursday, false);
            result.Add(DayOfWeek.Friday, false);
            result.Add(DayOfWeek.Saturday, true);
            result.Add(DayOfWeek.Sunday, true);
            return result;
        }

        private DateTime Now()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day);
        }
    }
}
