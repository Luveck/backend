using System;

namespace Luveck.Service.Administration.Models.Dto
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StateId { get; set; }
        public string StateCode { get; set; }
        public string stateName { get; set; }
        public string CreateBy { get; set; }
        public string countryId { get; set; }
        public string countryName { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }        
    }
}
