using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Administration.Models
{
    public class ExchangedProduct
    {
        [Key]
        public string Id { get; set; }
        public int QuantityExchanged { get; set; }
        public string userExchanged { get; set; }
        [ForeignKey("productId")]
        public int productId { get; set; }       
        public Product Product { get; set; }
        public string ExchangeBy { get; set; }
        public DateTime ExchangeDate { get; set; }
    }
}
