using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Administration.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string StateCode { get; set;}
        [ForeignKey("countryId")]
        public Country Country { get; set; }
    }
}
