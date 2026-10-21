using System;

namespace DaysOn.Facade.Models.WebModels.Shared
{
    public class Date
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public Date()
        {
        }

        public Date(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public bool BeforeThan(Date otherDate)
        {
            return new DateTime(Year, Month, Day) < new DateTime(otherDate.Year, otherDate.Month, otherDate.Day);
        }

        public bool AfterThan(Date otherDate)
        {
            return new DateTime(Year, Month, Day) > new DateTime(otherDate.Year, otherDate.Month, otherDate.Day);
        }

        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day);
        }
    }
}
