using System;

namespace Luveck.Service.Administration.Models.Dto
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool state { get; set; }
        public int departymentId { get; set; }
        public string departmentName { get; set; }
        public int countryId { get; set; }
        public string countryName { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }        
    }
}
