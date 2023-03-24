using System;

namespace Luveck.Service.Administration.DTO
{
    public class MedicalRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string register { get; set; }
        public bool isDeleted { get; set; }
        public int patologyId { get; set; }
    }
}
