using DaysOn.Facade.Models.WebModels.Team;
using DaysOn.Facade.Models.WebModels.User;

namespace DaysOn.Facade.Models.WebModels.Holiday
{
    public class HolidayDetails : HolidayOverview
    {
        /*public TeamOverview Team { get; set; }  THAT is not necessary*/ 
        public UserOverview User { get; set; }
    }
}
