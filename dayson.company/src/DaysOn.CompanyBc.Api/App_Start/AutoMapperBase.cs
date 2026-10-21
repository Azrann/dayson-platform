using AutoMapper;
using DaysOn.CompanyBc.BoundedContext.Entities;
using DaysOn.CompanyBc.Models.Dtos;

namespace DaysOn.CompanyBc.Api.App_Start
{
    public static class AutoMapperBase
    {
        private static IMapper _mapper;

        public static IMapper Mapper => _mapper ?? (_mapper = GetAutoMapperBase());

        private static IMapper GetAutoMapperBase()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>();
                cfg.CreateMap<Employee, EmployeeDto>();
                cfg.CreateMap<Employee, ShortEmployeeDto>();
                cfg.CreateMap<Employee, IdNameDto>().ForMember(x => x.Name, src => src.ResolveUsing(y => y.Firstname));
                cfg.CreateMap<Team, IdNameDto>();
                cfg.CreateMap<Team, TeamDto>();
                cfg.CreateMap<Team, RichTeamDto>();
                cfg.CreateMap<Company, CompanyDto>();
                cfg.CreateMap<Company, IdNameDto>();
            });

            return config.CreateMapper();
        }
    }
}