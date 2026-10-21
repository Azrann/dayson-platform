using System;

namespace DaysOn.Common.CloudSites.Models
{
    public class Database
    {
        public DateTime dateCreated { get; set; }
        public string id { get; set; }
        public bool locked { get; set; }
        public string name { get; set; }
        public string plan { get; set; }
        public int size { get; set; }
        public string type { get; set; }
    }
}
