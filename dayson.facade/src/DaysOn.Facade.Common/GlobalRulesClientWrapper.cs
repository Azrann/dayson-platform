using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.Facade.Common
{
    public static class GlobalRulesClientWrapper
    {
        private const string GlobalRulesRequestUri = "v1/globalRules";
        private const string GetEmployeeGlobalRulesUri = "v1/employee/{0}/globalRules";
        private const string GetMyGlobalRulesUri = "v1/employee/globalRules";

        public static async Task<HttpResponseMessage> UpdateCompanyGlobalRulesAsync(GlobalRulesDto globalRules, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.PostAsync(globalRules, baseUrl, GlobalRulesRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> GetCompanyGlobalRulesAsync(string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, GlobalRulesRequestUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> SetEmployeeGlobalRules(Guid employeeId, GlobalRulesDto globalRules,string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.PostAsync(globalRules, baseUrl, string.Format(GetEmployeeGlobalRulesUri, employeeId), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetEmployeeGlobalRules(Guid employeeId, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetEmployeeGlobalRulesUri,employeeId), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetMyGlobalRules(string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetMyGlobalRulesUri), accessToken);

            return response;
        }
    }
}
