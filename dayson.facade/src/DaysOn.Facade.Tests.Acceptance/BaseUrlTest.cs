using DaysOn.Facade.Tests.Acceptance.Helper;
using NUnit.Framework;

namespace DaysOn.Facade.Tests.Acceptance
{
    [TestFixture]
    public class BaseUrlTest
    {
        [Test]
        public void WhenISerializeBaseUrl_ThenIGetTheRightString()
        {
            Assert.AreEqual("AuthServer",BaseUrl.AuthServer.ToString());
        }
    }
}
