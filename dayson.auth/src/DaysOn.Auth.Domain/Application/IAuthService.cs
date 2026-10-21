using System.Threading.Tasks;
using DaysOn.Auth.Models.CommandResult;

namespace DaysOn.Auth.Domain.Application
{
    public interface IAuthService
    {
        Task<RegisterUserCommandResult> RegisterUserAsync(string email, string password, string companyId = "",
            string roleName = "", string employeeId = null);
    }
}
