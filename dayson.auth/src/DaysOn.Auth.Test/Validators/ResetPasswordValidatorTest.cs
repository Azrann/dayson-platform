using System;
using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Auth.Web.Validators;
using NUnit.Framework;
using ApiErrors = DaysOn.Auth.Domain.ApiErrors;

namespace DaysOn.Auth.Test.Validators
{
    [TestFixture]
    public class ResetPasswordValidatorTest
    {
        [Test]
        public void GivenValidResetPasswordValidatorCommand_ThenValidatorReturnNoErrors()
        {
            var request = new ResetPasswordCommand()
            {
                Password = "REPLACE_WITH_YOUR_VALUE",
                ConfirmPassword = "REPLACE_WITH_YOUR_VALUE",
                ResetPasswordToken = Guid.NewGuid()
            };

            var validator = new ResetPasswordCommandValidator();
            var result = validator.Validate(request);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void GivenEmptyPasswordInResetPasswordValidatorCommand_ThenValidatorReturnErrors()
        {
            var request = new ResetPasswordCommand()
            {
                ConfirmPassword = "REPLACE_WITH_YOUR_VALUE",
                ResetPasswordToken = Guid.NewGuid()
            };

            var validator = new ResetPasswordCommandValidator();
            var result = validator.Validate(request);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ApiErrors.PasswordMustBeSupplied, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void GivenEmptyConfirmationPasswordInResetPasswordValidatorCommand_ThenValidatorReturnErrors()
        {
            var request = new ResetPasswordCommand()
            {
                Password = "REPLACE_WITH_YOUR_VALUE",
                ResetPasswordToken = Guid.NewGuid()
            };

            var validator = new ResetPasswordCommandValidator();
            var result = validator.Validate(request);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ApiErrors.ConfirmPasswordMustBeSupplied, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void GivenNullEmptyResetPasswordTokenInResetPasswordValidatorCommand_ThenValidatorReturnErrors()
        {
            var request = new ResetPasswordCommand()
            {
                Password = "REPLACE_WITH_YOUR_VALUE",
                ConfirmPassword = "REPLACE_WITH_YOUR_VALUE",
            };

            var validator = new ResetPasswordCommandValidator();
            var result = validator.Validate(request);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ApiErrors.ResetPasswordTokenIsEmptyOrInvalid, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void GivenPasswordAndConfirmPassword_WhenTheyDoNotMatch_ThenValidatorReturnErrors()
        {
            var request = new ResetPasswordCommand()
            {
                Password = "REPLACE_WITH_YOUR_VALUE",
                ConfirmPassword = "REPLACE_WITH_YOUR_VALUE",
                ResetPasswordToken = Guid.NewGuid()
            };

            var validator = new ResetPasswordCommandValidator();
            var result = validator.Validate(request);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ApiErrors.PasswordAndConfirmPasswordMustMatch, result.Errors[0].ErrorMessage);
        }
    }
}
