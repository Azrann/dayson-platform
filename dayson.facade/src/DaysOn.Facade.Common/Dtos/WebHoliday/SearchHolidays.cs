using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;
using DaysOn.Facade.Models.WebModels.Holiday;
using DaysOn.Facade.Models.WebModels.Shared;

namespace DaysOn.Facade.Common.Dtos.WebHoliday
{
    public class SearchHolidaysRequest
    {
        public List<Guid> TeamIds { get; set; }
        public List<Guid> UserIds { get; set; }
        public Date From { get; set; }
        public Date To { get; set; }
        public HolidayStatus? Status { get; set; }
    }
    
    public class SearchHolidaysResponse
    {
        public List<HolidayDetails> Holidays { get; set; }
    }

    public class SearchHolidaysOffline
    {
        public static async Task<SearchHolidaysResponse> SearchHolidays(SearchHolidaysRequest request)
        {
            var holidays = OfflineDb.Holidays.Where(h => (
                h.CompanyId == OfflineDb.HardcodedCompanyId &&
                (request.Status.HasValue ? h.Status == request.Status : true) &&
                (request.TeamIds != null && request.TeamIds.Count > 0 ? request.TeamIds.Contains(OfflineDb.Users.First(u => u.Id == h.UserId).TeamId) : true) &&
                (request.UserIds != null && request.UserIds.Count > 0 ? request.UserIds.Contains(h.UserId) : true) &&
                (request.To != null ? h.From.BeforeThan(request.To) : true) &&
                (request.From != null ? h.To.AfterThan(request.From) : true)
            )).Select(h => new HolidayDetails
            {
                Id = h.Id,
                CompanyId = h.CompanyId,
                To = h.To,
                From = h.From,
                UserId = h.UserId,
                Status = h.Status,
                Notes = h.Notes,
                NumberOfDays = h.NumberOfDays,
               // Team = OfflineDb.Teams.First(t => t.Id == OfflineDb.Users.First(u => u.Id == h.UserId).TeamId),
                User = OfflineDb.Users.First(u => u.Id == h.UserId)
            }).ToList();

            // pedaso de search query en mierda de linq eh!?

            return await Task.FromResult(
                new SearchHolidaysResponse
                {
                    Holidays = holidays
                }
            );
        }
    }
}
