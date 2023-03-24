using System;

namespace Luveck.Service.Administration.DTO.Response
{
    public class ProductPruchaseByUser
    {
        public int IdFactura { get; set; }
        public string CreationDate { get; set; }
        public string NoPurchase { get; set;}
        public string UserId { get; set; }
        public int productId { get; set; }
        public int QuantityShiped { get; set; }
        public DateTime DateShiped { get; set; }
    }
}
