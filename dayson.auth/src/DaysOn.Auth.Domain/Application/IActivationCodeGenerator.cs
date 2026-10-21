using System;
using System.Threading.Tasks;

namespace DaysOn.Auth.Domain.Application
{
    public interface IActivationCodeGenerator
    {
        Task<string> GenerateCodeAsync(Guid userId);
    }
}