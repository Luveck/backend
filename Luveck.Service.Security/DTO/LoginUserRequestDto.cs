using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Security.DTO
{
    public class LoginUserRequestDto
    {
            [Required(ErrorMessage = "El DNI es obligatorio.")]
            public string DNI { get; set; }

            [Required(ErrorMessage = "El pasword es obligatorio")]
            public string Password { get; set; }
    }
}
