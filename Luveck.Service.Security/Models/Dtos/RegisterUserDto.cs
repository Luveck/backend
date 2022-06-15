using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Security.Models.Dtos
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El DNI es obligatorio")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "El pasword es obligatorio")]
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}
