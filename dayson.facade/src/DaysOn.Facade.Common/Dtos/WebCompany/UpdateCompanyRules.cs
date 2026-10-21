using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.Company;

namespace DaysOn.Facade.Common.Dtos.WebCompany
{
    public class UpdateCompanyRulesRequest
    {
        public CompanyRules Rules { get; set; }
    }

    public class UpdateCompanyRulesResponse
    {
        public CompanyRules Rules { get; set; }
    }

    public class UpdateCompanyRulesOffline
    {

        public static async Task<UpdateCompanyRulesResponse> UpdateCompanyRules(UpdateCompanyRulesRequest request)
        {
            CompanyRules rules = OfflineDb.CompanyRules.First(c => c.CompanyId == OfflineDb.HardcodedCompanyId);
            rules.HolidayEntitlement = request.Rules.HolidayEntitlement;
            rules.MaxConsecutiveDaysOff = request.Rules.MaxConsecutiveDaysOff;
            rules.MaxDaysCarriedOverNextYear = request.Rules.MaxDaysCarriedOverNextYear;
            rules.MinDaysAdvanceNoticeHolidays = request.Rules.MinDaysAdvanceNoticeHolidays;
            rules.WorkingDays = request.Rules.WorkingDays;
            rules.YearStartDay = request.Rules.YearStartDay;

            return await Task.FromResult(new UpdateCompanyRulesResponse
            {
                Rules = rules
            });
        }
    }

}
