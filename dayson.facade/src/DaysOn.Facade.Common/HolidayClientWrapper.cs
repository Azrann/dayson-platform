using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Dtos.WebHoliday;
using DaysOn.HolidaysBc.Models.Commands;
using DaysOn.HolidaysBc.Models.Dtos;
using Newtonsoft.Json;

namespace DaysOn.Facade.Common
{
    public static class HolidayClientWrapper
    {
        private const string AddHolidayRequestUri = "v1/holidays/add";
        private const string CancelHolidayRequestUri = "v1/holidays/cancel";
        private const string GetHolidayRequestUri = "v1/holidays/{0}/employee/{1}";
        private const string GetHolidaysRequestUri = "v1/holidays/employee/{0}";
        private const string GetHolidaysByYearRequestUri = "v1/holidays/employee/{0}/year/{1}";
        private const string ChangeHolidayStatusRequestUri = "v1/holidays/changeHolidayStatus";
        private const string SearchHolidaysRequestUri = "v1/holidays/searchHolidays";

        public static Task<HttpResponseMessage> CreateHolidayAsync(AddHolidayRequestCommand command, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            return HttpClientHelper.PostAsync(command, baseUrl, AddHolidayRequestUri, accessToken);
        }

        public static Task<HttpResponseMessage> ChangeHolidayStatusAsync(Dtos.WebHoliday.ChangeHolidayStatusRequest command,
            string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            return HttpClientHelper.PostAsync(command, baseUrl, ChangeHolidayStatusRequestUri, accessToken);
        }

        public static Task<HttpResponseMessage> CancelHolidayAsync(EmployeeHolidayCommand command, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            return HttpClientHelper.PostAsync(command, baseUrl, CancelHolidayRequestUri, accessToken);
        }

        public static Task<HttpResponseMessage> GetHolidayAsync(Guid employeId, Guid holidayId, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            var uri = string.Format(GetHolidayRequestUri, holidayId, employeId);
            
            return HttpClientHelper.GetAsync(baseUrl, uri, accessToken);
        }

        public static Task<HttpResponseMessage> GetHolidaysAsync(Guid employeId, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            return HttpClientHelper.GetAsync(baseUrl, string.Format(GetHolidaysRequestUri, employeId), accessToken);
        }

        public static Task<HttpResponseMessage> GetHolidaysByYearAsync(Guid employeId, int year, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            return HttpClientHelper.GetAsync(baseUrl, string.Format(GetHolidaysByYearRequestUri, employeId, year), accessToken);
        }

        public static Task<HttpResponseMessage> SearchHolidays(Dtos.WebHoliday.SearchHolidaysRequest request, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            return HttpClientHelper.PostAsync(request, baseUrl, SearchHolidaysRequestUri, accessToken);
        }
    }
}
