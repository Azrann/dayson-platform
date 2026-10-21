using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using DaysOn.Common.DataAccess;
using DaysOn.Common.Domain.Ioc;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.Functions.Api.App_Start;
using DaysOn.Functions.BoundedContext.Services;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;
using Status = DaysOn.Common.Events.Dtos.Status;

namespace DaysOn.Functions.Test.Integration.Services
{
    [TestFixtureAttribute]
    public class CreateHolidayServiceTest
    {
        private ICreateHolidayService _createHolidayService;
        private IEmployeeService _employeeService;
        private IHolidayService _holidayService;
        private string _id = "07fc6855-76b2-4541-9569-9f9344ec227c";
        private string _employeeId = "123c6855-76b2-4541-9569-9f9344ec2123";
        [SetUp]
        public void Initialize()
        {
            WindsorConfigurator.Configure(ContainerManager.Container);
            _createHolidayService = ContainerManager.Container.Resolve<ICreateHolidayService>();
            _employeeService = ContainerManager.Container.Resolve<IEmployeeService>();
            _holidayService = ContainerManager.Container.Resolve<IHolidayService>();
        }

        [Test]
        public async void WhenICreateHolidayRequest_ICanRetrieveIt()
        {
            Employee employee = new Employee(new Guid(_employeeId))
            {
                CompanyId = new Guid(),
                Email = "user@example.invalid",
                CreatedOn = DateTime.Now,
                YearStartDay = DateTime.Now
            };
            _employeeService.AddEmployee(employee);
            await _createHolidayService.CreateHolidayRequest(new AddHolidayRequestEvent()
            {
                CompanyId = employee.CompanyId,
                CorrelationId = Guid.NewGuid(),
                EmployeeId = new Guid(_employeeId),
                From = DateTime.Now.AddDays(10),
                To = DateTime.Now.AddDays(15),
                Notes = "Here is a note",
                Status = Status.Requested,
                Id = new Guid(_id),
                SubscriberId = Guid.NewGuid()
            },5, DateTime.Now.AddDays(10));

            var holidayRequest = await _holidayService.GetHolidayRequest(new Guid(_id), new Guid(_employeeId));

            Assert.IsNotNull(holidayRequest);
        }

        [TearDown]
        public void TearDown()
        {
            _holidayService.RemoveHolidayRequest(new Guid(_id), new Guid(_employeeId));
            _employeeService.RemoveEmployee(new Guid(_employeeId));
        }
    }
}
