using System.Collections.Generic;
using System.Threading.Tasks;
using DaysOn.Auth.Domain.Entities;
using Microsoft.AspNet.Identity;
using Claim = System.Security.Claims.Claim;

namespace DaysOn.Auth.Domain.Application
{
    public interface IUserManagerWrapper
    {
        Task<IdentityResult> CreateAsync(User user, string password, UserManager<User> userManager);

        Task<User> FindByIdAsync(string userId, UserManager<User> userManager);

        Task<User> FindByEmailAsync(string email, UserManager<User> userManager);

        Task<IdentityResult> UpdateAsync(User user, UserManager<User> userManager);

        Task<IdentityResult> AddClaimAsync(string userId, Claim claim, UserManager<User> userManager);

        IEnumerable<IdentityResult> AddClaims(string userId, IEnumerable<Claim> claims, UserManager<User> userManager);

        Task<IdentityResult> RemoveClaimAsync(string userId, Claim claim, UserManager<User> userManager);

        Task<IList<Claim>> GetClaimsAsync(string userId, UserManager<User> userManager);
        
        Task<IdentityResult> AddToRoleAsync(string userId, string role, UserManager<User> userManager);

        Task<IdentityResult> RemoveFromRoleAsync(string userId, string role, UserManager<User> userManager);

        Task<IList<string>> GetRolesAsync(string userId, UserManager<User> userManager);

        Task<IdentityResult> DeleteAsync(User user, UserManager<User> userManager);

        string HashPassword(string password, UserManager<User> userManager);
    }
}