using System;
using System.Collections.Generic;
using DaysOn.CompanyBc.Models.Dtos;

namespace DaysOn.Facade.Controllers
{
    public class Factory
    {
        public static List<TeamDto> GetMockTeams()
        {
            var result = new List<TeamDto>()
            {
                new TeamDto()
                {
                    Id = Guid.NewGuid(),
                    Name = "Red Team",
                    Manager = new IdNameDto()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Alberto"
                    }
                }
            };
            return result;
        }
    }
}