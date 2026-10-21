using System;
using System.Threading.Tasks;

namespace DaysOn.Auth.Domain.Application
{
    public interface ITeamService
    {
        Task AddEmployee(Guid userId, Guid teamId);
        Task RemoveTeamFromEmployee(Guid userId);
    }
}