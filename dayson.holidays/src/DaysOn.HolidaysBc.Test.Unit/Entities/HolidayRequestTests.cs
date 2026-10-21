using System;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using NUnit.Framework;

namespace DaysOn.HolidaysBc.Test.Unit.Entities
{
    [TestFixture]
    public class HolidayRequestTests
    {
        private HolidayRequest _holidayRequest;
        [SetUp]
        public void Initialize()
        {
            _holidayRequest = new HolidayRequest(Guid.NewGuid())
            {
                EmployeeId = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                Status = BoundedContext.Entities.Status.Requested,
                From = new DateTime(2025, 2, 15),
                To = new DateTime(2025, 2, 25),
            };
        }

        [Test]
        [TestCase(1,1,1000,2025)]
        [TestCase(14, 2, 1000, 2025)]
        [TestCase(15, 2, 1000, 2025)]
        [TestCase(16, 3, 1000, 2024)]
        [TestCase(26, 2, 1000, 2024)]
        [TestCase(31, 12, 1000, 2024)]
        public void WhenICallSetYear_ThenYearIsSet(int day, int month, int year, int result)
        {
            _holidayRequest.SetYear(new DateTime(year,month,day));

            Assert.AreEqual(result, _holidayRequest.Year);
        }

        [Test]
        [TestCase(16, 2, 1000, 2024)]
        [TestCase(25, 2, 1000, 2024)]
        public void WhenICallSetYearAndHolidayRequestBetween2Years_ThenIGetAnException(int day, int month, int year, int result)
        {
            Assert.That(() => _holidayRequest.SetYear(new DateTime(year, month, day)),
                Throws.TypeOf<ArgumentException>());
            Assert.AreEqual(result, _holidayRequest.Year);
        }

        [Test]
        [TestCase(26, 12, 1000, 2024)]
        [TestCase(31, 12, 1000, 2024)]
        [TestCase(1, 1, 1000, 2024)]
        public void WhenICallSetYearAndHolidayRequestBetween2Years_ThenIGetAnException2(int day, int month, int year, int result)
        {
            _holidayRequest.From = new DateTime(2025, 12, 25);
            _holidayRequest.To = new DateTime(2026, 1, 7);
            Assert.That(() => _holidayRequest.SetYear(new DateTime(year, month, day)),
                Throws.TypeOf<ArgumentException>());
        }
    }
}
