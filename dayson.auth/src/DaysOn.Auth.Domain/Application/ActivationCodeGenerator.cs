using System;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Auth.Domain.Services;

namespace DaysOn.Auth.Domain.Application
{
    [CastleComponent]
    public class ActivationCodeGenerator : IActivationCodeGenerator
    {
        private readonly IActivationService _activationService;

        public ActivationCodeGenerator(IActivationService activationService)
        {
            _activationService = activationService;
        }

        public async Task<string> GenerateCodeAsync(Guid userId)
        {
            Guid activationCodeId = Guid.NewGuid();
            await _activationService.CreateActivationCodeAsync(new ActivationCode(activationCodeId, userId));
            return activationCodeId.ToString();
        }
    }
}