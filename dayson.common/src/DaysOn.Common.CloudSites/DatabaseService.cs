using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using DaysOn.Common.CloudSites.Models;
using DaysOn.Common.CloudSites.Responses;
using Newtonsoft.Json;

namespace DaysOn.Common.CloudSites
{
    public class DatabaseService
    {
        private const string BaseUrl = "https://api.gearhost.com/v1/";
        private const string GetDatabasesUrl = "databases";
        private const string CreateDatabasesUrl = "databases";
        private const string GetDatabaseUrl = "databases/{0}";
        private const string GetDbUsersUrl = "databases/{0}/users";
        private const string DeleteDatabaseUrl = "databases/{0}";

        public static async Task<List<Database>> GetDatabases()
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.GetAsync(BaseUrl, GetDatabasesUrl, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return (JsonConvert.DeserializeObject<GetDatabasesResponse>(response.Content.ReadAsStringAsync().Result)).Databases;
            }
            return new List<Database>();
        }

        public static async Task<Database> GetDatabase(string DatabaseId)
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.GetAsync(BaseUrl, string.Format(GetDatabaseUrl, DatabaseId), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return (JsonConvert.DeserializeObject<GetDatabaseResponse>(response.Content.ReadAsStringAsync().Result)).Database;
            }
            return new Database();
        }

        public static async Task<bool> Create(string name, DatabasePlan plan, DatabaseType type)
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.PostAsync(new 
            {
                name = name,
                plan = plan.ToString(),
                type = type.ToString()

            }, BaseUrl, CreateDatabasesUrl, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ActionResponse>(response.Content.ReadAsStringAsync().Result).success;
            }
            return false;
        }

        public static async Task<bool> Delete(string id)
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.DeleteAsync(BaseUrl, string.Format(DeleteDatabaseUrl,id), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ActionResponse>(response.Content.ReadAsStringAsync().Result).success;
            }
            return false;
        }

        public static async Task<List<DbUser>> GetUsers(string DatabaseId)
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.GetAsync(BaseUrl, string.Format(GetDbUsersUrl, DatabaseId), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return (JsonConvert.DeserializeObject<GetUsersResponse>(response.Content.ReadAsStringAsync().Result)).Users;
            }
            return new List<DbUser>();
        }
    }
}
