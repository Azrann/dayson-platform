using System;
using System.Collections.Generic;

namespace DaysOn.CompanyBc.Models.Commands
{
    public class SearchUsersRequest
    {
        // filters
        public string Text { get; set; }
        public List<Guid> TeamIds { get; set; }
    }
}