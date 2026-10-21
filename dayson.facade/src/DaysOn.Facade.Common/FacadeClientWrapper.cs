using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.Facade.Models;
using DaysOn.HolidaysBc.Models.Commands;
using Newtonsoft.Json;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.Facade.Common
{
    public static class FacadeClientWrapper
    {
        private const string CompanyMyRequestUri = "v1/company";
        private const string CreateCompanyRequestUri = "v1/company";
        private const string CreateTeamRequestUri = "v1/company/team";
        private const string RemoveTeamRequestUri = "v1/company/team/remove";
        private const string CreateEmployeeRequestUri = "v1/company/employee/add";
        private const string GetMyDetailsRequestUri = "v1/company/employee";
        private const string GetEmployeeRequestUri = "v1/company/employee/{0}";
        private const string GetTeamRequestUri = "v1/company/team/{0}";
        private const string GetTeamsRequestUri = "v1/company/teams";
        private const string GetSimpleTeamsRequestUri = "v1/company/team/all";
        private const string GetRichTeamsRequestUri = "user/teamsOverview";
        private const string AddEmployeeToATeamRequestUri = "v1/company/team/addEmployee";
        private const string RemoveEmployeeToATeamRequestUri = "v1/company/team/removeEmployee";
        private const string AddManagerRequestUri = "v1/company/team/addManager";
        private const string AddTemporalManagerRequestUri = "v1/company/team/addTemporalManager";
        private const string RemoveManagerRequestUri = "v1/company/team/removeManager";
        private const string RemoveTemporalManagerRequestUri = "v1/company/team/removeTemporalManager";
        private const string AddHolidayRequestUri = "v1/holidays/add";
        private const string CancelHolidayRequestUri = "v1/holidays/cancel";
        private const string GetHolidayRequestUri = "v1/holidays/{0}/employee/{1}";
        private const string GetHolidaysRequestUri = "v1/holidays/employee/{0}";
        private const string GetHolidaysByYearRequestUri = "v1/holidays/employee/{0}/year/{1}";
        private const string GlobalRulesRequestUri = "v1/globalRules";
        private const string EmployeeGlobalRulesUri = "v1/employee/{0}/globalRules";
        private const string MyGlobalRulesUri = "v1/employee/globalRules";
        private const string GetYearlyHolidayCalculationRequestUri = "v1/calculations/year/{0}";
        private const string AdminGetYearlyHolidayCalculationRequestUri = "v1/admin/calculations/employee/{0}/year/{1}";
        private const string CarryOverDaysCommandUri = "v1/carryoverdays";
        private const string CorrectionDaysCommandUri = "v1/correctiondays";
        private const string AddHolidayRequestAdminUri = "v1/admin/holidays/add";
        private const string CancelHolidayRequestAdminUri = "v1/admin/holidays/cancel";
        private const string GetHolidayRequestAdminUri = "v1/admin/holidays/{0}/employee/{1}";
        private const string GetHolidaysRequestAdminUri = "v1/admin/holidays/employee/{0}";
        private const string GetHolidaysByYearRequestAdminUri = "v1/admin/holidays/employee/{0}/year/{1}";
        private const string ApproveHolidaysRequestAdminUri = "v1/admin/holidays/approve";
        private const string RejectHolidaysRequestAdminUri = "v1/admin/holidays/reject";
        private const string AddHolidayRequestManagerUri = "v1/manager/holidays/add";
        private const string CancelHolidayRequestManagerUri = "v1/manager/holidays/cancel";
        private const string GetHolidayRequestManagerUri = "v1/manager/holidays/{0}/employee/{1}";
        private const string GetHolidaysRequestManagerUri = "v1/manager/holidays/employee/{0}";
        private const string ApproveHolidaysRequestManagerUri = "v1/manager/holidays/approve";
        private const string RejectHolidaysRequestManagerUri =  "v1/manager/holidays/reject";
        private const string GetHolidaysByYearRequestManagerUri = "v1/manager/holidays/employee/{0}/year/{1}";
        private const string GetBusinessDaysRequestAdminUri = "v1/brain/businessDays";
        private const string AddHolidayRequestCommandUri = "v1/holidays/add";
        private const string GetTeamsOverviewUri = "v1/user/teamsOverview";
        private const string AddUserWithTeamUri = "v1/user/create";
        private const string GetAllApproversRequestUri = "v1/company/approvers/all";
        private const string AddEmployeeToApproverRequestUri = "v1/company/approver/addEmployee";
        private const string RemoveEmployeeFromApproverRequestUri = "v1/company/approver/removeEmployee";
        private const string AddApproverRequestUri = "v1/company/approver/add";
        private const string RemoveApproverRequestUri = "v1/company/approver/remove";
        private const string GetEmployeesForApproverRequestUri = "v1/company/approver/{0}/getemployees";

        public static async Task<Guid> CreateTeamAsync(string teamName, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var createTeamRequest = new
            {
                Name = teamName
            };

            Console.WriteLine("FullUrl={0}{1}", baseUrl, CreateTeamRequestUri);

            var response = await HttpClientHelper.PostAsync(createTeamRequest, baseUrl, CreateTeamRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Guid>(stringResult);
            }
            return Guid.Empty;
        }

        public static async Task<HttpResponseMessage> RemoveTeamAsync(Guid teamId, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, RemoveTeamRequestUri);

           return await HttpClientHelper.PostAsync(teamId, baseUrl, RemoveTeamRequestUri, token);
        }

        public static async Task<Guid> CreateEmployeeAsync(AddEmployeeCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, CreateEmployeeRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, CreateEmployeeRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Guid>(stringResult);
            }
            return Guid.Empty;
        }

        public static async Task<CompanyBc.Models.Dtos.EmployeeDto> GetEmployeeAsync(Guid employeeId, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, string.Format(GetEmployeeRequestUri, employeeId));

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetEmployeeRequestUri, employeeId), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CompanyBc.Models.Dtos.EmployeeDto>(stringResult);
            }
            return null;
        }

        public static async Task<CompanyBc.Models.Dtos.EmployeeDto> GetMyDetailsAsync(string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetMyDetailsRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, GetMyDetailsRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CompanyBc.Models.Dtos.EmployeeDto>(stringResult);
            }
            return null;
        }

        public static async Task<CompanyBc.Models.Dtos.CompanyDto> GetMyCompanyAsync(string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, CompanyMyRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CompanyBc.Models.Dtos.CompanyDto>(stringResult);
            }
            return null;
        }

        public static async Task<TokenResponse> CreateCompanyAsync(CreateCompanyCommand command)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.PostAsync(command,baseUrl, CreateCompanyRequestUri);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TokenResponse>(stringResult);
            }
            return null;
        }

        public static async Task<IEnumerable<CompanyBc.Models.Dtos.TeamDto>> GetTeamsAsync(string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetTeamsRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, GetTeamsRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<CompanyBc.Models.Dtos.TeamDto>>(stringResult);
            }
            return null;
        }

        public static async Task<IEnumerable<CompanyBc.Models.Dtos.RichTeamDto>> GetRichTeamsAsync(string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetRichTeamsRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, GetRichTeamsRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<CompanyBc.Models.Dtos.RichTeamDto>>(stringResult);
            }
            return null;
        }

        public static async Task<SimpleTeamsDto> GetSimpleTeamsAsync(string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetSimpleTeamsRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, GetSimpleTeamsRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SimpleTeamsDto>(stringResult);
            }
            return null;
        }

        public static async Task<CompanyBc.Models.Dtos.TeamDto> GetTeamAsync(Guid teamId, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetTeamsRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetTeamRequestUri, teamId), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CompanyBc.Models.Dtos.TeamDto>(stringResult);
            }
            return null;
        }

        public static async Task<HttpResponseMessage> AddEmployeeToATeam(AddEmployeeToATeamCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, AddEmployeeToATeamRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddEmployeeToATeamRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> RemoveEmployeeFromATeamCommand(RemoveEmployeeFromATeamCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, RemoveEmployeeToATeamRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, RemoveEmployeeToATeamRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> AddManager(AddManagerCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, AddManagerRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddManagerRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> AddTemporalManager(AddTemporalManagerCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, AddTemporalManagerRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddTemporalManagerRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> RemoveManager(RemoveManagerCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, RemoveManagerRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, RemoveManagerRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> RemoveTemporalManager(RemoveTemporalManagerCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, RemoveTemporalManagerRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, RemoveTemporalManagerRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> CreateHolidayAsync(AddHolidayCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, AddHolidayRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddHolidayRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> CancelHolidayAsync(EmployeeHolidayCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, CancelHolidayRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, CancelHolidayRequestUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetHolidayAsync(Guid employeId, Guid holidayId, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            var uri = string.Format(GetHolidayRequestUri, holidayId, employeId);
            Console.WriteLine("FullUrl={0}{1}", baseUrl, uri);
            Console.WriteLine("AccessToken={0}", accessToken);

            var response = await HttpClientHelper.GetAsync(baseUrl, uri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetHolidaysAsync(Guid employeId, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetHolidaysRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetHolidaysRequestUri, employeId), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetHolidaysByYearAsync(Guid employeId, int year, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetHolidaysByYearRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetHolidaysByYearRequestUri, employeId, year), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> UpdateCompanyGlobalRulesAsync(GlobalRulesDto globalRules, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.PostAsync(globalRules, baseUrl, GlobalRulesRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> GetCompanyGlobalRulesAsync(string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, GlobalRulesRequestUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> SetEmployeeGlobalRules(Guid employeeId, GlobalRulesDto globalRules, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.PostAsync(globalRules, baseUrl, string.Format(EmployeeGlobalRulesUri, employeeId), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetEmployeeGlobalRules(Guid employeeId, string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(EmployeeGlobalRulesUri, employeeId), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetMyGlobalRules(string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, MyGlobalRulesUri, accessToken);

            return response;
        }
        
        public static async Task<HttpResponseMessage> GetYearlyHolidayCalculationByEmployee(int year, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetYearlyHolidayCalculationRequestUri, year), token);

            return response;
        }

        public static async Task<HttpResponseMessage> AdminGetYearlyHolidayCalculationByEmployee(Guid employeeId, int year, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(AdminGetYearlyHolidayCalculationRequestUri, employeeId, year), token);

            return response;
        }


        public static async Task<HttpResponseMessage> CarryOverDays(CarryOverCommand command, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.PostAsync(command, baseUrl, CarryOverDaysCommandUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> CorrectionDays(CorrectionDaysCommand command, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.PostAsync(command, baseUrl, CorrectionDaysCommandUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> AdminCreateHolidayAsync(AddHolidayCommand command, string token, int milliseconds = 150)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, AddHolidayRequestAdminUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddHolidayRequestAdminUri, token);

            Thread.Sleep(milliseconds);
            return response;
        }

        public static async Task<HttpResponseMessage> AdminCancelHolidayAsync(EmployeeHolidayCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, CancelHolidayRequestAdminUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, CancelHolidayRequestAdminUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> AdminGetHolidayAsync(Guid employeId, Guid holidayId, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            var uri = string.Format(GetHolidayRequestAdminUri, holidayId, employeId);
            Console.WriteLine("FullUrl={0}{1}", baseUrl, uri);
            Console.WriteLine("AccessToken={0}", accessToken);

            var response = await HttpClientHelper.GetAsync(baseUrl, uri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> AdminGetHolidaysAsync(Guid employeId, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetHolidaysRequestAdminUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetHolidaysRequestAdminUri, employeId), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> AdminGetHolidaysByYearAsync(Guid employeId, int year, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetHolidaysByYearRequestAdminUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetHolidaysByYearRequestAdminUri, employeId, year), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> AdminApprovetHolidayAsync(EmployeeHolidayCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            Console.WriteLine("FullUrl={0}{1}", baseUrl, ApproveHolidaysRequestAdminUri);
            Console.WriteLine("AccessToken={0}", accessToken);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, ApproveHolidaysRequestAdminUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> AdminRejectHolidayAsync(EmployeeHolidayCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            Console.WriteLine("FullUrl={0}{1}", baseUrl, RejectHolidaysRequestAdminUri);
            Console.WriteLine("AccessToken={0}", accessToken);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, RejectHolidaysRequestAdminUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> ManagerCreateHolidayAsync(AddHolidayCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, AddHolidayRequestManagerUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddHolidayRequestManagerUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> ManagerCancelHolidayAsync(EmployeeHolidayCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, CancelHolidayRequestManagerUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, CancelHolidayRequestManagerUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> ManagerGetHolidayAsync(Guid employeId, Guid holidayId, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            var uri = string.Format(GetHolidayRequestManagerUri, holidayId, employeId);
            Console.WriteLine("FullUrl={0}{1}", baseUrl, uri);
            Console.WriteLine("AccessToken={0}", accessToken);

            var response = await HttpClientHelper.GetAsync(baseUrl, uri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> ManagerApprovetHolidayAsync(EmployeeHolidayCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            Console.WriteLine("FullUrl={0}{1}", baseUrl, ApproveHolidaysRequestManagerUri);
            Console.WriteLine("AccessToken={0}", accessToken);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, ApproveHolidaysRequestManagerUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> ManagerRejectHolidayAsync(EmployeeHolidayCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            Console.WriteLine("FullUrl={0}{1}", baseUrl, RejectHolidaysRequestManagerUri);
            Console.WriteLine("AccessToken={0}", accessToken);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, RejectHolidaysRequestManagerUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> ManagerGetHolidaysAsync(Guid employeId, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetHolidaysRequestManagerUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetHolidaysRequestManagerUri, employeId), accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> ManagerGetHolidaysByYearAsync(Guid employeId, int year, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetHolidaysByYearRequestManagerUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetHolidaysByYearRequestManagerUri, employeId, year), accessToken);

            return response;
        }
        public static async Task<HttpResponseMessage> GetBusinessDaysCommandAsync(GetBusinessDaysCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetBusinessDaysRequestAdminUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, GetBusinessDaysRequestAdminUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> AddHolidayRequestCommandAsync(AddHolidayRequestCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, AddHolidayRequestCommandUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddHolidayRequestCommandUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetTeamsOverview(string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, GetTeamsOverviewUri, accessToken);

            return response;
        }

        public static async Task<HttpResponseMessage> GetAllApproversAsync(string accessToken)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            var response = await HttpClientHelper.GetAsync(baseUrl, GetAllApproversRequestUri, accessToken);
            return response;
        }

        public static async Task<HttpResponseMessage> AddEmployeeToApproverAsync(AddEmployeeToApproverCommand command, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddEmployeeToApproverRequestUri, token);
            return response;
        }

        public static async Task<HttpResponseMessage> RemoveEmployeeFromApproverAsync(AddEmployeeToApproverCommand command, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            var response = await HttpClientHelper.PostAsync(command, baseUrl, RemoveEmployeeFromApproverRequestUri, token);
            return response;
        }

        public static async Task<HttpResponseMessage> AddApprover(Guid approverId, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            var response = await HttpClientHelper.PostAsync(approverId, baseUrl, AddApproverRequestUri, token);
            return response;
        }

        public static async Task<HttpResponseMessage> RemoveApprover(Guid approverId, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            var response = await HttpClientHelper.PostAsync(approverId, baseUrl, RemoveApproverRequestUri, token);
            return response;
        }

        public static async Task<HttpResponseMessage> GetEmployeesForApprover(Guid approverId, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];
            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetEmployeesForApproverRequestUri, approverId), token);
            return response;
        }

        public static async Task<HttpResponseMessage> AddEmployeeWithTeam(CreateEmployeeWithTeamCommand command, string accessToken)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["FacadeServer"]);

            var baseUrl = ConfigurationManager.AppSettings["FacadeServer"];

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddUserWithTeamUri, accessToken);

            return response;
        }

        public class SimpleTeamsDto
        {
            public List<CompanyBc.Models.Dtos.IdNameDto> Teams { get; set; }
        }
    }
}
