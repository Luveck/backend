using System;

namespace Luveck.Service.Administration.DTO.Response
{
    public class PurshaseResponseDto
    {
        public int Id { get; set; }
        public string NamePharmacy { get; set; }
        public string CityPharmacy { get; set; }
        public string NoPurchase { get; set; }
        public string Buyer { get; set; }
        public int IdPharmacy { get; set; }
        public int IdCityPharmacy { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Reviewed { get; set; }
        public DateTime DateShiped { get; set; }
    }
}
