using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.HolidaysBc.Models.Commands;

namespace DaysOn.Facade.Common
{
    public static class ApproverClientWrapper
    {
        private const string GetAllApproversRequestUri = "v1/company/approvers/all";
        private const string AddEmployeeToApproverRequestUri = "v1/company/approver/addEmployee";
        private const string RemoveEmployeeFromApproverRequestUri = "v1/company/approver/removeEmployee";
        private const string AddApproverRequestUri = "v1/company/approver/add";
        private const string RemoveApproverRequestUri = "v1/company/approver/remove";
        private const string GetEmployeesForApproverRequestUri = "v1/company/approver/{0}/getemployees";

        public static async Task<HttpResponseMessage> GetAllApproversAsync(string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            var response = await HttpClientHelper.GetAsync(baseUrl, GetAllApproversRequestUri, accessToken);
            return response;
        }

        public static async Task<HttpResponseMessage> AddEmployeeToApproverAsync(AddEmployeeToApproverCommand command, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddEmployeeToApproverRequestUri, token);
            return response;
        }

        public static async Task<HttpResponseMessage> RemoveEmployeeFromApprover(AddEmployeeToApproverCommand command, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            var response = await HttpClientHelper.PostAsync(command, baseUrl, RemoveEmployeeFromApproverRequestUri, token);
            return response;
        }

        public static async Task<HttpResponseMessage> AddApprover(Guid approverId, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            var response = await HttpClientHelper.PostAsync(approverId, baseUrl, AddApproverRequestUri, token);
            return response;
        }

        public static async Task<HttpResponseMessage> RemoveApprover(Guid approverId, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            var response = await HttpClientHelper.PostAsync(approverId, baseUrl, RemoveApproverRequestUri, token);
            return response;
        }

        public static async Task<HttpResponseMessage> GetEmployeesForApprover(Guid approverId, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["HolidayServer"];
            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetEmployeesForApproverRequestUri, approverId), token);
            return response;
        }
    }
}