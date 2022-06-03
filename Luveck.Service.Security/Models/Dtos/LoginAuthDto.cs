using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Security.Models.Dtos
{
    public class LoginAuthDto
    {
        public string DNI { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "El pasword es obligatorio")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
