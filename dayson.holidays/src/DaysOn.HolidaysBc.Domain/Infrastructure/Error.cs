namespace DaysOn.HolidaysBc.BoundedContext.Infrastructure
{
    public static class Error
    {
        public const string CompanyNotFound = "Company with Id {0} not found.";
        public const string TeamNotFound = "Team with Id {0} not found.";
        public const string EmployeeNotFound = "Employee with Id {0} not found.";
        public const string EmployeeNotFoundInCompany = "Employee with Id {0} not found in Company {1}.";
        public const string HolidayRequestNotFound = "HolidayRequest with Id {0} not found.";
        public const string ArgumentNull = "Argument {0} is null.";
        public const string ArgumentEmpty = "Argument {0} is empty.";
        public const string RuleNotFound = "Rule with Id {0} not found.";
        public const string GlobalRulesNotFound = "GlobalRules not found for company with Id {0}.";
        public const string GlobalRulesIsNull = "GlobalRules passed as argument is null";
        public const string TeamAlreadyExists = "Team with Id {0} already exists.";

        public const string CannotAddManagerBecauseIsTeamMember = "Cannot add the manager because is already a member of the team.";
        public const string CannotAddTeamMemberBecauseIsManager= "Cannot add the team member because is already the manager of the team.";
        public const string CannotCarryOverDaysBecuaseNotEnoughNumberOfDaysLeft = "NumberOfDaysLeft: {0} are smaller than the number of days you want to carry over {1}.";
        public const string MaxDaysToCarryForwardAre = "Maximum number of days to carry forward are {0}";
    }
}