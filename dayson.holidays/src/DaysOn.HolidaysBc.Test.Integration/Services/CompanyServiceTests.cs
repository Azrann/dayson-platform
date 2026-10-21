using System;
using System.Collections.Generic;
using System.Data.Entity;
using DaysOn.HolidaysBc.BoundedContext.Data;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.BoundedContext.Services;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.HolidaysBc.Test.Common;
using NUnit.Framework;
using DaysOn.Common.DataAccess;

namespace DaysOn.HolidaysBc.Test.Integration.Services
{
    [TestFixture]
    public class CompanyServiceTests
    {
        private DbContext _dbContext;
        private CompanyService _companyService;
        private CompanyFactory _factory;
        private List<Company> _companyListToRemove;

        [SetUp]
        public void Initialize()
        {
            _factory = new CompanyFactory();
            _dbContext = new HolidaysContext();
            _companyService = new CompanyService(new Repository<Company>(_dbContext), new DapperNoSql<OpenClosedDates>());
            _companyListToRemove = new List<Company>();
        }

        [Test]
        public void WhenIAddACompany_ThenICanRetrieveIt()
        {
            var company = _factory.CreateCompany();
            _companyService.AddCompany(company);

            var myCompany = _companyService.GetCompanyById(company.Id);

            Assert.IsNotNull(myCompany);
            Assert.IsNotNull(myCompany.GlobalRules);

            _companyListToRemove.Add(company);
        }

        [Test]
        public void WhenIRemoveACompany_ItCannotAccessAnymore()
        {
            var company = _factory.CreateCompany();
            _companyService.AddCompany(company);

            var myCompany = _companyService.GetCompanyById(company.Id);
            _companyService.RemoveCompany(myCompany.Id);

            Assert.That(() => _companyService.GetCompanyById(company.Id),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void WhenICallGetGlobalRules_ThenICanRetrieveIt()
        {
            var company = _factory.CreateCompany();

            _companyService.AddCompany(company);

            var globalRules = _companyService.GetGlobalRules(company.Id);

            Assert.IsNotNull(globalRules);

            _companyListToRemove.Add(company);
        }

        [Test]
        public void WhenICallGetGlobalRulesAndCompanyDoesNotExists_IThrowAnException()
        {
            Assert.That(() => _companyService.GetGlobalRules(Guid.NewGuid()),
                Throws.TypeOf<ArgumentException>());

        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            foreach (var company in _companyListToRemove)
            {
                _companyService.RemoveCompany(company.Id);
            }
        }
    }
}