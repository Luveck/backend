using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Administration.Models
{
    public class Medical
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string register { get; set; }
        public string CreateBy { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("patologyId")]
        public int patologyId { get; set; }
        public Patology patology { get; set; }
    }
}
