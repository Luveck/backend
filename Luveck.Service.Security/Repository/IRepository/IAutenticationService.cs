using Luveck.Service.Security.DTO;
using Luveck.Service.Security.DTO.Response;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Repository.IRepository
{
    public interface IAutenticationService
    {
        Task<LoginResponseDto> Login(string user, string password);
        Task<LoginResponseDto> Register(RegisterUserRequestDto registerUserRequestDto);
        Task<RegisterUserResponseDto> Createuser(RegisterUserRequestDto user, string role);
        Task<RegisterUserResponseDto> changePassword(ChangePasswordRequestDto changePasswordRequestDto);
        Task<RegisterUserResponseDto> forgotPassword(string Email);
        Task<LoginResponseDto> resetPassword(ResetPasswordRequestDto resetPasswordRequestDto);
        Task<UserResponseDto> getUser(string Email);
        Task<RegisterUserResponseDto> updateUser(RegisterUserRequestDto user, string role, string roleRequestor);
    }
}
