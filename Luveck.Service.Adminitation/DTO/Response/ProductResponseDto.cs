using System;

namespace Luveck.Service.Administration.DTO.Response
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string presentation { get; set; }
        public string Quantity { get; set; }
        public string TypeSell { get; set; }
        public double Cost { get; set; }
        public string CreateBy { get; set; }
        public bool state { get; set; }
        public string urlOficial { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int IdCategory { get; set; }
        public string NameCategory { get; set; }
    }
}
