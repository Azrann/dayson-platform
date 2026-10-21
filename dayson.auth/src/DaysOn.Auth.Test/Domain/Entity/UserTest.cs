using System;
using DaysOn.Auth.Domain.Entities;
using NUnit.Framework;

namespace DaysOn.Auth.Test.Domain.Entity
{
    [TestFixture]
    public class UserTest
    {
        [Test]
        public void WhenIActivateUserWithValidParameters_UserIsActivated()
        {
            var userId = Guid.NewGuid();

            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            user.Activate(new ActivationCode(Guid.NewGuid(),userId));

            Assert.IsTrue(user.IsActive);
        }

        [Test]
        public void WhenIActivateUserWithValidParameters_UserEmailIsConfirmed()
        {
            var userId = Guid.NewGuid();

            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            user.Activate(new ActivationCode(Guid.NewGuid(), userId));

            Assert.IsTrue(user.EmailConfirmed);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Activation code 960609e8-af47-495c-81a5-7a11ba698f66 is not valid")]
        public void WhenIActivateUserWithInValidParameters_ExceptionIsThrown()
        {
            var userId = Guid.NewGuid();

            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            user.Activate(new ActivationCode(new Guid("960609e8-af47-495c-81a5-7a11ba698f66"), Guid.NewGuid()));
        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Activation code 960609e8-af47-495c-81a5-7a11ba698f66 has already expired")]
        public void WhenIActivateUserWithExpiredActivationCode_ExceptionIsThrown()
        {
            var userId = Guid.NewGuid();

            var user = new User(userId.ToString(), "user@example.invalid", Guid.NewGuid().ToString());

            var activationCode = new ActivationCode(new Guid("960609e8-af47-495c-81a5-7a11ba698f66"), userId, -1);

            user.Activate(activationCode);
        }

        //// Added to get code coverage 100% due to limitation of Entity Framework parameterless constructor requirement
        [Test]
        public void WhenICreateUserWithNoParameters_IGetAnInstance()
        {
            var mock = new MockUser();
            Assert.IsNotNull(mock);
        }
    }

    public class MockUser : User
    {
        public MockUser() : base()
        {
            
        }
    }
}
