using System;
using AutoMapper;
using DaysOn.CompanyBc.Api.App_Start;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.Models.Dtos;
using NUnit.Framework;

namespace DaysOn.CompanyBc.Test.Unit
{
    [TestFixture]
    public class AutoMapperTests 
    {
        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test]
        public void WhenIMapAEmployee_ThenIGetTheRightDto()
        {
            Team team1 = new Team(Guid.NewGuid())
            {
                Name = "TeamA"
            };
            Team team2 = new Team(Guid.NewGuid())
            {
                Name = "TeamB"
            };
            Employee emp1 = new Employee(Guid.NewGuid())
            {
                Firstname = "Frodo",
                Lastname = "Bolson",
                JobTitle = "Explorador",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
                PictureUrl = "http://wwww.pictures/mypic.jpg",
                EmailAddress = "user@example.invalid",
                Role = "Admin",
                DateOfBirth = new DateTime(1981, 9, 15, 9, 0, 0),
                Middlename = "Notengo",
                OthersJson = "{'dni':'7855454545'}"
            };

            team1.AddTeamMember(emp1);
            team2.SetManager(emp1);

            EmployeeDto dto = AutoMapperBase.Mapper.Map<EmployeeDto>(emp1);

            Assert.AreEqual(emp1.Firstname, dto.Firstname);
            Assert.AreEqual(team1.Name, dto.MyTeam.Name);
            Assert.AreEqual(team2.Name, dto.ManagerOf[0].Name);
            Assert.AreEqual(emp1.EmailAddress, dto.EmailAddress);
            Assert.AreEqual(emp1.Role, dto.Role);
            Assert.AreEqual(emp1.Lastname, dto.Lastname);
            Assert.AreEqual(emp1.JobTitle, dto.JobTitle);
            Assert.AreEqual(emp1.PhoneNumber, dto.PhoneNumber);
            Assert.AreEqual(emp1.PictureUrl, dto.PictureUrl);
            Assert.AreEqual(emp1.Middlename, dto.Middlename);
            Assert.AreEqual(emp1.DateOfBirth, dto.DateOfBirth);
            Assert.AreEqual(emp1.OthersJson, dto.OthersJson);
        }

        [Test]
        public void WhenIMapAEmployeeToShortDto_ThenIGetTheRightDto()
        {
            Team team1 = new Team(Guid.NewGuid())
            {
                Name = "TeamA"
            };
            Team team2 = new Team(Guid.NewGuid())
            {
                Name = "TeamB"
            };
            Employee emp1 = new Employee(Guid.NewGuid())
            {
                Firstname = "Frodo",
                Lastname = "Bolson",
                JobTitle = "Explorador",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
                PictureUrl = "http://wwww.pictures/mypic.jpg",
                EmailAddress = "user@example.invalid",
                Role = "Admin",
                DateOfBirth = new DateTime(1981, 9, 15, 9, 0, 0),
                Middlename = "Notengo",
                OthersJson = "{'dni':'7855454545'}"
            };

            team1.AddTeamMember(emp1);
            team2.SetManager(emp1);

            ShortEmployeeDto dto = AutoMapperBase.Mapper.Map<ShortEmployeeDto>(emp1);

            Assert.AreEqual(emp1.Firstname, dto.Firstname);
            Assert.AreEqual(emp1.Lastname, dto.Lastname);
            Assert.AreEqual(emp1.EmailAddress, dto.EmailAddress);
            Assert.AreEqual(emp1.Role, dto.Role);
            Assert.AreEqual(emp1.JobTitle, dto.JobTitle);
            Assert.AreEqual(emp1.PhoneNumber, dto.PhoneNumber);
            Assert.AreEqual(emp1.PictureUrl, dto.PictureUrl);
            Assert.AreEqual(emp1.Middlename, dto.Middlename);
            Assert.AreEqual(emp1.DateOfBirth, dto.DateOfBirth);
            Assert.AreEqual(emp1.OthersJson, dto.OthersJson);
        }

        [Test]
        public void WhenIMapATeamToRichDto_ThenIGetTheRightDto()
        {
            Team team1 = new Team(Guid.NewGuid())
            {
                Name = "TeamA"
            };

            Employee emp1 = new Employee(Guid.NewGuid())
            {
                Firstname = "Frodo",
                Lastname = "Bolson",
                JobTitle = "Explorador",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
                PictureUrl = "http://wwww.pictures/mypic.jpg",
                EmailAddress = "user@example.invalid",
                Role = "Admin",
            };

            Employee emp2 = new Employee(Guid.NewGuid())
            {
                Firstname = "Gandalf",
                Lastname = "El Blanco",
                JobTitle = "Mago",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
                PictureUrl = "http://wwww.pictures/mypic2.jpg",
                EmailAddress = "user@example.invalid",
                Role = "Owner",
                DateOfBirth = new DateTime(1981, 9, 15, 9, 0, 0),
                Middlename = "Notengo",
                OthersJson = "{'dni':'7855454545'}"
            };
           

            Employee emp3 = new Employee(Guid.NewGuid())
            {
                Firstname = "Sam",
                Lastname = "Sagaz",
                JobTitle = "Cocinero",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
                PictureUrl = "http://wwww.pictures/mypic3.jpg",
                EmailAddress = "user@example.invalid",
                Role = "User",
            };

            Employee emp4 = new Employee(Guid.NewGuid())
            {
                Firstname = "Legoals",
                Lastname = "Luji",
                JobTitle = "Arquero",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
                PictureUrl = "http://wwww.pictures/mypic4.jpg",
                EmailAddress = "user@example.invalid",
                Role = "Admin",
            };

            team1.AddTeamMember(emp1);
            team1.AddTeamMember(emp2);
            team1.SetManager(emp3);
            team1.SetTemporalManager(emp4);

            RichTeamDto dto = AutoMapperBase.Mapper.Map<RichTeamDto>(team1);

            Assert.AreEqual(team1.Name, dto.Name);
            Assert.AreEqual(emp1.Firstname, dto.TeamMembers[0].Firstname);
            Assert.AreEqual(emp2.Firstname, dto.TeamMembers[1].Firstname);
            Assert.AreEqual(emp3.Firstname, dto.Manager.Firstname);
            Assert.AreEqual(emp4.Firstname, dto.TemporalManager.Firstname);
            Assert.AreEqual(emp1.Lastname, dto.TeamMembers[0].Lastname);
            Assert.AreEqual(emp2.Lastname, dto.TeamMembers[1].Lastname);
            Assert.AreEqual(emp3.JobTitle, dto.Manager.JobTitle);
            Assert.AreEqual(emp4.JobTitle, dto.TemporalManager.JobTitle);
            Assert.AreEqual(emp1.Role, dto.TeamMembers[0].Role);
            Assert.AreEqual(emp2.Role, dto.TeamMembers[1].Role);
            Assert.AreEqual(emp3.PhoneNumber, dto.Manager.PhoneNumber);
            Assert.AreEqual(emp4.PhoneNumber, dto.TemporalManager.PhoneNumber);
            Assert.AreEqual(emp1.EmailAddress, dto.TeamMembers[0].EmailAddress);
            Assert.AreEqual(emp2.EmailAddress, dto.TeamMembers[1].EmailAddress);
            Assert.AreEqual(emp3.PictureUrl, dto.Manager.PictureUrl);
            Assert.AreEqual(emp4.PictureUrl, dto.TemporalManager.PictureUrl);
            Assert.AreEqual(emp2.Middlename, dto.TeamMembers[1].Middlename);
            Assert.AreEqual(emp2.DateOfBirth, dto.TeamMembers[1].DateOfBirth);
            Assert.AreEqual(emp2.OthersJson, dto.TeamMembers[1].OthersJson);
        }

        [Test]
        public void WhenIMapATeam_ThenIGetTheRightDto()
        {
            Team team1 = new Team(Guid.NewGuid())
            {
                Name = "TeamA"
            };

            Employee emp1 = new Employee(Guid.NewGuid())
            {
                Firstname = "Frodo",
                EmailAddress = "user@example.invalid",
            };

            Employee emp2 = new Employee(Guid.NewGuid())
            {
                Firstname = "Gandalf",
                EmailAddress = "user@example.invalid",
            };

            Employee emp3 = new Employee(Guid.NewGuid())
            {
                Firstname = "Sam",
                EmailAddress = "user@example.invalid",
            };

            Employee emp4 = new Employee(Guid.NewGuid())
            {
                Firstname = "Legoals",
                EmailAddress = "user@example.invalid",
            };

            team1.AddTeamMember(emp1);
            team1.AddTeamMember(emp2);
            team1.SetManager(emp3);
            team1.SetTemporalManager(emp4);

            TeamDto dto = AutoMapperBase.Mapper.Map<TeamDto>(team1);

            Assert.AreEqual(team1.Name, dto.Name);
            Assert.AreEqual(emp1.Firstname, dto.TeamMembers[0].Name);
            Assert.AreEqual(emp2.Firstname, dto.TeamMembers[1].Name);
            Assert.AreEqual(emp3.Firstname, dto.Manager.Name);
            Assert.AreEqual(emp4.Firstname, dto.TemporalManager.Name);
        }
    }
}
