using AutoMapper;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Luveck.Service.Security.Mapper
{
    public class MapperConfigAdministration : Profile
    {
        public MapperConfigAdministration()
        {
            CreateMap<RoleModule, ListModuleRoleDto>().ReverseMap();
            CreateMap<userDto, IdentityUser>().ReverseMap();
            CreateMap<Module, ModuleDto>().ReverseMap();
        }
    }
}
