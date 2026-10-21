using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysOn.Common.CloudSites.Models
{
    public class DbUser
    {
        public DateTime dateCreated { get; set; }
        public string id { get; set; }
        public string password { get; set; }
        public string permission { get; set; }
        public string userName { get; set; }
    }
}
