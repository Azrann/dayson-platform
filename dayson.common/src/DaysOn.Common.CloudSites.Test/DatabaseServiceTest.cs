using System;
using System.Linq;
using DaysOn.Common.CloudSites;
using DaysOn.Common.CloudSites.Models;
using NUnit.Framework;

namespace DaysOn.Common.CloudDatabases.Test
{
    [TestFixture]
    public class DatabaseServiceTest
    {
        [Test]
        public void WhenIRequestDatabasesIReceiveAList()
        {
            var result = DatabaseService.GetDatabases().Result;
            Assert.IsTrue(result.Count > 4);
        }
        
        [Test]
        public void ICanCreateADatabase()
        {
            var result = DatabaseService.Create("testhrnow",DatabasePlan.free, DatabaseType.mssql).Result;
            Assert.IsTrue(result);
        }

        [Test]
        public void ICanDeleteADatabase()
        {
            var Databases = DatabaseService.GetDatabases().Result;
            var DatabaseId = Databases.First(x => x.name == "testhrnow").id;

            var result = DatabaseService.Delete(DatabaseId).Result;
            Assert.IsTrue(result);
        }

        [Test]
        public void ICanRetrieveDbUsers()
        {
            var Databases = DatabaseService.GetDatabases().Result;
            var DatabaseId = Databases.First(x => x.name == "testhrnow").id;

            var result = DatabaseService.GetUsers(DatabaseId).Result;
            Assert.IsTrue(result.Count > 0);
        }
    }
}
