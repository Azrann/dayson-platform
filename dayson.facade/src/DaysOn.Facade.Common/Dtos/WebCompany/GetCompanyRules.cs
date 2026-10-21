using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.Company;

namespace DaysOn.Facade.Common.Dtos.WebCompany
{
    public class GetCompanyRulesRequest
    {
    }

    public class GetCompanyRulesResponse
    {
        public CompanyRules Rules { get; set; }
    }

    public class GetCompanyRulesOffline
    {

        public static async Task<GetCompanyRulesResponse> GetCompanyRules(GetCompanyRulesRequest request)
        {
            return await Task.FromResult(new GetCompanyRulesResponse
            {
                Rules = OfflineDb.CompanyRules.First(c => c.CompanyId == OfflineDb.HardcodedCompanyId)
            });
        }
    }

}
