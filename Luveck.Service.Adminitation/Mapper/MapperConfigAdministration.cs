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
            CreateMap<Department, DepartmentsDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<SBU, SBUDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Patology, PatologyDto>().ReverseMap();
            CreateMap<Pharmacy, PharmacyDto>().ReverseMap();
            CreateMap<Medical, MedicalDto>().ReverseMap();
            CreateMap<Purchase, PurchaseDto>().ReverseMap();
        }
    }
}
