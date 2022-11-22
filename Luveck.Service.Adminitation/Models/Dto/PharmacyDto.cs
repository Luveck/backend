using System;

namespace Luveck.Service.Administration.Models.Dto
{
    public class PharmacyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
        public int cityId { get; set; }
        public string cityName { get; set; }
    }
}
