using System;
using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.Holiday;

namespace DaysOn.Facade.Common.Dtos.WebHoliday
{
    public class ChangeHolidayStatusRequest
    {
        public Guid HolidayId { get; set; }

        public HolidayStatus Status { get; set; }
    }

    public class ChangeHolidayStatusResponse
    {
        public HolidayOverview Holiday { get; set; }
    }

    public class ChangeHolidayStatusOffline
    {
        public static async Task<ChangeHolidayStatusResponse> ChangeHolidayStatus(ChangeHolidayStatusRequest request)
        {
            var holiday = OfflineDb.Holidays.First(h => h.Id == request.HolidayId);

            holiday.Status = request.Status;

            return await Task.FromResult(
                new ChangeHolidayStatusResponse
                {
                    Holiday = holiday
                }
            );
        }
    }
}
