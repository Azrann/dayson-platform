using System;
using System.Collections.Generic;
using System.Linq;
using DaysOn.Common.DataAccess;

namespace DaysOn.HolidaysBc.BoundedContext.Entities
{
    public class OpenClosedDates : TableEntity
    {
        public OpenClosedDates() : base()
        {
            ClosedDays = new List<DateTime>();
            ClosedHalfDays = new List<DateTime>();
        }

        public Guid CompanyId { get; set; }

        public List<DateTime> ClosedDays { get; set; }

        public List<DateTime> ClosedHalfDays { get; set; }

        public static OpenClosedDates operator +(OpenClosedDates oc1, OpenClosedDates oc2)
        {
            oc1.ClosedDays.AddRange(oc2.ClosedDays);
            oc1.ClosedHalfDays.AddRange(oc2.ClosedHalfDays);
            return oc1;
        }

        public void SetYear(int year)
        {
            List<DateTime> result = new List<DateTime>();
            foreach (var day in ClosedDays.Where(x => x.Year != year))
            {
                result.Add(new DateTime(year, day.Month, day.Day, day.Hour, day.Minute, day.Second));
            }
            ClosedDays.AddRange(result);
            result.Clear();
            foreach (var day in ClosedHalfDays.Where(x => x.Year != year))
            {
                result.Add(new DateTime(year, day.Month, day.Day, day.Hour, day.Minute, day.Second));
            }
            ClosedHalfDays.AddRange(result);
            ClosedDays.RemoveAll(x => x.Year != year);
            ClosedHalfDays.RemoveAll(x => x.Year != year);
        }
    }
}