using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.Company;

namespace DaysOn.Facade.Common.Dtos.WebCompany
{
    public class GetCompanyOverviewRequest
    {
    }

    public class GetCompanyOverviewResponse
    {
        public CompanyOverview Company { get; set; }
    }

    public class GetCompanyOverviewOffline
    {
        public static async Task<GetCompanyOverviewResponse> GetCompanyOverview(GetCompanyOverviewRequest request)
        {
            return await Task.FromResult(new GetCompanyOverviewResponse
            {
                Company = OfflineDb.Companies.First(c => c.Id == OfflineDb.HardcodedCompanyId)
            });
        }
    }

}
