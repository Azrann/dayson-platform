using System.Threading.Tasks;
using DaysOn.Auth.Domain.Entities;

namespace DaysOn.Auth.Domain.Application
{
    public interface IUserStoreWrapper
    {
        Task ResetPasswordAsync(User user, string hashedNewPassword);
    }
}