using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Security.Models.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El pasword es obligatorio")]
        public string Password { get; set; }
        [Required(ErrorMessage = "El pasword es obligatorio")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}
