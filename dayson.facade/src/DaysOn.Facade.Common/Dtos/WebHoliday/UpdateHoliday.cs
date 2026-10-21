using System;
using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.Holiday;
using DaysOn.Facade.Models.WebModels.Shared;

namespace DaysOn.Facade.Common.Dtos.WebHoliday
{
    public class UpdateHolidayRequest
    {
        public Guid HolidayId { get; set; }

        public string Notes { get; set; }
        public Date From { get; set; }
        public Date To { get; set; }
    }

    public class UpdateHolidayResponse
    {
        public HolidayOverview Holiday { get; set; }
    }

    public class UpdateHolidayOffline
    {
        public static async Task<UpdateHolidayResponse> UpdateHoliday(UpdateHolidayRequest request)
        {
            var holiday = OfflineDb.Holidays.First(h => h.Id == request.HolidayId);

            holiday.To = request.To;
            holiday.From = request.From;
            holiday.Notes = request.Notes;
            holiday.NumberOfDays++;

            return await Task.FromResult(
                new UpdateHolidayResponse
                {
                    Holiday = holiday
                }
            );
        }
    }
}
