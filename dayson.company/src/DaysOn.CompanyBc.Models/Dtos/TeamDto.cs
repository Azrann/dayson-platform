using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysOn.CompanyBc.Models.Dtos
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IdNameDto Manager { get; set; }
        public IdNameDto TemporalManager { get; set; }

        public virtual List<IdNameDto> TeamMembers { get; set; }
    }
}
