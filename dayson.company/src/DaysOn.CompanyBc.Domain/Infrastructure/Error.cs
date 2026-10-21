namespace DaysOn.CompanyBc.BoundedContext.Infrastructure
{
    public static class Error
    {
        public const string CompanyNotFound = "Company with Id {0} not found.";
        public const string TeamNotFound = "Team with Id {0} not found.";
        public const string EmployeeNotFound = "Employee with Id {0} not found.";
        public static string ArgumentNull = "Argument {0} is null.";

        public static string CannotAddManagerBecauseIsTeamMember = "Cannot add the manager because is already a member of the team.";
        public static string CannotAddTeamMemberBecauseIsManager= "Cannot add the team member because is already the manager of the team.";
    }
}