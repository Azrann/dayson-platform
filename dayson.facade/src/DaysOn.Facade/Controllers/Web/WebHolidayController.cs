using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using DaysOn.Common.Events.Dtos;
using DaysOn.Common.Utilities.Extension;
using DaysOn.Facade.Common;
using DaysOn.Facade.Common.Dtos.WebHoliday;
using DaysOn.Facade.Models.WebModels.Holiday;
using DaysOn.Facade.Models.WebModels.Shared;
using DaysOn.HolidaysBc.Models.Commands;
using DaysOn.HolidaysBc.Models.Dtos;
using log4net;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using SearchHolidaysRequest = DaysOn.Facade.Common.Dtos.WebHoliday.SearchHolidaysRequest;

namespace DaysOn.Facade.Controllers.Web
{
    [RoutePrefix("v1")]
    public class WebHolidayController : ApiControllerBase
    {
        private readonly ILog _logger;

        public WebHolidayController()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        [AllowAnonymous]
        [Route("WebHoliday/Ping")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> Ping()
        {
            _logger.LogDeb("WebHoliday.Ping invoked");

            return await Task.FromResult<IHttpActionResult>(Ok("WebHoliday service is up and running!"));
        }

        [AllowAnonymous]
        [Route("WebHoliday/GetUserHolidays")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GetUserHolidaysResponse))]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserHolidays([FromUri] GetUserHolidaysRequest request)
        {
#if OFFLINE
            return Ok(await GetUserHolidaysOffline.GetUserHolidays(request));
#else
            _logger.LogInf("GetHolidayAsync method called");
            GetUserHolidaysResponse result = new GetUserHolidaysResponse()
            {
                Holidays = new List<HolidayOverview>()
            };
            var response = await HolidayClientWrapper.GetHolidaysAsync(request.UserId, Request.Headers.Authorization.Parameter);
            var responseString = await response.Content.ReadAsStringAsync();
            foreach (var holidayRequestDto in JsonConvert.DeserializeObject<List<HolidayRequestDto>>(responseString))
            {
                result.Holidays.Add(MapHolidayRequestDto(holidayRequestDto));
            }

            return Ok(result);
#endif
        }

        [AllowAnonymous]
        [Route("WebHoliday/CreateHoliday")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CreateHolidayResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> CreateHoliday([FromBody] CreateHolidayRequest request)
        {
#if OFFLINE
            return Ok(await CreateHolidayOffline.CreateHoliday(request));
#else
            _logger.LogInf("CreateHoliday method called");
            GetUserHolidaysResponse result = new GetUserHolidaysResponse()
            {
                Holidays = new List<HolidayOverview>()
            };
            DateTime From = request.From.ToDateTime();
            if (request.FromHalfDay)
            {
                From=From.AddHours(12);
            }

            DateTime To = request.To.ToDateTime();
            if (request.ToHalfDay)
            {
                To=To.AddHours(12);
            }

            var response = await HolidayClientWrapper.CreateHolidayAsync(new AddHolidayRequestCommand()
            {
                CorrelationId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Status = Status.Requested,
                Notes = request.Notes,
                From = From,
                To = To,
                SubscriberId = request.UserId
            }, Request.Headers.Authorization.Parameter);
            
            return Ok(response);
#endif
        }

        [AllowAnonymous]
        [Route("WebHoliday/UpdateHoliday")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UpdateHolidayResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateHoliday([FromBody] UpdateHolidayRequest request)
        {
#if OFFLINE
            return Ok(await UpdateHolidayOffline.UpdateHoliday(request));
#else
            throw new NotImplementedException();
#endif
        }

        [AllowAnonymous]
        [Route("WebHoliday/ChangeHolidayStatus")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ChangeHolidayStatusResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> ChangeHolidayStatus([FromBody] Common.Dtos.WebHoliday.ChangeHolidayStatusRequest request)
        {
#if OFFLINE     
            return Ok(await ChangeHolidayStatusOffline.ChangeHolidayStatus(request));
#else
            var response = await HolidayClientWrapper.ChangeHolidayStatusAsync(request, Request.Headers.Authorization.Parameter);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.GetHolidayRequest());
            }

            return InternalServerError();
#endif
        }

        [AllowAnonymous]
        [Route("WebHoliday/SearchHolidays")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SearchHolidaysResponse))]
        [HttpPost]
        public async Task<IHttpActionResult> SearchHolidays([FromBody] SearchHolidaysRequest request)
        {
#if OFFLINE     
            return Ok(await SearchHolidaysOffline.SearchHolidays(request));
#else
            var response = await HolidayClientWrapper.SearchHolidays(request, Request.Headers.Authorization.Parameter);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<HolidayRequestOverviewDto>>(responseString);
                /* Aqui devuelvo un holiday request dto + nombre del employee dime si con esto es suficiente.
                 La DTO retocala a tu gusto*/
                return Ok(result);
            }

            return InternalServerError();
#endif
        }

        private HolidayOverview MapHolidayRequestDto(HolidayRequestDto holidayRequestDto)
        {
            return new HolidayOverview
            {
                UserId = holidayRequestDto.EmployeeId,
                CompanyId = Guid.Empty, //TODO do you really need this?
                Id = holidayRequestDto.Id,
                From = GetDate(holidayRequestDto.From),
                FromHalfDay = holidayRequestDto.From.Hour == 12,
                To = GetDate(holidayRequestDto.To),
                ToHalfDay = holidayRequestDto.To.Hour == 12,
                Notes = holidayRequestDto.Notes,
                NumberOfDays = holidayRequestDto.NumberOfDays,
                Status = GetStatus(holidayRequestDto.Status)
            };
        }

        private HolidayStatus GetStatus(Status status)
        {
            switch (status)
            {
                case Status.Aproved: return HolidayStatus.Accepted;
                case Status.Canceled: return HolidayStatus.Canceled;
                case Status.Rejected: return HolidayStatus.Rejected;
                case Status.Requested: return HolidayStatus.Requested;
                default: return HolidayStatus.Requested;
            }
        }

        private Models.WebModels.Shared.Date GetDate(DateTime datetime)
        {
            return new Models.WebModels.Shared.Date(datetime.Year, datetime.Month, datetime.Day);
        }

    }
}
