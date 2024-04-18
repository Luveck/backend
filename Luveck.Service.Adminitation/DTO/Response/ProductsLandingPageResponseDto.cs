using System.Collections.Generic;

namespace Luveck.Service.Administration.DTO.Response
{
    public class ProductsLandingPageResponseDto
    {
        public int Id { get; set; }
        public string nameProduct { get; set; }
        public string description { get; set; }
        public List<ProductImgResponseDto> urlImages { get; set; }
        public int IdCategoria { get; set; }
        public string nameCategoria { get; set; }
        public string presentation { get; set; }
        public int maxYear { get; set; }
        public int QuantityBuy { get; set; }
        public int QuantityGive { get; set; }
    }
}
