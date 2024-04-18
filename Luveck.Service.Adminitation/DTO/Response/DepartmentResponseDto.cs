using System;

namespace Luveck.Service.Administration.DTO.Response
{
    public class DepartmentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int countryId { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public bool status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
