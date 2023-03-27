using System;
using System.Collections.Generic;

namespace Luveck.Service.Administration.DTO
{
    public class ProductRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string presentation { get; set; }
        public string Quantity { get; set; }
        public string TypeSell { get; set; }
        public double Cost { get; set; }
        public bool state { get; set; }
        public int IdCategory { get; set; }
        public string urlOficial { get; set; }
        public List<FileRequestDto> File { get; set; }
    }
}
