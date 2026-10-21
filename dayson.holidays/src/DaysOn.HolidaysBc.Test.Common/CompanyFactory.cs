using System;
using System.Collections.Generic;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using Status = DaysOn.HolidaysBc.BoundedContext.Entities.Status;

namespace DaysOn.HolidaysBc.Test.Common
{
    public class CompanyFactory
    {
        public Company CreateCompany()
        {
            Guid companyId = Guid.NewGuid();
            var company = new Company(companyId)
            {
                Name = NameHelper.GenerateCompanyName(),
                CreatedOn = DateTime.UtcNow,
                GlobalRules = CreateGlobalRules(companyId),
                LastUpdated = DateTime.UtcNow
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

        public Employee CreateEmployee()
        {
            var name = NameHelper.GenerateEmployeeName();
            var employee = new Employee(Guid.NewGuid())
            {
                MaxDaysCarriedOverNextYear = 0,
                HolidayEntitlement = 24,
                HolidayRequests = new List<HolidayRequest>(),
                MaxConsecutiveDaysOff = 12,
                MinDaysAdvanceNoticeHolidays = 2,
                Email = $"{name.Replace(' ', '.')}@gmail.com",
                YearStartDay = new DateTime(2000,1,1),
                CompanyId = Guid.NewGuid()
            };
            return employee;
        }

        public Team CreateRichTeam()
        {
            var team = new Team(Guid.NewGuid())
            {
                Name = NameHelper.GenerateTeamName(),
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid()
            };
            return team;
        }


        public Rule CreateRule()
        {
            var name = NameHelper.GenerateEmployeeName();
            var rule = new Rule(Guid.NewGuid())
            {
                Name = name,
            };
            return rule;
        }

        public HolidayRequest CreateHolidayRequest()
        {
            return new HolidayRequest(Guid.NewGuid())
            {
                CreatedOn = DateTime.UtcNow,
                From = DateTime.UtcNow.AddDays(2),
                To = DateTime.UtcNow.AddDays(5),
                Notes = "Holidays in Spain,",
                NumberOfDays = 3,
                Status = Status.Requested,
                LastUpdated = DateTime.UtcNow
            };
        }

        public HolidayRequest CreateHolidayRequest(int days, int when = 2)
        {
            return new HolidayRequest(Guid.NewGuid())
            {
                CreatedOn = DateTime.UtcNow,
                From = DateTime.UtcNow.AddDays(when),
                To = DateTime.UtcNow.AddDays(when + days),
                Notes = $"Holidays for {days} days",
                NumberOfDays = days,
                Status = Status.Requested,
                LastUpdated = DateTime.UtcNow
            };
        }

        public GlobalRules CreateGlobalRules(Guid companyId)
        {
            return new GlobalRules(Guid.NewGuid(),companyId)
            {
                MaxDaysCarriedOverNextYear = 5,
                DefaultNumberOfHolidays = 25,
                MaxConsecutiveDaysOff = 10,
                MinDaysAdvanceNoticeHolidays = 15,
                CreatedOn = DateTime.Now,
                LastUpdated = DateTime.Now,
                YearStartDay = new DateTime(2000, 2, 2),
            };
        }

        public YearlyHolidayCalculation CreateYearlyHolidayCalculation(int year = 2000)
        {
            return new YearlyHolidayCalculation(Guid.NewGuid())
            {
                Year = year,
                NumberOfDaysLeft = 30,
                NumberOfDays = 30,
                CreatedOn = DateTime.Now,
                LastUpdated = DateTime.Now
            };
        }
    }
}