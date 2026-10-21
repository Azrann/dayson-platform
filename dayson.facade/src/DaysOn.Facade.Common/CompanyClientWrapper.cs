using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.CompanyBc.Models.Dtos;
using DaysOn.Facade.Common.Dtos;
using Newtonsoft.Json;

namespace DaysOn.Facade.Common
{
    public static class CompanyClientWrapper
    {
        private const string CompanyMyRequestUri = "v1/company";
        private const string CreateTeamRequestUri = "v1/company/team";
        private const string UpdateTeamRequestUri = "v1/company/team/update";
        private const string CreateCompanyRequestUri = "v1/company";
        private const string CreateEmployeeRequestUri = "v1/company/employee/add";
        private const string GetMyDetailsRequestUri = "v1/company/employee";
        private const string EmployeeRequestUri = "v1/company/employee/{0}";
        private const string GetTeamRequestUri = "v1/company/team/{0}";
        private const string GetMyTeamRequestUri = "v1/company/team";
        private const string RemoveTeamRequestUri = "v1/company/team/remove"; 
        private const string GetTeamsRequestUri = "v1/company/teams";
        private const string GetSimpleTeamsRequestUri = "v1/company/team/all";
        private const string GetRichTeamsRequestUri = "v1/company/richteams";
        private const string AddEmployeeToATeamRequestUri = "v1/company/team/addEmployee";
        private const string AddManagerRequestUri = "v1/company/team/addManager";
        private const string AddTemporalManagerRequestUri = "v1/company/team/addTemporalManager";
        private const string RemoveManagerRequestUri = "v1/company/team/removeManager";
        private const string RemoveTemporalManagerRequestUri = "v1/company/team/removeTemporalManager";
        private const string RemoveEmployeeToATeamRequestUri = "v1/company/team/removeEmployee";
        private const string UpdateEmployeeDetailsRequestUri = "v1/company/employee/update";
        private const string UpdateMyDetailsRequestUri = "v1/company/employee/updateMyDetails";
        private const string SearchEmployeeUri = "v1/company/search";

        public static async Task<Guid> CreateCompanyAsync(CreateCompanyCommand command)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, CreateCompanyRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, CreateCompanyRequestUri);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Guid>(response.Content.ReadAsStringAsync().Result);
            }
            return Guid.Empty;
        }

        public static async Task<Guid> CreateTeamAsync(string teamName, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

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

        public static async Task<Guid> UpdateTeamAsync(Guid teamId, string teamName, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            var updateTeamRequest = new
            {
                Name = teamName,
                Id = teamId
            };

            Console.WriteLine("FullUrl={0}{1}", baseUrl, UpdateTeamRequestUri);

            var response = await HttpClientHelper.PostAsync(updateTeamRequest, baseUrl, UpdateTeamRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Guid>(stringResult);
            }
            return Guid.Empty;
        }

        public static async Task<HttpResponseMessage> RemoveTeamAsync(Guid teamId, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, CreateTeamRequestUri);

            return await HttpClientHelper.PostAsync(teamId, baseUrl, RemoveTeamRequestUri, token);
        }

        public static async Task<Guid> CreateEmployeeAsync(AddEmployeeCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, CreateEmployeeRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, CreateEmployeeRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Guid>(stringResult);
            }
            return Guid.Empty;
        }

        public static async Task<EmployeeDto> GetMyDetailsEmployeeAsync(string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetMyDetailsRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, GetMyDetailsRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EmployeeDto>(stringResult);
            }
            return null;
        }

        public static async Task<EmployeeDto>GetEmployeeAsync(Guid employeeId, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, string.Format(EmployeeRequestUri, employeeId));

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(EmployeeRequestUri, employeeId), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EmployeeDto>(stringResult);
            }
            return null;
        }

        public static async Task<HttpResponseMessage> UpdateEmployeeDetails(UpdateEmployeeCommand command, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];
            var response = await HttpClientHelper.PostAsync(command, baseUrl, UpdateEmployeeDetailsRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> UpdateMyDetails(UpdateEmployeeCommand command, string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];
            var response = await HttpClientHelper.PostAsync(command, baseUrl, UpdateMyDetailsRequestUri, token);

            return response;
        }

        public static async Task<IEnumerable<TeamDto>> GetTeamsAsync(string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetTeamsRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, GetTeamsRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TeamDto>>(stringResult);
            }
            return null;
        }

        public static async Task<IEnumerable<IdNameDto>> GetSimpleTeamsAsync(string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetTeamsRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, GetSimpleTeamsRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<IdNameDto>>(stringResult);
            }
            return null;
        }

        public static async Task<IEnumerable<RichTeamDto>> GetRichTeamsAsync(string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetTeamsRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, GetRichTeamsRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<RichTeamDto>>(stringResult);
            }
            return null;
        }

        public static async Task<TeamDto> GetTeamAsync(Guid teamId, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, GetTeamsRequestUri);

            var response = await HttpClientHelper.GetAsync(baseUrl, string.Format(GetTeamRequestUri, teamId), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TeamDto>(stringResult);
            }
            return null;
        }

        public static async Task<TeamDto> GetMyTeamAsync(string token)
        {
            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            var response = await HttpClientHelper.GetAsync(baseUrl, GetMyTeamRequestUri, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TeamDto>(stringResult);
            }
            return null;
        }

        public static async Task<HttpResponseMessage> AddEmployeeToATeamAsync(AddEmployeeToATeamCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, AddEmployeeToATeamRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddEmployeeToATeamRequestUri,token);

            return response;
        }

        public static async Task<List<EmployeeDto>> SearchAsync(SearchUsersRequest command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, SearchEmployeeUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, SearchEmployeeUri, token);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Not OK received from server");
            }

            var stringResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<EmployeeDto>>(stringResult);
        }

        public static async Task<HttpResponseMessage> RemoveEmployeeFromATeamCommand(RemoveEmployeeFromATeamCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, RemoveEmployeeToATeamRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, RemoveEmployeeToATeamRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> AddManager(AddManagerCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, AddManagerRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddManagerRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> AddTemporalManager(AddTemporalManagerCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, AddTemporalManagerRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, AddTemporalManagerRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> RemoveManager(RemoveManagerCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, RemoveManagerRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, RemoveManagerRequestUri, token);

            return response;
        }

        public static async Task<HttpResponseMessage> RemoveTemporalManager(RemoveTemporalManagerCommand command, string token)
        {
            Console.WriteLine("REPLACE_WITH_YOUR_VALUE", ConfigurationManager.AppSettings["CompanyServer"]);

            var baseUrl = ConfigurationManager.AppSettings["CompanyServer"];

            Console.WriteLine("FullUrl={0}{1}", baseUrl, RemoveTemporalManagerRequestUri);

            var response = await HttpClientHelper.PostAsync(command, baseUrl, RemoveTemporalManagerRequestUri, token);

            return response;
        }
    }
}
