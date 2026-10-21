using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.Test.Common;
using NUnit.Framework;

namespace DaysOn.CompanyBc.Test.Unit
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
        public void WhenISetAManagerTheManagerIsAssignedToTheTeam()
        {
            var team = _factory.CreateTeam();
            var employee = _factory.CreateEmployee();

            team.SetManager(employee);

            Assert.AreEqual(team.Manager.Id,employee.Id);
            Assert.IsNotNull(employee.ManagerOf.FirstOrDefault(x=> x.Id == team.Id));
        }

        [Test]
        public void WhenISetAManagerThePreviousManagerIsCleared()
        {
            var team = _factory.CreateTeam();
            var employee = _factory.CreateEmployee();
            var employee2 = _factory.CreateEmployee();

            team.SetManager(employee);
            team.SetManager(employee2);
            Assert.AreEqual(team.Manager.Id, employee2.Id);
            Assert.IsNotNull(employee2.ManagerOf.FirstOrDefault(x => x.Id == team.Id));
            Assert.IsNull(employee.ManagerOf.FirstOrDefault(x => x.Id == team.Id));
        }

        [Test]
        public void WhenISetAManagerAndEmployeeIsNullIGotAnException()
        {
            var team = _factory.CreateTeam();
            Assert.That(() => team.SetManager(null),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Argument Employee is null."));
        }

        [Test]
        public void WhenISetAManagerThatIsAlreadyATeamMemberIGetAnException()
        {
            Team team = _factory.CreateTeam();
            var employee = _factory.CreateEmployee();

            team.AddTeamMember(employee);
            
            Assert.That(() => team.SetManager(employee),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Cannot add the manager because is already a member of the team."));
        }


        [Test]
        public void WhenIAddTemporalManagerAndIAmManagerAlreadyNothingHappens()
        {
            var team = _factory.CreateTeam();
            var employee = _factory.CreateEmployee();

            team.SetManager(employee);
            
            team.SetTemporalManager(employee);
            Assert.AreEqual(team.Manager.Id, employee.Id);
            Assert.IsNotNull(employee.ManagerOf.FirstOrDefault(x => x.Id == team.Id));
            Assert.IsNull(team.TemporalManager);
            Assert.AreEqual(0,employee.TemporalManagerOf.Count);
        }

        [Test]
        public void WhenIAddATemporalManagerICanBeRetrievedFromTemporalManagerProperty()
        {
            var team = _factory.CreateTeam();
            var employee = _factory.CreateEmployee();

            team.SetTemporalManager(employee);

            var result = team.TemporalManager;

            Assert.IsNotNull(result);
            Assert.AreEqual(employee.Id,result.Id);
        }

        [Test]
        public void WhenIAddATemporalManagerTwoTimesOnlyIsAddedOnce()
        {
            var team = _factory.CreateTeam();
            var employee = _factory.CreateEmployee();

            team.SetTemporalManager(employee);
            team.SetTemporalManager(employee);

            var countTemporalManagerOf = (employee.TemporalManagerOf.Count(x => x.Id == team.Id));
            Assert.AreEqual(1, countTemporalManagerOf);
        }


        [Test]
        public void WhenITryToAddTemporalManagerPassingNullAnExceptionIsThrown()
        {
            var team = _factory.CreateTeam();
            
            Assert.That(() => team.SetTemporalManager(null),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Argument Employee is null."));
        }

        [Test]
        public void WhenITryToRemoveATemporalManagerItGetsRemoved()
        {
            var team = _factory.CreateTeam();
            var employee = _factory.CreateEmployee();

            team.SetTemporalManager(employee);

            team.RemoveTemporalManager();

            Assert.IsNull(team.TemporalManager);
            Assert.AreEqual(0, employee.TemporalManagerOf.Count);
        }

        [Test]
        public void WhenIAddATeamMemberICanRetrieveIt()
        {
            Team team = _factory.CreateTeam();
            Employee employee1 = _factory.CreateEmployee();
            var employee2 = _factory.CreateEmployee();

            team.AddTeamMember(employee1);
            team.SetManager(employee2);

            Assert.AreEqual(1, team.TeamMembers.Count);
            Assert.AreEqual(employee1.Id, team.TeamMembers[0].Id);
            Assert.AreEqual(team.Id, employee1.MyTeam.Id);
        }

        [Test]
        public void WhenITryToAddTeamMemberPassingNullAnExceptionIsThrown()
        {
            var team = _factory.CreateTeam();
            Assert.That(() => team.AddTeamMember(null),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Argument Employee is null."));
        }

        [Test]
        public void WhenIAddATeamMemberTwoTimesOnlyIsAddedOnce()
        {
            Team team = _factory.CreateTeam();
            Employee employee = _factory.CreateEmployee();

            team.AddTeamMember(employee);
            team.AddTeamMember(employee);

            var countOnTeamMembers = team.TeamMembers.Count(x => x.Id == employee.Id);
            Assert.AreEqual(1, countOnTeamMembers);
        }

        [Test]
        public void WhenITryToRemoveATeamMemberThatDoesNotExistsNothingHappens()
        {
            Team team = _factory.CreateTeam();
            var employee = _factory.CreateEmployee();
            var employee2 = _factory.CreateEmployee();

            team.AddTeamMember(employee);

            team.RemoveTeamMember(employee2);

            Assert.AreEqual(1, team.TeamMembers.Count);
        }

        [Test]
        public void WhenITryToRemoveATeamMemberItGetsRemoved()
        {
            Team team = _factory.CreateTeam();
            Employee employee = _factory.CreateEmployee();
            var employee2 = _factory.CreateEmployee();

            team.AddTeamMember(employee);
            team.AddTeamMember(employee2);

            team.RemoveTeamMember(employee);

            Assert.AreEqual(1, team.TeamMembers.Count);
            Assert.IsNull(employee.MyTeam);
            Assert.AreEqual(team.Id, employee2.MyTeam.Id);
        }

        [Test]
        public void WhenITryToRemoveATeamMemberPassingNullAnExceptionIsThrown()
        {
            var team = _factory.CreateTeam();
            var employee = _factory.CreateEmployee();
            var employee2 = _factory.CreateEmployee();

            team.AddTeamMember(employee);
            team.AddTeamMember(employee2);

            Assert.That(() => team.RemoveTeamMember(null),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Argument Employee is null."));
        }
    }
}
