using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysOn.Facade.Models
{
    public class TeamOverviewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public EmployeeOverviewDto Manager { get; set; }
        public EmployeeOverviewDto TemporalManager { get; set; }

        public virtual List<EmployeeOverviewDto> TeamMembers { get; set; }
    }
}
