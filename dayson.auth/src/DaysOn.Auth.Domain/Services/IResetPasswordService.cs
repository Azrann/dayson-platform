using System;
using System.Threading.Tasks;
using DaysOn.Auth.Models.ApiCommand;

namespace DaysOn.Auth.Domain.Services
{
    public interface IResetPasswordService
    {
        Task CreateResetPasswordCodeAsync(Guid code, string email);

        Task ResetPasswordAsync(ResetPasswordCommand command);

        Task ResetToRandomPassword(string email);
    }
}