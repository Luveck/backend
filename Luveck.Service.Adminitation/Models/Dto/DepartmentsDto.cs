namespace Luveck.Service.Administration.Models.Dto
{
    public class DepartmentsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StateCode { get; set; }
        public int countryId { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
    }
}
