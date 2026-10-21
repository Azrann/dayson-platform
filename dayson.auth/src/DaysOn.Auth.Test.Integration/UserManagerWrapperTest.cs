using System;
using System.Linq;
using System.Security.Claims;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Data;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Common.Domain.Ioc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;

namespace DaysOn.Auth.Test.Integration
{
    [TestFixture]
    public class UserManagerWrapperTest
    {
        private IUserManagerWrapper _userManagerWrapper;
        private IUserManagerFactory _userManagerFactory;
        private UserManager<User> _userManager;
        private User _user;

        [TestFixtureSetUp]
        public void Initialize()
        {
            _userManagerFactory = new UserManagerFactory();
            _userManagerWrapper = new UserManagerWrapper();
            _userManager = _userManagerFactory.CreateUserManager();
            _user = new User(Guid.NewGuid().ToString(), $"{Guid.NewGuid()}user@example.invalid", Guid.NewGuid().ToString());
            var result = _userManagerWrapper.CreateAsync(_user, "pass123", _userManager).Result;
        }

        [Test]
        public void WhenIAddAClaimToAUserICanRetrieveIt()
        {
            var claimResult =_userManagerWrapper.AddClaimAsync(_user.Id, new Claim("Company", "123"), _userManager).Result;

            var result = _userManagerWrapper.FindByIdAsync(_user.Id, _userManager).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(1,result.Claims.Count);
            Assert.AreEqual("Company", result.Claims.First().ClaimType);
            Assert.AreEqual("123", result.Claims.First().ClaimValue);
        }

        [Test]
        public void WhenIAddTwoClaimsOfTheSameTypeBothOfThemCanBeRetrieved()
        {
            var claimResult1 = _userManagerWrapper.AddClaimAsync(_user.Id, new Claim("Company", "123"), _userManager).Result;
            var claimResult2 = _userManagerWrapper.AddClaimAsync(_user.Id, new Claim("Company", "345"), _userManager).Result;

            var result = _userManagerWrapper.FindByIdAsync(_user.Id, _userManager).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Claims.Count);
            Assert.AreEqual("Company", result.Claims.First().ClaimType);
            Assert.AreEqual("123", result.Claims.First().ClaimValue);
            Assert.AreEqual("Company", result.Claims.Last().ClaimType);
            Assert.AreEqual("345", result.Claims.Last().ClaimValue);
        }

        [Test]
        public void WhenIAddARoleToAUserICanRetrieveIt()
        {
            var roleResult = _userManagerWrapper.AddToRoleAsync(_user.Id, "Owner", _userManager).Result;

            var result = _userManagerWrapper.FindByIdAsync(_user.Id, _userManager).Result;

            var rolesManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AuthContext()));
            var role = rolesManager.FindByIdAsync(result.Roles.First().RoleId).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Roles.Count);
            Assert.AreEqual("Owner", role.Name);
        }

        [TearDown]
        public void TearDown()
        {
            ContainerManager.ReleaseAll();
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            var deleteResult = _userManagerWrapper.DeleteAsync(_userManagerWrapper.FindByIdAsync(_user.Id, _userManager).Result, _userManager).Result;
        }
    }
}
