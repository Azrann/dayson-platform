using AutoMapper;
using DaysOn.Common.Events.FunctionsEvents;
using DaysOn.HolidaysBc.BoundedContext.Entities;
using DaysOn.HolidaysBc.Models.Commands;
using DaysOn.HolidaysBc.Models.Dtos;

namespace DaysOn.HolidaysBc.Api
{
    public static class AutoMapperBase
    {
        private static IMapper _Mapper;

        public static IMapper Mapper => _Mapper ?? (_Mapper = GetAutoMapperBase());

        public static IMapper GetAutoMapperBase()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddHolidayCommand, HolidayRequest>();

                cfg.CreateMap<AddHolidayRequestCommand, AddHolidayRequestEvent>();
                cfg.CreateMap<AddHolidayCommand, AddHolidayRequestEvent>();

                cfg.CreateMap<Employee, EmployeeDto>();
                cfg.CreateMap<EmployeeDto, Employee>().AfterMap((src, dest) =>
                {
                    dest.InitializeWorkinDaysOfTheWeek(src.WorkingDaysOfTheWeek);
                });
                cfg.CreateMap<Employee, IdNameDto>();
                cfg.CreateMap<Team, IdNameDto>();
                cfg.CreateMap<Team, TeamDto>();
                cfg.CreateMap<Team, TeamPermisionsDto>();
                cfg.CreateMap<Company, CompanyDto>();
                cfg.CreateMap<Company, IdNameDto>();
                cfg.CreateMap<GlobalRules, GlobalRulesDto>();
                cfg.CreateMap<GlobalRulesDto, GlobalRules>().AfterMap((src, dest) =>
                {
                    dest.InitializeWorkinDaysOfTheWeek(src.WorkingDaysOfTheWeek);
                });
                cfg.CreateMap<OpenClosedDates, OpenClosedDatesDto>();
                cfg.CreateMap<AddDatesCommand, OpenClosedDates>();
                cfg.CreateMap<AddRecurrentDatesCommand, OpenClosedDates>();
                cfg.CreateMap<HolidayRequest, HolidayRequestDto>();
                cfg.CreateMap<YearlyHolidayCalculation, YearlyHolidayCalculationDto>();
            });

            return config.CreateMapper();
        }
    }
}
