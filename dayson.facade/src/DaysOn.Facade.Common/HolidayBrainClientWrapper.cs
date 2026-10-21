using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using DaysOn.HolidaysBc.Models.Commands;

namespace DaysOn.Facade.Common
{
    public static class HolidayBrainClientWrapper
    {
        private const string GetBusinessDaysRequestUri = "v1/brain/businessDays";
        private const string AddHolidayRequestUri = "v1/brain/holidays";

        public static async Task<HttpResponseMessage> GetBusinessDaysCommandAsync(GetBusinessDaysCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["HolidayServer"]);

            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetBusinessDaysRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, GetBusinessDaysRequestUri, token);

            return response;
        }
    }
}
