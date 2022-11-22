using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Administration.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("pharmacyId")]
        public Pharmacy Pharmacy { get; set; }
        [ForeignKey("userId")]
        public User user { get; set; }
        public string NoPurchase { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
