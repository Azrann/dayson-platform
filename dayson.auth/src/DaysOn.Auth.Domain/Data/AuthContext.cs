using System.Data.Entity;
using DaysOn.Auth.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DaysOn.Auth.Domain.Data
{
    public class AuthContext : IdentityDbContext<User>
    {
        public AuthContext() : base("AuthDB")
        {
            
        }

        public DbSet<Audience> Audience { get; set; }

        public DbSet<ActivationCode> ActivationCodes { get; set; }

        public DbSet<ResetPasswordCode> ResetPasswordCodes { get; set; }
    }
}