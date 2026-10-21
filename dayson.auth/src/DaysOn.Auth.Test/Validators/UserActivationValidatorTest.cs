using System;
using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Auth.Web.Validators;
using NUnit.Framework;

namespace DaysOn.Auth.Test.Validators
{
    [TestFixture]
    public class UserActivationValidatorTest
    {
        [Test]
        public void GivenValidActivationRequest_ThenValidatorReturnNoErrors()
        {
            var request = new ActivateUserCommand()
                {
                    ActivationCode = Guid.NewGuid()
                };

            var validator = new UserActivationValidator();
            var result = validator.Validate(request);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void GivenNullActivationRequest_ThenValidatorReturnInvalid()
        {
            var request = new ActivateUserCommand();

            var validator = new UserActivationValidator();
            var result = validator.Validate(request);

            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void GivenNullActivationRequest_ThenValidatorReturn1Error()
        {
            var request = new ActivateUserCommand();

            var validator = new UserActivationValidator();
            var result = validator.Validate(request);

            Assert.AreEqual(1, result.Errors.Count);
        }


        [Test]
        public void GivenGuidEmptyActivationRequest_ThenValidatorReturn1Error()
        {
            var request = new ActivateUserCommand()
            {
                ActivationCode = Guid.Empty
            };

            var validator = new UserActivationValidator();
            var result = validator.Validate(request);

            Assert.AreEqual(1, result.Errors.Count);
        }
    }
}
