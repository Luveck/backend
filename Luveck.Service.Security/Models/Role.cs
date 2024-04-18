using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Security.Models
{
    public class Role : IdentityRole
    {
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public bool state { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
