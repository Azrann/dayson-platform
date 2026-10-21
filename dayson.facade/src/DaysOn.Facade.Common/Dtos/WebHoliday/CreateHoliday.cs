using System;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.Holiday;
using DaysOn.Facade.Models.WebModels.Shared;

namespace DaysOn.Facade.Common.Dtos.WebHoliday
{
    public class CreateHolidayRequest
    {
        public Guid UserId { get; set; }

        public string Notes { get; set; }
        public Date From { get; set; }
        public bool FromHalfDay { get; set; }

        public Date To { get; set; }
        public bool ToHalfDay { get; set; }
    }

    public class CreateHolidayResponse
    {
    }

    public class CreateHolidayOffline
    {
        public static async Task<CreateHolidayResponse> CreateHoliday(CreateHolidayRequest request)
        {
            var holiday = new HolidayOverview
            {
                From = request.From,
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                To = request.To,
                Notes = request.Notes,
                NumberOfDays = 1,
                Status = HolidayStatus.Requested,
                CompanyId = OfflineDb.HardcodedCompanyId
            };

            OfflineDb.Holidays.Add(holiday);

            return await Task.FromResult(
                new CreateHolidayResponse
                {
                }
            );
        }
    }

}
