using System;
using DaysOn.Facade.Common;
using DaysOn.Facade.Tests.Acceptance.Helper;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Models.Commands;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class FacadeOpenClosedDatesControllerTest
    {
        private TokenResponse _tokenAdmin;

        [SetUp]
        public void Setup()
        {
            SetupAsync().Wait();
        }

        private async Task SetupAsync()
        {
            var companyName = NameHelper.GenerateCompanyName();
            _tokenAdmin = await CompanyFactory.CreateCompanyAsync(companyName);
        }

        [Test]
        public async Task WhenIAddDays_ICanRetrieveThemByYear()
        {
            var globalRules = (await FacadeOpenCloseDatesClientWrapper.AddClosedDays(Factory.GetAddDaysCommand(2014), _tokenAdmin.access_token));

            var result = (await FacadeOpenCloseDatesClientWrapper.GetByYear(2014, _tokenAdmin.access_token)).GetOpenClosedDatesDto();

            Assert.AreEqual(3, result.ClosedDays.Count);
            Assert.AreEqual(4, result.ClosedHalfDays.Count);
        }

        [Test]
        public async Task WhenIAddDays_ICanRetrieveThemAllByYear()
        {
            var responseAddDays = (await FacadeOpenCloseDatesClientWrapper.AddClosedDays(Factory.GetAddDaysCommand(2013), _tokenAdmin.access_token));
            var responseAddRecurrentDays = (await FacadeOpenCloseDatesClientWrapper.AddRecurrentClosedDays(Factory.GetRecurrentDaysCommand(), _tokenAdmin.access_token));

            var result = (await FacadeOpenCloseDatesClientWrapper.GetAllByYear(2013, _tokenAdmin.access_token)).GetOpenClosedDatesDto();

            Assert.AreEqual(6, result.ClosedDays.Count);
            Assert.AreEqual(8, result.ClosedHalfDays.Count);
        }

        [Test]
        public async Task WhenIAddRecurrentDays_ICanRetrieveThem()
        {
            var responseAddRecurrentDays = (await FacadeOpenCloseDatesClientWrapper.AddRecurrentClosedDays(Factory.GetRecurrentDaysCommand(), _tokenAdmin.access_token));

            var result = (await FacadeOpenCloseDatesClientWrapper.GetRecurrent(_tokenAdmin.access_token)).GetOpenClosedDatesDto();

            Assert.AreEqual(3, result.ClosedDays.Count);
            Assert.AreEqual(4, result.ClosedHalfDays.Count);
        }

        [Test]
        public async Task WhenIAddRecurrentClosedDay_ICanRetrieveIt()
        {
            var responseAddRecurrentDays = (await FacadeOpenCloseDatesClientWrapper.AddRecurrentClosedDay(Factory.GetAddDayCommand(2012), _tokenAdmin.access_token));

            var result = (await FacadeOpenCloseDatesClientWrapper.GetRecurrent(_tokenAdmin.access_token)).GetOpenClosedDatesDto();

            Assert.AreEqual(1, result.ClosedDays.Count);
            Assert.AreEqual(0, result.ClosedHalfDays.Count);
        }
        
        [Test]
        public async Task WhenIAddRecurrentClosedHalfDay_ICanRetrieveIt()
        {
            var responseAddRecurrentDays = (await FacadeOpenCloseDatesClientWrapper.AddRecurrentClosedHalfDay(Factory.GetAddDayCommand(2011), _tokenAdmin.access_token));

            var result = (await FacadeOpenCloseDatesClientWrapper.GetRecurrent(_tokenAdmin.access_token)).GetOpenClosedDatesDto();

            Assert.AreEqual(0, result.ClosedDays.Count);
            Assert.AreEqual(1, result.ClosedHalfDays.Count);
        }

        [Test]
        public async Task WhenIAddClosedDay_ICanRetrieveIt()
        {
            var responseAddDays = (await FacadeOpenCloseDatesClientWrapper.AddClosedDay(Factory.GetAddDayCommand(2012), _tokenAdmin.access_token));

            var result = (await FacadeOpenCloseDatesClientWrapper.GetByYear(2012, _tokenAdmin.access_token)).GetOpenClosedDatesDto();

            Assert.AreEqual(1, result.ClosedDays.Count);
            Assert.AreEqual(0, result.ClosedHalfDays.Count);
        }

        [Test]
        public async Task WhenIAddClosedHalfDay_ICanRetrieveIt()
        {
            var responseAddDays = (await FacadeOpenCloseDatesClientWrapper.AddClosedHalfDay(Factory.GetAddDayCommand(2011), _tokenAdmin.access_token));

            var result = (await FacadeOpenCloseDatesClientWrapper.GetByYear(2011, _tokenAdmin.access_token)).GetOpenClosedDatesDto();

            Assert.AreEqual(0, result.ClosedDays.Count);
            Assert.AreEqual(1, result.ClosedHalfDays.Count);
        }

        [Test]
        public async Task WhenIAddDaysOneByOne_ICanRetrieveThem()
        {
            var responseAddDays1 = (await FacadeOpenCloseDatesClientWrapper.AddClosedDay(Factory.GetAddDayCommandByDay(1, 5), _tokenAdmin.access_token));
            var responseAddDays2 = (await FacadeOpenCloseDatesClientWrapper.AddClosedDay(Factory.GetAddDayCommandByDay(1, 6), _tokenAdmin.access_token));
            var responseAddDays3 = (await FacadeOpenCloseDatesClientWrapper.AddClosedDay(Factory.GetAddDayCommandByDay(12, 25), _tokenAdmin.access_token));
            var responseAddDays4 = (await FacadeOpenCloseDatesClientWrapper.AddClosedHalfDay(Factory.GetAddDayCommandByDay(12, 24), _tokenAdmin.access_token));
            var responseAddDays5 = (await FacadeOpenCloseDatesClientWrapper.AddRecurrentClosedHalfDay(Factory.GetAddDayCommandByDay(12, 31), _tokenAdmin.access_token));
            var responseAddDays6 = (await FacadeOpenCloseDatesClientWrapper.AddRecurrentClosedDay(Factory.GetAddDayCommandByDay(1, 1), _tokenAdmin.access_token));

            var result = (await FacadeOpenCloseDatesClientWrapper.GetAllByYear(2010, _tokenAdmin.access_token)).GetOpenClosedDatesDto();

            Assert.AreEqual(4, result.ClosedDays.Count);
            Assert.AreEqual(2, result.ClosedHalfDays.Count);
        }
    }
}
