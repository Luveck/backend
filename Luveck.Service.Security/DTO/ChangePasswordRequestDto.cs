using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Security.DTO
{
    public class ChangePasswordRequestDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string mail { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string password { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "La contraseña debe tener minimo 8 caracteres")]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "Confirmar la nueva contraseña es obligatorio")]
        [Compare("newPassword", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string confirmPassword { get; set; }
    }
}
