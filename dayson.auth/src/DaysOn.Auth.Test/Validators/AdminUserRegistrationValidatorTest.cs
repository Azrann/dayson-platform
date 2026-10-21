using System;
using DaysOn.Auth.Models.ApiCommand;
using DaysOn.Auth.Web.Validators;
using NUnit.Framework;

namespace DaysOn.Auth.Test.Validators
{
    [TestFixture]
    public class AdminUserRegistrationValidatorTest
    {
        [Test]
        public void GivenValidUserRegistrationRequest_ThenValidatorReturnNoErrors()
        {
            var request = new RegisterAdminUserCommand
                {
                    CompanyId = Guid.NewGuid().ToString(),
                    Email = "user@example.invalid",
                    CorrelationId = Guid.NewGuid(),
                    LanguageCode = string.Empty
                };

            var validator = new AdminUserRegistrationValidator();
            var result = validator.Validate(request);

            Assert.IsTrue(result.IsValid);
        }
    }
}
