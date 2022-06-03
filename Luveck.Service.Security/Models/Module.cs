using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Security.Models
{
    public class Module
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string name { get; set; }
    }
}
