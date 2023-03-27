using System;
using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Administration.Models
{
    public class ImageProduct
    {
        [Key]
        public Int64 Id { get; set; }
        public string fileName { get; set; }
        public int productId { get; set; }
        public Product product { get; set; }
    }
}
