using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Auth.Web.Validators;
using NUnit.Framework;

namespace DaysOn.Auth.Test.Validators
{
    [TestFixture]
    public class ResetPasswordCodeValidatorTest
    {
        [Test]
        public void GivenValidResetPasswordCodeValidatorCommand_ThenValidatorReturnNoErrors()
        {
            var request = new ResetPasswordCodeCommand()
            {
                Email = "user@example.invalid"
            };

            var validator = new ResetPasswordCodeCommandValidator();
            var result = validator.Validate(request);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void GivenEmptyEmailInResetPasswordCodeValidatorCommand_ThenValidatorReturnErrors()
        {
            var request = new ResetPasswordCodeCommand()
            {
                Email = string.Empty
            };

            var validator = new ResetPasswordCodeCommandValidator();
            var result = validator.Validate(request);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("'Email' should not be empty.", result.Errors[0].ErrorMessage);
        }

        [Test]
        public void GivenNullEmailInResetPasswordCodeValidatorCommand_ThenValidatorReturnErrors()
        {
            var request = new ResetPasswordCodeCommand()
            {
                Email = null
            };

            var validator = new ResetPasswordCodeCommandValidator();
            var result = validator.Validate(request);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("'Email' must not be empty.", result.Errors[0].ErrorMessage);
        }
    }
}
