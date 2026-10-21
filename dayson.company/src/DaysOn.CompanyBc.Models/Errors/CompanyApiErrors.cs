using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysOn.CompanyBc.Models.Errors
{
    public static class CompanyApiErrors
    {
        public const string CompanyIdIsNull = "Invalid claim, companyId is null.";
        public const string EmployeeIdIsNull = "Invalid claim, employeeId is null.";
        public const string TeamIdIsNull = "Invalid claim, teamId is null.";
    }
}
