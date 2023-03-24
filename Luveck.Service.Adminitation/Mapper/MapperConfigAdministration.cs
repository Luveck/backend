using AutoMapper;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;

namespace Luveck.Service.Administration.Mapper
{
    public class MapperConfigAdministration : Profile
    {
        public MapperConfigAdministration()
        {
            CreateMap<SBU, SBUDto>().ReverseMap();
        }
    }
}
