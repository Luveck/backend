using System;

namespace Luveck.Service.Administration.DTO
{
    public class ProductChangeRuleRequestDto
    {
        public int Id { get; set; }
        public int DaysAround { get; set; }
        public int Periodicity { get; set; }
        public int QuantityBuy { get; set; }
        public int QuantityGive { get; set; }
        public int MaxChangeYear { get; set; }
        public int productId { get; set; }
        public bool state { get; set; }
    }
}
