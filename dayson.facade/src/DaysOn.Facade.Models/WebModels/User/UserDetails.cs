namespace DaysOn.Facade.Models.WebModels.User
{
    public class UserDetails : UserOverview
    {
        public UserRules Rules { get; set; }

        public string ApproverName { get; set; }
    }
}
