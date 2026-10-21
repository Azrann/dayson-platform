using DaysOn.Auth.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DaysOn.Auth.Domain.Application
{
    public interface IUserManagerFactory
    {
        UserManager<User> CreateUserManager();

        UserStore<User> CreateUserStore();
    }
}