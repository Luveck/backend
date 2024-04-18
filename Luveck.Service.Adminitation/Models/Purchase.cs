using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Administration.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("pharmacyId")]
        public int pharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
        [ForeignKey("userId")]
        public string userId { get; set; }
        public string NoPurchase { get; set; }
        public string CreateBy { get; set; }
        public bool purchaseReviewed { get; set; }
        public DateTime DateShiped { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
