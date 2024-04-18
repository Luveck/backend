using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Administration.Models
{
    public class ProductChangeRule
    {
        [Key]
        public int Id { get; set; }
        public int DaysAround { get; set; }
        public int Periodicity { get; set; }
        public int QuantityBuy { get; set; }
        public int QuantityGive { get; set; }
        public int MaxChangeYear { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public bool state { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("productId")]
        public int productId { get; set; }
        public Product product{ get; set; }
    }
}
