using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using DaysOn.HolidaysBc.Models.Commands;

namespace DaysOn.Facade.Common
{
    public static class HolidayManagerClientWrapper
    {
        private const string AddHolidayRequestUri = "v1/manager/holidays/add";
        private const string CancelHolidayRequestUri = "v1/manager/holidays/cancel";
        private const string GetHolidayRequestUri = "v1/manager/holidays/{0}/employee/{1}";
        private const string GetHolidaysRequestUri = "v1/manager/holidays/employee/{0}";
        private const string GetHolidaysByYearRequestUri = "v1/manager/holidays/employee/{0}/year/{1}";
        private const string ApproveHolidayRequestUri = "v1/manager/holidays/approve";
        private const string RejectHolidayRequestUri = "v1/manager/holidays/reject";

        public static async Task<HttpResponseMessage> CreateHolidayAsync(AddHolidayCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["HolidayServer"]);

            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, AddHolidayRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddHolidayRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> CancelHolidayAsync(EmployeeHolidayCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["HolidayServer"]);

            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, CancelHolidayRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, CancelHolidayRequestUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetHolidayAsync(Guid employeId, Guid holidayId, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["HolidayServer"]);

            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            var uri = string.Format(GetHolidayRequestUri, holidayId, employeId);
            Console.WriteLine("FullUrl={0}{1}", baseUrl, uri);
            Console.WriteLine("AccessToken={0}", accessToken);

            var response = await HttpClientHelper.GetAsync(baseUrl, uri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetHolidaysAsync(Guid employeId, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["HolidayServer"]);

            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetHolidaysRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetHolidaysRequestUri, employeId), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetHolidaysByYearAsync(Guid employeId, int year, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["HolidayServer"]);

            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetHolidaysByYearRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetHolidaysByYearRequestUri, employeId, year), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> ApproveHolidayAsync(EmployeeHolidayCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["HolidayServer"]);

            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            Console.WriteLine("FullUrl={0}{1}", baseUrl, ApproveHolidayRequestUri);
            Console.WriteLine("AccessToken={0}", accessToken);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, ApproveHolidayRequestUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> RejectHolidayAsync(EmployeeHolidayCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["HolidayServer"]);

            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            Console.WriteLine("FullUrl={0}{1}", baseUrl, RejectHolidayRequestUri);
            Console.WriteLine("AccessToken={0}", accessToken);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, RejectHolidayRequestUri, accessToken);

            return response;
        }
    }
}
