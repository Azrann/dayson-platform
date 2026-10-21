using System;
using System.Linq;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.Test.Common;
using NUnit.Framework;

namespace DaysOn.HolidaysBc.Test.Unit.Entities
{
    [TestFixture]
    public class TeamTests
    {
        private CompanyFactory _factory;

        [SetUp]
        public void Initialize()
        {
             _factory = new CompanyFactory();
        }
        [Test]
        public void WhenIAddARuleICanRetrieveIt()
        {
            Team team = _factory.CreateTeam();
            var rule = _factory.CreateRule();

            team.AddRule(rule);

            Assert.AreEqual(1, team.Rules.Count);
            Assert.AreEqual(rule.Id, team.Rules[0].Id);
            Assert.AreEqual(team.Id, rule.Team.Id);
        }

        [Test]
        public void WhenITryToAddRulePassingNullAnExceptionIsThrown()
        {
            var team = _factory.CreateTeam();
            Assert.That(() => team.AddRule(null),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Argument Rule is null."));         
        }

        [Test]
        public void WhenIAddARuleTwoTimesOnlyIsAddedOnce()
        {
            Team team = _factory.CreateTeam();
            var rule = _factory.CreateRule();

            team.AddRule(rule);
            team.AddRule(rule);

            var countOnTeamMembers = team.Rules.Count(x => x.Id == rule.Id);
            Assert.AreEqual(1, countOnTeamMembers);
        }

        [Test]
        public void WhenITryToRemoveARuleThatDoesNotExistsNothingHappens()
        {
            Team team = _factory.CreateTeam();
            var rule = _factory.CreateRule();
            var rule1 = _factory.CreateRule();

            team.AddRule(rule);

            team.RemoveRule(rule1);

            Assert.AreEqual(1, team.Rules.Count);
        }

        [Test]
        public void WhenITryToRemoveARuleItGetsRemoved()
        {
            Team team = _factory.CreateTeam();
            var rule = _factory.CreateRule();
            var rule1 = _factory.CreateRule();

            team.AddRule(rule);
            team.AddRule(rule1);

            team.RemoveRule(rule);

            Assert.AreEqual(1, team.Rules.Count);
            Assert.IsNull(rule.Team);
            Assert.AreEqual(team.Id, rule1.Team.Id);
        }

        [Test]
        public void WhenITryToRemoveARulePassingNullAnExceptionIsThrown()
        {
            var team = _factory.CreateTeam();
            var rule = _factory.CreateRule();
            var rule2 = _factory.CreateRule();

            team.AddRule(rule);
            team.AddRule(rule2);

            Assert.That(() => team.RemoveRule(null),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Argument Rule is null."));
        }
    }
}
