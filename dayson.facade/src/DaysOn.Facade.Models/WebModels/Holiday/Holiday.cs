using System;
using DaysOn.Facade.Models.WebModels.Shared;

namespace DaysOn.Facade.Models.WebModels.Holiday
{
    public class HolidayOverview
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }

        public string Notes { get; set; }

        public Date From { get; set; }
        public bool FromHalfDay { get; set; }

        public Date To { get; set; }
        public bool ToHalfDay { get; set; }

        public decimal NumberOfDays { get; set; }

        public HolidayStatus Status { get; set; }
    }
}
