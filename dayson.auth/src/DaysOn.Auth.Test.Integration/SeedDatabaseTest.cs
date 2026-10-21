using DaysOn.Auth.Web;
using NUnit.Framework;

namespace DaysOn.Auth.Test.Integration
{
    [TestFixture]
    public class SeedDatabaseTest
    {
        [Test]
        public void WhenISeedDatabaseIDontGetExceptions()
        {
            DatabaseSeed.Run();
        }
    }
}
