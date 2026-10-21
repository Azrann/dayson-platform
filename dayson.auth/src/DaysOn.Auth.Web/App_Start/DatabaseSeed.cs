using System;
using DaysOn.Auth.Domain.Data;
using DaysOn.Auth.Domain.Entities;
using DaysOn.Auth.Web.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DaysOn.Auth.Web
{
    public class DatabaseSeed
    {
        public static void Run()
        {
            var userManager = new UserManager<User>(new UserStore<User>(new AuthContext()));
            var rolesManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AuthContext()));

            var megaAdminRole = rolesManager.FindByName("MegaAdmin");
            if (megaAdminRole == null)
            {
                SeedRoles(rolesManager);
            }
            var megaAdmin = userManager.FindByEmail("REPLACE_WITH_YOUR_VALUE");
            if (megaAdmin == null)
            {
                SeedMegaAdmin(userManager);
                megaAdmin = userManager.FindByEmail("REPLACE_WITH_YOUR_VALUE");
            }
            userManager.AddToRoleAsync(megaAdmin.Id, "MegaAdmin");

            var audience = AudiencesStore.FindAudience("REPLACE_WITH_YOUR_VALUE");
            if (audience == null)
            {
                AudiencesStore.AddAudience("CompanyBC", "REPLACE_WITH_YOUR_VALUE");
            }
        }

        private static void SeedMegaAdmin(UserManager<User> userManager)
        {
            var result = userManager.Create(GetMegaAdmin(), "REPLACE_WITH_YOUR_VALUE");
        }

        private static void SeedRoles(RoleManager<IdentityRole> rolesManager)
        {
            rolesManager.Create(new IdentityRole("MegaAdmin"));
            rolesManager.Create(new IdentityRole("SuperAdmin"));
            rolesManager.Create(new IdentityRole("Support"));

            rolesManager.Create(new IdentityRole("Owner"));
            rolesManager.Create(new IdentityRole("Admin"));
            rolesManager.Create(new IdentityRole("Manager"));
            rolesManager.Create(new IdentityRole("Finance"));
            rolesManager.Create(new IdentityRole("User"));
        }

        public static User GetMegaAdmin()
        {
            var result = new User(Guid.NewGuid().ToString(), "REPLACE_WITH_YOUR_VALUE", Guid.NewGuid().ToString());
            return result;
        }
    }
}