using Luveck.Service.Administration.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Luveck.Service.Administration.DTO
{
    public class DepartmentCreateUpdateRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool status { get; set; }
        public int idCountry { get; set; }
    }
}
