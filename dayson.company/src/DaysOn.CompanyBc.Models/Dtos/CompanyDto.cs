using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysOn.CompanyBc.Models.Dtos
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<IdNameDto> Teams { get; set; }
        public List<IdNameDto> Employees { get; set; }
    }

    public class MyCompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerId { get; set; }
    }
}
