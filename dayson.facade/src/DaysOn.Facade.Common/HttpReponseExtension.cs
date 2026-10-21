using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DaysOn.HolidaysBc.Models.Dtos;
using Newtonsoft.Json;

namespace DaysOn.Facade.Common
{
    public static class HttpReponseExtension
    {
        public static Guid GetGuid(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Guid>(response.Content.ReadAsStringAsync().Result);
            }
            return Guid.Empty;
        }

        public static async Task<Guid> GetGuidAsync(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Guid>(response.Content.ReadAsStringAsync().Result);
            }
            return Guid.Empty;
        }

        public static HolidayRequestDto GetHolidayRequest(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<HolidayRequestDto>(response.Content.ReadAsStringAsync().Result);
            }
            return null;
        }

        public static async Task<HolidayRequestDto> GetHolidayRequestAsync(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<HolidayRequestDto>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }

        public static GlobalRulesDto GetGlobalRuleDto(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<GlobalRulesDto>(response.Content.ReadAsStringAsync().Result);
            }
            return null;
        }

        public static async Task<GlobalRulesDto> GetGlobalRuleDtoAsync(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<GlobalRulesDto>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }

        public static OpenClosedDatesDto GetOpenClosedDatesDto(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<OpenClosedDatesDto>(response.Content.ReadAsStringAsync().Result);
            }
            return null;
        }

        public static List<HolidayRequestDto> GetListHolidayRequest(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<HolidayRequestDto>>(response.Content.ReadAsStringAsync().Result);
            }
            return null;
        }

        public static async Task<List<HolidayRequestDto>> GetListHolidayRequestAsync(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<HolidayRequestDto>>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }

        public static async Task<List<IdNameDto>> GetListTeamApprovers(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<IdNameDto>>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }

        public static YearlyHolidayCalculationDto GetYearlyHolidayCalculationDto(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<YearlyHolidayCalculationDto>(response.Content.ReadAsStringAsync().Result);
            }
            return null;
        }

        public static async Task<List<Guid>> GetListGuid(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Guid>>(stringResult);
            }
            return null;
        }

        public static HolidaysBc.Models.Dtos.EmployeeDto GetEmployeeGlobalRules(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<HolidaysBc.Models.Dtos.EmployeeDto>(response.Content.ReadAsStringAsync().Result);
            }
            return null;
        }
    }
}
