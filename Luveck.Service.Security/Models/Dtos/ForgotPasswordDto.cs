using System.ComponentModel.DataAnnotations;

namespace Luveck.Service.Security.Models.Dtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
