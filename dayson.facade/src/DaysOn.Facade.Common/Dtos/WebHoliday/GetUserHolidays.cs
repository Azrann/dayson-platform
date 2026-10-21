using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.Holiday;

namespace DaysOn.Facade.Common.Dtos.WebHoliday
{
    public class GetUserHolidaysRequest
    {
        public Guid UserId { get; set; }
    }

    public class GetUserHolidaysResponse
    {
        public List<HolidayOverview> Holidays { get; set; }
    }

    public class GetUserHolidaysOffline
    {
        public static async Task<GetUserHolidaysResponse> GetUserHolidays(GetUserHolidaysRequest request)
        {
            List<HolidayOverview> holidays = OfflineDb.Holidays.Where(h => h.UserId == request.UserId).ToList();

            return await Task.FromResult(
                new GetUserHolidaysResponse
                {
                    Holidays = holidays
                }
            );
        }
    }
}
