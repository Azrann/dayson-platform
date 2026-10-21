using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Castle.Core;
using DaysOn.Auth.Domain.Entities;
using Microsoft.AspNet.Identity;
using Claim = System.Security.Claims.Claim;
using System.Linq;

namespace DaysOn.Auth.Domain.Application
{
    [ExcludeFromCodeCoverage]
    [CastleComponent]
    public class UserManagerWrapper : IUserManagerWrapper
    {
        public async Task<IdentityResult> CreateAsync(User user, string password, UserManager<User> userManager)
        {
            return await userManager.CreateAsync(user, password);
        }

        public async Task<User> FindByIdAsync(string userId, UserManager<User> userManager)
        {
            return await userManager.FindByIdAsync(userId);
        }

        public async Task<User> FindByEmailAsync(string email, UserManager<User> userManager)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> UpdateAsync(User user, UserManager<User> userManager)
        {
            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> AddClaimAsync(string userId, Claim claim, UserManager<User> userManager)
        {
            return await userManager.AddClaimAsync(userId, claim);
        }

        public IEnumerable<IdentityResult> AddClaims(string userId, IEnumerable<Claim> claims, UserManager<User> userManager)
        {
            var results = new List<IdentityResult>();
            foreach (var claim in claims)
            {
                results.Add(userManager.AddClaim(userId, claim));
            }
            return results;
        }

        public async Task<IdentityResult> RemoveClaimAsync(string userId, Claim claim, UserManager<User> userManager)
        {
            return await userManager.RemoveClaimAsync(userId, claim);
        }

        public async Task<IList<Claim>> GetClaimsAsync(string userId, UserManager<User> userManager)
        {
            return await userManager.GetClaimsAsync(userId);
        }

        public async Task<IdentityResult> AddToRoleAsync(string userId, string role, UserManager<User> userManager)
        {
            return await userManager.AddToRoleAsync(userId, role);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(string userId, string role, UserManager<User> userManager)
        {
            return await userManager.RemoveFromRoleAsync(userId, role);
        }

        public async Task<IList<string>> GetRolesAsync(string userId, UserManager<User> userManager)
        {
            return await userManager.GetRolesAsync(userId);
        }

        public async Task<IdentityResult> DeleteAsync(User user, UserManager<User> userManager)
        {
            return await userManager.DeleteAsync(user);
        }

        public string HashPassword(string password, UserManager<User> userManager)
        {
            return userManager.PasswordHasher.HashPassword(password);
        }
    }
}