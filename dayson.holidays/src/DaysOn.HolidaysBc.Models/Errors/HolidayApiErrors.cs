namespace DaysOn.HolidaysBc.Models.Errors
{
    public static class HolidayApiErrors
    {
        public const string CompanyIdIsNull = "Invalid claim, companyId is null.";
        public const string EmployeeIdIsNull = "Invalid claim, employeeId is null.";
        public const string EmployeeNotFound = "Employee with Id {0} not found.";
        public const string EmployeeNotInMyTeam = "Employee with Id {0} is not in my Team.";
        public const string RoleNotFound = "Role not found.";
        public const string HolidayRequestBetweenTwoYears =
            "Your holiday request is between years you will need to create two separate holiday request.One finishing on {0} and the second one starting on the {1}";
    }
}
