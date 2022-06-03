using AutoMapper;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;

namespace Luveck.Service.Administration.Mapper
{
    public class MapperConfigAdministration : Profile
    {
        public MapperConfigAdministration()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CountryCreateUpdateDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<SBU, SBUDto>().ReverseMap();
        }
    }
}
