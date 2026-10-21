using DaysOn.HolidaysBc.Models.Commands;
using System;
using System.Collections.Generic;

namespace DaysOn.Facade.Tests.Acceptance.Helper
{
    public static class Factory
    {
        public static Dictionary<DayOfWeek, bool> GetWorkingDays()
        {
            Dictionary<DayOfWeek, bool> daysOfTheWeek = new Dictionary<DayOfWeek, bool>
            {
                [DayOfWeek.Monday] = true,
                [DayOfWeek.Tuesday] = true,
                [DayOfWeek.Wednesday] = true,
                [DayOfWeek.Thursday] = true,
                [DayOfWeek.Friday] = true,
                [DayOfWeek.Saturday] = false,
                [DayOfWeek.Sunday] = false
            };
            return daysOfTheWeek;
        }


        public static AddRecurrentDatesCommand GetRecurrentDaysCommand()
        {
            return new AddRecurrentDatesCommand
            {
                ClosedDays = new List<DateTime>
                {
                    new DateTime(2000, 11, 2),
                    new DateTime(2000, 11, 1),
                    new DateTime(2000, 10, 3)
                },
                ClosedHalfDays = new List<DateTime>
                {
                    new DateTime(2000, 12, 22),
                    new DateTime(2000, 12, 23),
                    new DateTime(2000, 12, 24),
                    new DateTime(2000, 12, 25)
                },
                CorrelationId = Guid.NewGuid(),
                SubscriberId = Guid.NewGuid()
            };
        }

        public static AddRecurrentDatesCommand GetRecurrentDaysCommandJanuary()
        {
            return new AddRecurrentDatesCommand
            {
                ClosedDays = new List<DateTime>
                {
                    new DateTime(2000, 1, 4),
                    new DateTime(2000, 10, 11),
                    new DateTime(2000, 10, 3)
                },
                ClosedHalfDays = new List<DateTime>
                {
                    new DateTime(2000, 12, 22),
                    new DateTime(2000, 12, 23),
                    new DateTime(2000, 12, 24),
                    new DateTime(2000, 12, 25)
                },
                CorrelationId = Guid.NewGuid(),
                SubscriberId = Guid.NewGuid()
            };
        }

        public static AddDatesCommand GetAddDaysCommand(int year)
        {
            return new AddDatesCommand
            {
                ClosedDays = new List<DateTime>
                {
                    new DateTime(year, 2, 2),
                    new DateTime(year, 1, 1),
                    new DateTime(year, 3, 3)
                },
                ClosedHalfDays = new List<DateTime>
                {
                    new DateTime(year, 5, 2),
                    new DateTime(year, 6, 1),
                    new DateTime(year, 7, 3),
                    new DateTime(year, 9, 3)
                },
                Year = year,
                CorrelationId = Guid.NewGuid(),
                SubscriberId = Guid.NewGuid()
            };
        }

        public static AddDatesCommand GetAddDaysCommandJanuary(int year)
        {
            return new AddDatesCommand
            {
                ClosedDays = new List<DateTime>
                {
                    new DateTime(year, 1, 1),
                    new DateTime(year, 1, 4),
                    new DateTime(year, 1, 8)
                },
                ClosedHalfDays = new List<DateTime>
                {
                    new DateTime(year, 1, 10),
                    new DateTime(year, 5, 2),
                    new DateTime(year, 7, 3),
                    new DateTime(year, 9, 3)
                },
                Year = year,
                CorrelationId = Guid.NewGuid(),
                SubscriberId = Guid.NewGuid()
            };
        }

        public static AddDateCommand GetAddDayCommand(int year)
        {
            return new AddDateCommand
            {
                Date = new DateTime(year, 7, 7)
            };
        }

        public static AddDateCommand GetAddDayCommandByDay(int month, int day)
        {
            return new AddDateCommand
            {
                Date = new DateTime(2010, month, day)
            };
        }
    }
}
