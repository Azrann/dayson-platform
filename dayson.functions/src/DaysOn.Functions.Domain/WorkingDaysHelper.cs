using System;
using System.Collections.Generic;
using System.Linq;

namespace DaysOn.Functions.BoundedContext
{
    public class WorkingDaysHelper
    {
        public static decimal GetWorkingDays(DateTime from, DateTime to, Dictionary<DayOfWeek, bool> workingDaysOfTheWeek)
        {
            var totalDays = (to - from).TotalDays;
            if (!IsHalfDay(to))
            {
                totalDays++;
            }
            
            var numWorkingDaysInAWeek = workingDaysOfTheWeek.Count(x => x.Value);
            double result = 0;
            var odd = totalDays % 7;

            for (int i = 0; i < odd; i++)
            {
                if (IsWorkingDay(from.AddDays(i), workingDaysOfTheWeek))
                {
                    result++;
                }
            }

            result += Math.Floor(totalDays / 7) * numWorkingDaysInAWeek;

            if (IsHalfDay(to) && IsWorkingDay(to, workingDaysOfTheWeek))
            {
                result = result - 0.5;
            }

            if (IsHalfDay(from) && IsWorkingDay(from, workingDaysOfTheWeek))
            {
                result = result - 0.5;
            }

            return Convert.ToDecimal(result);
        }

        private static bool IsWorkingDay(DateTime dateTime, Dictionary<DayOfWeek, bool> workingDaysOfTheWeek)
        {
            var day = dateTime.DayOfWeek;
            return workingDaysOfTheWeek[day];
        }

        private static bool IsHalfDay(DateTime to)
        {
            return to.Hour == 12;
        }

        public static List<DateTime> FilterWorkingDaysOfTheWeek(Dictionary<DayOfWeek, bool> workingDaysOfTheWeek, List<DateTime> nonWorkingDays)
        {
            return nonWorkingDays.Where(x => workingDaysOfTheWeek[x.DayOfWeek]).ToList();
        }

        public static List<DateTime> GetTotalBankHolidaysBetweenDates(List<DateTime> nonWorkingDays, DateTime from, DateTime to)
        {
            return nonWorkingDays.Where(x => from <= x && to >= x).ToList();
        }
    }
}
