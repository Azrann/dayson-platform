using System;

namespace DaysOn.Facade.Models.WebModels.Company
{
    public class CompanyOverview
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }

        public string Name { get; set; }
    }
}
