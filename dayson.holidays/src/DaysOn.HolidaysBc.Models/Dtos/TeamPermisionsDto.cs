using System;
using System.Collections.Generic;

namespace DaysOn.HolidaysBc.Models.Dtos
{
    public class TeamPermisionsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> Owners { get; set; }
    }
}
