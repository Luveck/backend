using Luveck.Service.Security.Models;

namespace Luveck.Service.Security.DTO.Response
{
    public class UserResponseDto
    {
        public User userEntity { get; set; }
        public string Role { get; set; }
    }
}
