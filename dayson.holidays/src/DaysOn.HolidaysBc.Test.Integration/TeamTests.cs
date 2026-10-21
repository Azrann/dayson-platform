using System;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.HolidaysBc.Test.Common;
using NUnit.Framework;
using DaysOn.Common.DataAccess;

namespace DaysOn.HolidaysBc.Test.Integration
{
    [TestFixture]
    public class TeamTests
    {
        private CompanyFactory _factory;
        private ICompanyService _companyService;
        private ITeamService _teamService;
        private Company _company;

        [OneTimeSetUp]
        public void Initialize()
        {
            _factory = new CompanyFactory();
            var context = new HolidaysContext();
            _teamService = new TeamService(new Repository<Team>(context), new Repository<Rule>(context));
            _companyService = new CompanyService(new Repository<Company>(context), new DapperNoSql<OpenClosedDates>());

            _company = _factory.CreateCompany();
            _companyService.AddCompany(_company);
        }

        [Test]
        public void WhenIRetrieveATeamThatDoesNotExistsIRecieveNullObject()
        {
            var result = _teamService.GetTeamById(Guid.NewGuid());
            Assert.IsNull(result);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _companyService.RemoveCompany(_company.Id);
        }
    }
}
