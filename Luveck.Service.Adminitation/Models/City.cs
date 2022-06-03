using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Administration.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string StateCode { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("departmentId")]
        public Department department { get; set; }
    }
}
