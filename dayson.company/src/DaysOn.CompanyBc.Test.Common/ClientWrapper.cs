using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DaysOn.CompanyBc.BoundedContext.Entities;
using Newtonsoft.Json;

namespace DaysOn.CompanyBc.Test.Common
{
    public class ClientWrapper
    { 
        private const string CompanyMyRequestUri = "v1/company";
        private const string CreateTeamRequestUri = "v1/company/team";
        private const string CreateCompanyRequestUri = "v1/company";
        private const string CreateEmployeeRequestUri = "v1/company/employee";
        private const string GetTeamRequestUri = "v1/company/team/{0}";
        private const string GetTeamsRequestUri = "v1/company/teams";

        public static TokenResponse GetToken(string clientIdentity = "REPLACE_WITH_YOUR_VALUE")
        {
            const string requestUri = "v1/oauth";

            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["AuthServer"]);

            var baseUrl = ConfigurationManager.AppSettings["AuthServer"];

            var loginRequest = new
            {
                username = "REPLACE_WITH_YOUR_VALUE",
                password = "REPLACE_WITH_YOUR_VALUE",
                clientId = clientIdentity
            };

            Console.WriteLine("FullUrl={0}{1}", baseUrl, requestUri);

            var response = HttpClientHelper.Post(loginRequest, baseUrl, requestUri);

            string responseString = response.Content.ReadAsStringAsync().Result;
            
            return JsonConvert.DeserializeObject<TokenResponse>(responseString);
        }

        public static bool CreateCompany(string email, string companyName)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            var createCompanyRequest = new
            {
                AdminEmailAddress = email,
                Name = companyName
            };

            Console.WriteLine("FullUrl={0}{1}", baseUrl, CreateCompanyRequestUri);

            var response = HttpClientHelper.Post(createCompanyRequest, baseUrl, CreateCompanyRequestUri);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async static Task<IEnumerable<Team>> GetTeamsAsync(string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetTeamsRequestUri);

            var response = await HttpClientHelper.GetAsync(token, baseUrl, GetTeamsRequestUri);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Team>>(stringResult);
            }
            return null;
        }

        public async static Task<Team> GetTeamAsync(Guid teamId, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetTeamsRequestUri);

            var response = await HttpClientHelper.GetAsync(token, baseUrl, string.Format(GetTeamRequestUri, teamId));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Team>(stringResult);
            }
            return null;
        }
    }
}
