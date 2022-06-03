namespace Luveck.Service.Administration.Models.Dto
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StateCode { get; set; }
        public int countryId { get; set; }
        public Country country { get; set; }
    }
}
