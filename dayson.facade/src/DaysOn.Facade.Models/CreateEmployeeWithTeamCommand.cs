using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Models.Commands;

namespace DaysOn.Facade.Models
{
    public class CreateEmployeeWithTeamCommand : AddEmployeeCommand
    {
        public Guid TeamId { get; set; }
    }
}
