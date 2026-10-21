using System;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.CompanyBc.BoundedContext.Data;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.BoundedContext.Services;

namespace DaysOn.CompanyBc.Test.Common
{
    public class CompanyFactory
    {
        public Company CreateCompany()
        {
            var company = new Company(Guid.NewGuid())
            {
                Name = NameHelper.GenerateCompanyName(),
            };
            return company;
        }

        public Team CreateTeam()
        {
            var team = new Team(Guid.NewGuid())
            {
                Name = NameHelper.GenerateTeamName()
            };
            return team;
        }

        public Employee CreateEmployee(string role = "User")
        {
            var random = new Random(DateTime.Now.Millisecond);
            var name = NameHelper.GenerateEmployeeFirstName();
            var Lastname = NameHelper.GenerateEmployeeLastname();
            var phone = NameHelper.GeneratePhoneNumber();
            var jobtitle = NameHelper.GenerateJobTitle();
            var pictureurl = NameHelper.GeneratePictureUrl();

            var employee = new Employee(Guid.NewGuid())
            {
                Firstname = name,
                Lastname = Lastname,
                JobTitle = jobtitle,
                PictureUrl = pictureurl,
                PhoneNumber = phone,
                EmailAddress = $"{name.Replace(' ', '.')}@gmail.com",
                Role = role,
                DateOfBirth = new DateTime(random.Next(1970,1996), random.Next(1, 11), random.Next(1, 28),0,0,0),
                OthersJson = $"{{'passport':'{GeneratePassport()}'}}"
            };
            return employee;
        }

        private string GeneratePassport()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(7);
        }

        public Company CreateCompanyWithATeam()
        {
            var company = CreateCompany();
            company.Teams.Add(CreateTeam());
            return company;
        }

        public ITeamService CreateTeamService()
        {
            return new TeamService(new Repository<Team>(new CompanyContext()), new Repository<Employee>(new CompanyContext()));
        }
    }
}