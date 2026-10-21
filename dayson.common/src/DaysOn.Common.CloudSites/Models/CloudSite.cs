using System;

namespace DaysOn.Common.CloudSites.Models
{
    public class CloudSite
    {
        public DateTime dateCreated { get; set; }
        public string id { get; set; }
        public int intstances { get; set; }
        public string name { get; set; }
        public string plan { get; set; }
        public string status { get; set; }
    }
}