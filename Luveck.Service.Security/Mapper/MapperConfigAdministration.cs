using AutoMapper;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Models.Dtos;

namespace Luveck.Service.Security.Mapper
{
    public class MapperConfigAdministration : Profile
    {
        public MapperConfigAdministration()
        {
            CreateMap<RoleModule, ListModuleRoleDto>().ReverseMap();
        }
    }
}
