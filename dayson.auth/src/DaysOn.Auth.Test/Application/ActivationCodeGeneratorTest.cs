using System;
using DaysOn.Auth.Domain.Application;
using DaysOn.Auth.Domain.Services;
using Moq;
using NUnit.Framework;

namespace DaysOn.Auth.Test.Application
{
    [TestFixture]
    public class ActivationCodeGeneratorTest
    {
        [Test]
        public void WhenICallGenerateCode_ThenIReceivedANewActivationCode()
        {
            var mockService = new Mock<IActivationService>();
            var activationGenerator = new ActivationCodeGenerator(mockService.Object);

            var result = activationGenerator.GenerateCodeAsync(Guid.NewGuid()).Result;
            Assert.IsNotNullOrEmpty(result);
        }

    }
}
