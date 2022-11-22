using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Security.DTO
{
    public class RegisterUserRequestDto
    {
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "El pasword es obligatorio")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Phone { get; set; }
    }
}
