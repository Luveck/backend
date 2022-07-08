namespace Luveck.Service.Administration.Models.Dto
{
    public class GetCitiesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StateCode { get; set; }
        public int departmentId { get; set; }
        public Department department { get; set; }
    }
}
