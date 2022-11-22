using System;

namespace Luveck.Service.Administration.Models.Dto
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public int PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string NameUser { get; set; }
        public string userId { get; set; }
        public string NoPurchase { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
