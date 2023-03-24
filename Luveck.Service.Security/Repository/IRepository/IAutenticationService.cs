using Luveck.Service.Security.DTO;
using Luveck.Service.Security.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Repository.IRepository
{
    public interface IAutenticationService
    {
        Task<LoginResponseDto> Login(string user, string password);
        Task<LoginResponseDto> Register(RegisterUserRequestDto registerUserRequestDto);
        Task<RegisterUserResponseDto> Createuser(RegisterUserRequestDto user, string userRequest);
        Task<RegisterUserResponseDto> changePassword(ChangePasswordRequestDto changePasswordRequestDto);
        Task<RegisterUserResponseDto> forgotPassword(string Email);
        Task<LoginResponseDto> resetPassword(ResetPasswordRequestDto resetPasswordRequestDto);
        Task<UserResponseDto> getUserByEmail(string Email);
        Task<UserResponseDto> getUserByDNI(string DNI);
        Task<UserResponseDto> getUserById(string Id);
        Task<RegisterUserResponseDto> updateUser(RegisterUserRequestDto user, string roleRequestor);
        Task<List<UserListResponseDto>> getUsers();
        Task<bool> sendMail(string user);
    }
}
