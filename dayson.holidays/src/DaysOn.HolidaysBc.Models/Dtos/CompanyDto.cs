using System;

namespace DaysOn.HolidaysBc.Models.Dtos
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public GlobalRulesDto GlobalRules { get; set; }
        public OpenClosedDatesDto OpenClosedDates { get; set; }
    }
}
