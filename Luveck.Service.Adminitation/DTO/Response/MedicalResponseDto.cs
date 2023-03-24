using System;

namespace Luveck.Service.Administration.DTO.Response
{
    public class MedicalResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string register { get; set; }
        public string CreateBy { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int patologyId { get; set; }
        public string patologyName { get; set; }
    }
}
