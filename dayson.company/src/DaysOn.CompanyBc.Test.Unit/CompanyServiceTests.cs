using System;
using System.Linq;
using System.Linq.Expressions;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.BoundedContext.Services;
using DaysOn.Common.SqlDataAccess.Repositories;
using DaysOn.CompanyBc.Test.Common;
using NUnit.Framework;
using Moq;

namespace DaysOn.CompanyBc.Test.Unit
{
    [TestFixture]
    public class CompanyServiceTests
    {
        private CompanyFactory _factory;
        private Mock<IRepository<Company>> _mockRepository;
            
        [SetUp]
        public void Initialize()
        {
            _factory = new CompanyFactory();
            _mockRepository = new Mock<IRepository<Company>>();
        }

        [Test]
        public void WhenIAddATeamAndCompanyDoesNotExistsIGetAnException()
        {
            _mockRepository.Setup(x => x.GetAll(It.IsAny<Expression<Func<Company, bool>>>()))
                .Returns(Enumerable.Empty<Company>());

            ICompanyService service = new CompanyService(_mockRepository.Object, null,null,null);
            var team = _factory.CreateTeam();
            Assert.That(() => service.AddTeam(team, new Guid("4d486a93-3698-4c7c-a15f-1423aea2f6cb")),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Company with Id 4d486a93-3698-4c7c-a15f-1423aea2f6cb not found."));
        }

        [Test]
        public void WhenIRemoveATeamAndCompanyDoesNotExistsIGetAnException()
        {
            _mockRepository.Setup(x => x.GetAll(It.IsAny<Expression<Func<Company, bool>>>()))
                .Returns(Enumerable.Empty<Company>());
            var mock = new Mock<ITeamService>();

            ICompanyService service = new CompanyService(_mockRepository.Object, mock.Object, null, null);
            var team = _factory.CreateTeam();
            Assert.That(() => service.RemoveTeam(team.Id, new Guid("4d486a93-3698-4c7c-a15f-1423aea2f6cb")),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Company with Id 4d486a93-3698-4c7c-a15f-1423aea2f6cb not found."));
        }

        [Test]
        public void WhenIAddAEmployeeAndCompanyDoesNotExistsIGetAnException()
        {
            _mockRepository.Setup(x => x.GetAll(It.IsAny<Expression<Func<Company, bool>>>()))
                .Returns(Enumerable.Empty<Company>());

            ICompanyService service = new CompanyService(_mockRepository.Object, null, null, null);
            var employee = _factory.CreateEmployee();
            Assert.That(() => service.AddEmployee(employee, new Guid("4d486a93-3698-4c7c-a15f-1423aea2f6cb")),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Company with Id 4d486a93-3698-4c7c-a15f-1423aea2f6cb not found."));
        }


        [Test]
        public void WhenIUpdateAnEmployeeAndCompanyDoesNotExistsIGetAnException()
        {
            _mockRepository.Setup(x => x.GetAll(It.IsAny<Expression<Func<Company, bool>>>()))
                .Returns(Enumerable.Empty<Company>());

            ICompanyService service = new CompanyService(_mockRepository.Object, null, null, null);
            var employee = _factory.CreateEmployee();
            
            Assert.That(() => service.UpdateEmployee(employee, new Guid("4d486a93-3698-4c7c-a15f-1423aea2f6cb")),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Company with Id 4d486a93-3698-4c7c-a15f-1423aea2f6cb not found."));
        }

        [Test]
        public void WhenIRemoveAEmployeeAndCompanyDoesNotExistsIGetAnException()
        {
            _mockRepository.Setup(x => x.GetAll(It.IsAny<Expression<Func<Company, bool>>>()))
                .Returns(Enumerable.Empty<Company>());

            ICompanyService service = new CompanyService(_mockRepository.Object, null, null, null);
            var employee = _factory.CreateEmployee();
            Assert.That(() => service.RemoveEmployee(employee, new Guid("4d486a93-3698-4c7c-a15f-1423aea2f6cb")),
                Throws.TypeOf<ArgumentException>()
                    .With.Message.EqualTo("Company with Id 4d486a93-3698-4c7c-a15f-1423aea2f6cb not found.")); 
        }
    }
}
