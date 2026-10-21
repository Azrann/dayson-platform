using System;
using System.Threading.Tasks;

namespace DaysOn.Auth.Domain.Application
{
    public interface IManagerOfTeamService  
    {
        Task AddManager(Guid userId, Guid teamId);
        Task RemoveManagerFromTeam(Guid userId, Guid teamId);
    }
}