using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Administration.Models
{
    public class ProductPurchase
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("purchaseId")]
        public int purchaseId { get; set; }
        public Purchase Purchase { get; set; }
        [ForeignKey("productId")]
        public int productId { get; set; }
        public Product Product { get; set; }
        public int QuantityShiped { get; set; }
        public DateTime DateShiped { get; set; }
        public bool Exchanged { get; set; }
        public bool Losed { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
