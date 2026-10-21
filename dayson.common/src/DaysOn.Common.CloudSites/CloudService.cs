using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;
using DaysOn.Common.CloudSites.Models;
using DaysOn.Common.CloudSites.Responses;

namespace DaysOn.Common.CloudSites
{
    public class CloudService
    {
        private const string BaseUrl = "https://api.gearhost.com/v1/";
        private const string GetSitesUrl = "cloudsites";
        private const string CreateSitesUrl = "cloudsites";
        private const string GetSiteUrl = "cloudsites/{0}";
        private const string DeleteSiteUrl = "cloudsites/{0}";
        private const string RestartSiteUrl = "cloudsites/{0}/restart";
        private const string StopSiteUrl = "cloudsites/{0}/stop";
        private const string StartSiteUrl = "cloudsites/{0}/start";
        private const string ScaleSiteUrl = "cloudSites/{0}/scale/{1}";

        public static async Task<List<CloudSite>> GetSites()
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.GetAsync(BaseUrl, GetSitesUrl, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return (JsonConvert.DeserializeObject<GetSitesResponse>(response.Content.ReadAsStringAsync().Result)).cloudSites;
            }
            return new List<CloudSite>();
        }

        public static async Task<CloudSite> GetSite(string siteId)
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.GetAsync(BaseUrl, string.Format(GetSiteUrl, siteId), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return (JsonConvert.DeserializeObject<GetSiteResponse>(response.Content.ReadAsStringAsync().Result)).cloudSite;
            }
            return new CloudSite();
        }

        public static async Task<bool> Restart(string id)
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.PostAsync(new object(), BaseUrl, string.Format(RestartSiteUrl, id), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ActionResponse>(response.Content.ReadAsStringAsync().Result).success;
            }
            return false;
        }

        public static async Task<bool> Stop(string id)
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.PostAsync(new object(), BaseUrl, string.Format(StopSiteUrl,id), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ActionResponse>(response.Content.ReadAsStringAsync().Result).success;
            }
            return false;
        }

        public static async Task<bool> Start(string id)
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.PostAsync(new object(), BaseUrl, string.Format(StartSiteUrl, id), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ActionResponse>(response.Content.ReadAsStringAsync().Result).success;
            }
            return false;
        }

        public static async Task<bool> ScaleTo(string id, CloudSitePlan plan)
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.PostAsync(new object(), BaseUrl, string.Format(ScaleSiteUrl, id, plan.ToString()), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ActionResponse>(response.Content.ReadAsStringAsync().Result).success;
            }
            return false;
        }

        public static async Task<bool> Create(string name, CloudSitePlan plan)
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.PostAsync(new 
            {
                name = name,
                plan = plan.ToString()

            }, BaseUrl, CreateSitesUrl, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ActionResponse>(response.Content.ReadAsStringAsync().Result).success;
            }
            return false;
        }

        public static async Task<bool> Delete(string id)
        {
            var token = ConfigurationManager.AppSettings["gearhostkey"];
            var response = await HttpClientHelper.DeleteAsync(BaseUrl, string.Format(DeleteSiteUrl,id), token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ActionResponse>(response.Content.ReadAsStringAsync().Result).success;
            }
            return false;
        }
    }
}
