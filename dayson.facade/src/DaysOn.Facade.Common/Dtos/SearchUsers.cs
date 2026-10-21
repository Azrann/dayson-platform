using System;
using System.Collections.Generic;
using DaysOn.Facade.Models.WebModels.User;

namespace DaysOn.Facade.Common.Dtos
{
    public class SearchUsersRequest
    {
        // filters
        public string Text { get; set; }
        public List<Guid> TeamIds { get; set; }
    }

    public class SearchUsersResponse
    {
        public List<UserOverview> Users { get; set; }
    }
}
