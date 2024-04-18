using System;
using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Administration.Models
{
    public class MassiveRemainder
    {
        [Key]
        public int Id { get; set; }
        public string date { get; set; }
        public bool sent { get; set; }
    }
}
