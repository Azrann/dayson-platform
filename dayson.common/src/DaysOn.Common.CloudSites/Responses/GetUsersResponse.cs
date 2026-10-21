using System.Collections.Generic;
using DaysOn.Common.CloudSites.Models;

namespace DaysOn.Common.CloudSites.Responses
{
    public class GetUsersResponse
    {
        public List<DbUser> Users { get; set; }
    }
}