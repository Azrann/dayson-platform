using System.Collections.Generic;
using DaysOn.Common.CloudSites.Models;

namespace DaysOn.Common.CloudSites.Responses
{
    public class GetDatabasesResponse
    {
        public List<Database> Databases { get; set; }
    }
}