using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using DaysOn.HolidaysBc.Models.Commands;

namespace DaysOn.Facade.Common
{
    public static class YearlyHolidayCalculationClientWrapper
    {
        private const string GetYearlyHolidayCalculationRequestUri = "v1/calculations/year/{0}";
        private const string AdminGetYearlyHolidayCalculationRequestUri = "v1/admin/calculations/employee/{0}/year/{1}";
        private const string CarryOverDaysCommandUri = "v1/carryoverdays";
        private const string CorrectionDaysCommandUri = "v1/correctiondays";

        public static async Task<HttpResponseMessage> GetYearlyHolidayCalculationByEmployee(int year, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl,string.Format(GetYearlyHolidayCalculationRequestUri,year), token);

            return response;
        }

        public static async Task<HttpResponseMessage> AdminGetYearlyHolidayCalculationByEmployee(Guid employeeId, int year, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(AdminGetYearlyHolidayCalculationRequestUri, employeeId, year), token);

            return response;
        }

        public static async Task<HttpResponseMessage> CarryOverDays(CarryOverCommand command, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.PostAsync(command, baseUrl, CarryOverDaysCommandUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> CorrectionDays(CorrectionDaysCommand command, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.PostAsync(command, baseUrl, CorrectionDaysCommandUri, token);

            return response;
        }
    }
}
