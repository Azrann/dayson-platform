using System;
using PubnubApi;

namespace DaysOn.Auth.Domain.Services
{
    public interface IUserIdService
    {
        string GetUserIdByEmail(string email);
    }
}