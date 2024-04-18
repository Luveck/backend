using Luveck.Service.Administration.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Luveck.Service.Administration.DTO.Response
{
    public class ProductRuleChangeResponseDto
    {
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
        public string productName { get; set; }
        public int productId { get; set; }
        public string Barcode { get; set; }
    }
}
