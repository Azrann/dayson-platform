using System.Collections.Generic;
using DaysOn.Common.CloudSites.Models;

namespace DaysOn.Common.CloudSites.Responses
{
    public class GetSitesResponse
    {
        public List<CloudSite> cloudSites { get; set; }
    }
}