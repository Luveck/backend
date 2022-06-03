using System;
using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Administration.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string Iso { get; set; }
        public string Name { get; set; }
        public string Iso3 { get; set; }
        public string PhoneCode { get; set; }
        public string Currency { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public bool Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
