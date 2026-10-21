using System;
using System.Threading.Tasks;
using DaysOn.Auth.Domain.Entities;

namespace DaysOn.Auth.Domain.Services
{
    public interface IActivationService
    {
        Task ActivateAsync(Guid activationCode);

        Task CreateActivationCodeAsync(ActivationCode code);

        Task EmailConfirmed(string userId);
    }
}