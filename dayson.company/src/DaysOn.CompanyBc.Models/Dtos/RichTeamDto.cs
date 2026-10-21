using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysOn.CompanyBc.Models.Dtos
{
    public class RichTeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ShortEmployeeDto Manager { get; set; }
        public ShortEmployeeDto TemporalManager { get; set; }

        public virtual List<ShortEmployeeDto> TeamMembers { get; set; }
    }
}
