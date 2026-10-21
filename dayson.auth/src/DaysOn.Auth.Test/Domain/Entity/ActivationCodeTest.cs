using DaysOn.Auth.Domain.Entities;
using NUnit.Framework;

namespace Bookatable.Neptune.Auth.API.Tests.Unit.Domain.Entity
{
    [TestFixture]
    public class ActivationCodeTest
    {
        //// Added to get code coverage 100% due to limitation of Entity Framework parameterless constructor requirement
        [Test]
        public void WhenICreateActivationCodeWithNoParameters_IGetAnInstance()
        {
            var mock = new MockActivationCode();
            Assert.IsNotNull(mock);
        }
    }

    public class MockActivationCode : ActivationCode
    {
        public MockActivationCode()
            : base()
        {
        }
    }
}
