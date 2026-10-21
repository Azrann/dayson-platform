using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using DaysOn.HolidaysBc.Models.Commands;

namespace DaysOn.Facade.Common
{
    public static class OpenCloseDatesClientWrapper
    {
        private const string GetByYearUri = "v1/openCloseDates/year/{0}";
        private const string GetAllByYearUri = "v1/openCloseDates/all/year/{0}";
        private const string GetRecurrentUri = "v1/openCloseDates";
        private const string AddRecurrentClosedDayUri = "v1/openCloseDates/addRecurrent/ClosedDay";
        private const string AddRecurrentClosedHalfDayUri = "v1/openCloseDates/addRecurrent/ClosedHalfDay";
        private const string AddClosedHalfDayUri = "v1/openCloseDates/add/ClosedHalfDay";
        private const string AddClosedDayUri = "v1/openCloseDates/add/ClosedDay";
        private const string AddClosedDaysUri = "v1/openCloseDates/add/ClosedDays";
        private const string AddRecurrentClosedDaysUri = "v1/openCloseDates/addRecurrent/ClosedDays";


        public static async Task<HttpResponseMessage> GetByYear(int year, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetByYearUri, year), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetAllByYear(int year, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetAllByYearUri, year), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetRecurrent(string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, GetRecurrentUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> AddRecurrentClosedDay(AddDateCommand command, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddRecurrentClosedDayUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> AddRecurrentClosedHalfDay(AddDateCommand command, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddRecurrentClosedHalfDayUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> AddClosedHalfDay(AddDateCommand command, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddClosedHalfDayUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> AddClosedDay(AddDateCommand command, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddClosedDayUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> AddClosedDays(AddDatesCommand command, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddClosedDaysUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> AddRecurrentClosedDays(AddRecurrentDatesCommand command, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddRecurrentClosedDaysUri, accessToken);

            return response;
        }
    }
}
