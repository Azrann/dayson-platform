using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaysOn.Common.CloudSites.Models;

namespace DaysOn.Common.CloudSites
{
    public class DaysOnCloudService
    {
        public static Task RestartAllSites()
        {
            var sites = CloudService.GetSites().Result;
            var sitesIds = sites
                .Where(x => x.name.Contains("dayson-") && !x.name.Contains("log") && !x.name.Contains("tower"))
                .Select(x => x.id);
            
            List<Task> tasks = new List<Task>();

            foreach (var siteId in sitesIds)
            {
                tasks.Add(CloudService.Restart(siteId));
            }
            return Task.WhenAll(tasks);
        }

        public static Task ScaleDownSitesForNight()
        {
            var sites = CloudService.GetSites().Result;
            var sitesIds = sites.
                Where(x => x.name == "dayson-auth" || x.name == "dayson-companybc").
                Select(y => y.id).
                ToList();

            List<Task> tasks = new List<Task>();

            foreach (var siteId in sitesIds)
            {
                tasks.Add(CloudService.ScaleTo(siteId,CloudSitePlan.free));
            }
            return Task.WhenAll(tasks);
        }

        public static Task ScaleUpSitesForTest()
        {
            var sites = CloudService.GetSites().Result;
            var sitesIds = sites.
                Where(x => x.name == "dayson-auth" || x.name == "dayson-companybc").
                Select(y => y.id).
                ToList();

            List<Task> tasks = new List<Task>();

            foreach (var siteId in sitesIds)
            {
                tasks.Add(CloudService.ScaleTo(siteId, CloudSitePlan.standard));
            }
            return Task.WhenAll(tasks);
        }
    }
}
