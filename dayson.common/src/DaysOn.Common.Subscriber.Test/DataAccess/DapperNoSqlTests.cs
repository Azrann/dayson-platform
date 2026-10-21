using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DaysOn.Common.DataAccess.Tests
{
    [TestFixture]
    public class DapperNoSqlTests
    {
        private INoSqlDb<Person> noSqlDb;
        [OneTimeSetUp]
        public void Initialize()
        {
            noSqlDb = new DapperNoSql<Person>();
            noSqlDb.CreateTableIfNotExists("TestPerson");
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            noSqlDb.DeleteTable("TestPerson");
        }

        [Test]
        public void WhenIAddAValue_ThenICanRetrieveIt()
        {
            Person Alberto = new Person
            {
                Name = "Alberto",
                Age = 35,
                DayOfBirth = new DateTime(1981,9,15)
            };
            noSqlDb.AddOrUpdate("TestPerson", "1", Alberto);

            var result = noSqlDb.GetByKey("TestPerson", "1");

            Assert.AreEqual(Alberto.Name, result.Name);
            Assert.AreEqual(Alberto.Age, result.Age);
            Assert.AreEqual(Alberto.DayOfBirth, result.DayOfBirth);
        }

        [Test]
        public void WhenIUpdateAValue_ThenGetUpdated()
        {
            Person Alberto = new Person
            {
                Name = "Alberto",
                Age = 35,
                DayOfBirth = new DateTime(1981, 9, 15)
            };
            noSqlDb.AddOrUpdate("TestPerson", "2", Alberto);
            Alberto.Age = 36;

            noSqlDb.AddOrUpdate("TestPerson", "2", Alberto);

            var result = noSqlDb.GetByKey("TestPerson", "2");

            Assert.AreEqual(Alberto.Name, result.Name);
            Assert.AreEqual(Alberto.Age, result.Age);
            Assert.AreEqual(Alberto.DayOfBirth, result.DayOfBirth);
        }

        [Test]
        public void WhenIDeleteAValue_ThenIRetrieveNull()
        {
            Person Alberto = new Person
            {
                Name = "Alberto",
                Age = 35,
                DayOfBirth = new DateTime(1981, 9, 15)
            };
            noSqlDb.AddOrUpdate("TestPerson", "3", Alberto);
            noSqlDb.Delete("TestPerson", "3");

            var result = noSqlDb.GetByKey("TestPerson", "3");

            Assert.IsNull(result);
        }

        [Test]
        public void WhenIAddAComplexValue_ThenICanRetrieveIt()
        {
            var complexNoSqlDb = new DapperNoSql<Team>();
            complexNoSqlDb.CreateTableIfNotExists("TestTeam");

            Team team = GetTeam();

            complexNoSqlDb.AddOrUpdate("TestTeam", "2", team);

            var result = complexNoSqlDb.GetByKey("TestTeam", "2");
            
            Assert.AreEqual(team.Name, result.Name);
            Assert.AreEqual(team.Members[0].Name, result.Members[0].Name);
            Assert.AreEqual(team.Members[0].DayOfBirth, result.Members[0].DayOfBirth);
            Assert.AreEqual(team.FreeLunch[1], result.FreeLunch[1]);
            Assert.AreEqual(team.FreeLunch[2], result.FreeLunch[2]);
            Assert.AreEqual(team.FreeLunch[3], result.FreeLunch[3]);

            complexNoSqlDb.DeleteTable("TestTeam");
        }

        private Team GetTeam()
        {
            Person Alberto = new Person
            {
                Name = "Alberto",
                Age = 35,
                DayOfBirth = new DateTime(1981, 9, 15)
            };
            Person Juan = new Person
            {
                Name = "Juan",
                Age = 15,
                DayOfBirth = new DateTime(1980, 3, 15)
            };

            var freeLunch = new Dictionary<int, DateTime>();
            freeLunch.Add(1, DateTime.Now);
            freeLunch.Add(2, DateTime.Now.AddDays(3));
            freeLunch.Add(3, DateTime.Now.AddDays(7));

            return new Team
            {
                Name = "Red Team",
                Members = new List<Person> { Alberto, Juan },
                FreeLunch = freeLunch
            };
        }
    }

    public class Person : TableEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime DayOfBirth { get; set; }
    }

    public class Team : TableEntity
    {
        public string Name { get; set; }
        public List<Person> Members { get; set; }
        public Dictionary<int, DateTime> FreeLunch { get; set; }
    }
}