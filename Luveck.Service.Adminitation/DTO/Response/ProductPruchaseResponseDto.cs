using System;

namespace Luveck.Service.Administration.DTO.Response
{
    public class ProductPruchaseResponseDto
    {
        public string ProductName { get; set; }
        public int ProductId{ get; set; }
        public int QuantityShiped { get; set; }
        public DateTime DateShiped { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
