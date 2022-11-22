using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Administration.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string presentation { get; set; }
        public string Quantity { get; set; }
        public string TypeSell { get; set; }
        public double Cost{ get; set; }
        public string descuento { get; set; }
        public string descuentoLab { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public bool state { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("categoryId")]
        public Category category { get; set; }
    }
}
