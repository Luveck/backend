using System;
using System.Collections.Generic;

namespace Luveck.Service.Administration.DTO
{
    public class ProductPurchaseRequestDto
    {
        public int purchaseId { get; set; }
        public List<LstProductPurchase> ProductPurchase { get; set; }
    }

    public class LstProductPurchase
    {
        public int productId { get; set; }
        public int QuantityShiped { get; set; }
        public DateTime DateShiped { get; set; }
    }
}
