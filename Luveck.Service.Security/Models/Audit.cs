using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Security.Models
{
    public class Audit
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("userId")]
        public User user { get; set; }
        public string userId { get; set; }
        public string IP { get; set; }
        public DateTime LoginDate { get; set; }
        public string Device { get; set; }
    }
}
