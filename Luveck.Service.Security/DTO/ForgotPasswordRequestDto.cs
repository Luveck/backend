using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Security.DTO
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
