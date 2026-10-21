using NUnit.Framework;
using System;
using DaysOn.Common.DataAccess;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Data;

namespace DaysOn.HolidaysBc.BoundedContext.Services.Tests
{
    [TestFixture]
    public class OpenClosedDatesServiceTests
    {
        OpenClosedDatesService service;
        Guid companyId1;
        Guid companyId2;
        Guid companyId3;
        Guid companyId4;
        
        [SetUp]
        public void Initialize()
        {
            var db = new DapperNoSql<OpenClosedDates>();
            service = new OpenClosedDatesService(db);
            db.CreateTableIfNotExists(NoSqlTables.OpenClosedDates);

            companyId1 = Guid.NewGuid();
            companyId2 = Guid.NewGuid();
            companyId3 = Guid.NewGuid();
            companyId4 = Guid.NewGuid();
        }

        [Test]
        public void WhenICreateOpenCloseDate_ThenICanRetrieveThemByYear()
        {
            service.AddClosedDay(companyId1, new DateTime(2015, 1, 1));
            var result = service.GetByYear(companyId1, 2015);
            Assert.AreEqual(1, result.ClosedDays.Count);
        }

        [Test]
        public void WhenICreateOpenCloseDate_ThenICanRetrieveThemAll()
        {
            service.AddClosedDay(companyId1, new DateTime(2014, 1, 1));
            var result = service.GetAllByYear(companyId1, 2014);
            Assert.AreEqual(1, result.ClosedDays.Count);
        }

        [Test]
        public void WhenICreateOpenCloseDateWithRecurrent_ThenICanRetrieveThemAll()
        {
            service.AddClosedDay(companyId1, new DateTime(2013, 1, 1));
            service.AddRecurrentClosedDay(companyId1, new DateTime(2000, 1, 1));
            var result = service.GetAllByYear(companyId1, 2013);
            Assert.AreEqual(2, result.ClosedDays.Count);
        }

        [Test]
        public void WhenICreateOpenCloseDateWithHalfDays_ThenICanRetrieveThemByYear()
        {
            service.AddClosedHalfDay(companyId2, new DateTime(2015, 1, 1));
            var result = service.GetByYear(companyId2, 2015);
            Assert.AreEqual(1, result.ClosedHalfDays.Count);
            Assert.AreEqual(0, result.ClosedDays.Count);
        }

        [Test]
        public void WhenICreateOpenCloseDateWithHalfDays_ThenICanRetrieveThemAll()
        {
            service.AddClosedHalfDay(companyId2, new DateTime(2014, 1, 1));
            var result = service.GetAllByYear(companyId2, 2014);
            Assert.AreEqual(1, result.ClosedHalfDays.Count);
            Assert.AreEqual(0, result.ClosedDays.Count);
        }

        [Test]
        public void WhenICreateOpenCloseDateWithHalfDaysAndRecurrent_ThenICanRetrieveThemAll()
        {
            service.AddClosedHalfDay(companyId2, new DateTime(2013, 1, 1));
            service.AddRecurrentClosedHalfDay(companyId2, new DateTime(2000, 1, 1));
            var result = service.GetAllByYear(companyId2, 2013);
            Assert.AreEqual(2, result.ClosedHalfDays.Count);
            Assert.AreEqual(0, result.ClosedDays.Count);
        }

        [Test]
        public void WhenICreateOpenCloseDateComplex_ThenICanRetrieveThemAll()
        {
            service.AddClosedHalfDay(companyId3, new DateTime(2013, 1, 1));
            service.AddClosedDay(companyId3, new DateTime(2013, 2, 2));
            service.AddClosedDay(companyId3, new DateTime(2013, 2, 3));
            service.AddRecurrentClosedHalfDay(companyId3, new DateTime(2000, 1, 2));
            service.AddRecurrentClosedHalfDay(companyId3, new DateTime(1990, 4, 2));

            var result = service.GetAllByYear(companyId3, 2013);
            Assert.AreEqual(3, result.ClosedHalfDays.Count);
            Assert.AreEqual(2, result.ClosedDays.Count);
        }

        [Test]
        public void WhenICreateOpenCloseDateComplex_ThenICanRetrieveRecurrent()
        {
            service.AddClosedHalfDay(companyId4, new DateTime(2013, 1, 1));
            service.AddClosedDay(companyId4, new DateTime(2013, 2, 2));
            service.AddClosedDay(companyId4, new DateTime(2013, 2, 3));
            service.AddRecurrentClosedHalfDay(companyId4, new DateTime(2000, 1, 2));
            service.AddRecurrentClosedHalfDay(companyId4, new DateTime(1990, 4, 2));
            service.AddRecurrentClosedDay(companyId4, new DateTime(1990, 3, 2));
            var result = service.GetRecurrent(companyId4);
            Assert.AreEqual(2, result.ClosedHalfDays.Count);
            Assert.AreEqual(1, result.ClosedDays.Count);
        }

        [Test]
        public void WhenIModifyOpenCloseDate_ThenICanSaveIt()
        {
            var companyId = Guid.NewGuid();

            service.AddClosedDay(companyId, new DateTime(2015, 1, 1));
            var openCloseDates = service.GetByYear(companyId, 2015);

            openCloseDates.ClosedDays.Add(new DateTime(2015, 2, 2));
            openCloseDates.ClosedHalfDays.Add(new DateTime(2015, 2, 3));
            service.AddClosedDays(openCloseDates, 2015);

            var result = service.GetByYear(companyId, 2015);
            Assert.AreEqual(2, result.ClosedDays.Count);
            Assert.AreEqual(1, result.ClosedHalfDays.Count);
        }

        [Test]
        public void WhenIModifyRecurrentOpenCloseDate_ThenICanSaveIt()
        {
            var companyId = Guid.NewGuid();

            service.AddRecurrentClosedDay(companyId, new DateTime(2015, 1, 1));
            var openCloseDates = service.GetRecurrent(companyId);

            openCloseDates.ClosedDays.Add(new DateTime(2015, 2, 2));
            openCloseDates.ClosedHalfDays.Add(new DateTime(2015, 2, 3));
            service.AddRecurrentClosedDays(openCloseDates);

            var result = service.GetRecurrent(companyId);
            Assert.AreEqual(2, result.ClosedDays.Count);
            Assert.AreEqual(1, result.ClosedHalfDays.Count);
        }
    }
}