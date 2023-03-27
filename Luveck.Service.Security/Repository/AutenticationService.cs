using Luveck.Service.Security.Data;
using Luveck.Service.Security.DTO.Response;
using Luveck.Service.Security.DTO;
using Luveck.Service.Security.Repository.IRepository;
using Luveck.Service.Security.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Utils.Exceptions;
using Luveck.Service.Security.Utils.Resource;
using System.Collections.Generic;
using Luveck.Service.Security.UnitWork;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.Encoders;
using System.Xml.Linq;

namespace Luveck.Service.Security.Repository
{
    public class AutenticationService : IAutenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtServices _jwtServices;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;        
        private readonly IUnitOfWork _unitOfWork;

        public AutenticationService(UserManager<User> userManager, IJwtServices jwtServices,
             IConfiguration config, RoleManager<IdentityRole> roleManager, IMailService mailService,
             IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtServices = jwtServices;
            _mailService = mailService;
            _config = config;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterUserResponseDto> changePassword(ChangePasswordRequestDto changePasswordRequestDto)
        {
            var userExists = await _userManager.FindByEmailAsync(changePasswordRequestDto.mail.Trim());

            if (userExists == null) throw new BusinessException(GeneralMessage.UserNoExist);

            try
            {
                var result = await _userManager.ChangePasswordAsync(userExists, changePasswordRequestDto.password, changePasswordRequestDto.newPassword);

                if (result.Succeeded)
                {
                    userExists.changePass = false;
                    await _userManager.UpdateAsync(userExists);
                    return new RegisterUserResponseDto() { Code = "201", Message = GeneralMessage.PasswordChanged };
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            throw new BusinessException(GeneralMessage.GeneralError);
        }

        public async Task<RegisterUserResponseDto> Createuser(RegisterUserRequestDto user, string userRequest)
        {
            var userExists = await _userManager.FindByNameAsync(user.DNI.Trim());

            if (userExists != null) throw new BusinessException(GeneralMessage.DataRegistered);

            userExists = await _userManager.FindByEmailAsync(user.Email.Trim());

            if (userExists != null) throw new BusinessException(GeneralMessage.DataRegistered);

            User userNew = new User
            {
                UserName = user.DNI.Trim(),
                Email = user.Email,
                LastName = user.LastName,
                Name = user.Name,
                NormalizedEmail = user.Email.ToUpper(),
                changePass = true,
                State = true,
                PhoneNumber = user.Phone,
                BornDate = user.BornDate,
                Sex = user.Sex,
                CreateDate = DateTime.Now,
                CreatedBy = userRequest,
                Address = user.Address
            };

            string passwordGenerated = Shared.GenerateRandomPassword();

            try
            {
                var result = await _userManager.CreateAsync(userNew, passwordGenerated);                

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userNew, user.Role);
                    MailRequest request = new MailRequest();
                    request.Body = "Se genero una contraseña para su primer ingreso, esta debe ser cambiada una vez ingrese. Contraseña: " + passwordGenerated;
                    request.Subject = "Contraseña Generada";
                    request.ToEmail = user.Email;

                    await _mailService.SendEmailAsync(request);

                    return new RegisterUserResponseDto() { Code = "201", Message = GeneralMessage.SuccesRegister };
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            throw new BusinessException(GeneralMessage.ErrorRegistrando);
        }

        public async Task<RegisterUserResponseDto> forgotPassword(string Email)
        {
            var userExists = await _userManager.FindByEmailAsync(Email.Trim());

            if (userExists == null) throw new BusinessException(GeneralMessage.UserNoExist);
            try
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(userExists);
                code = code.Replace("+", "%2B");
                MailRequest request = new MailRequest();
                request.Body = "Para cambiar la contraseña <a href=\"" + _config.GetSection("Genearls:UrlForgot").Value + "/?mail=" + userExists.Email + "&id=" + code + "\">Da click aqui</a>";
                request.Subject = "Resetear Contraseña";
                request.ToEmail = userExists.Email;

                await _mailService.SendEmailAsync(request);

                return new RegisterUserResponseDto() { Code = "201", Message = GeneralMessage.forgotPassword + userExists.Email };
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            throw new BusinessException(GeneralMessage.GeneralError);
        }

        public async Task<UserResponseDto> getUserByEmail(string Email)
        {
            var userExists = await _userManager.FindByEmailAsync(Email.Trim());
            if (userExists == null) throw new BusinessException(GeneralMessage.UserNoExist);

            return new UserResponseDto
            {
                userEntity = userExists,
                Role = _userManager.GetRolesAsync(userExists).Result[0],
            };
            throw new NotImplementedException();
        }

        public async Task<UserResponseDto> getUserByDNI(string DNI)
        {
            var userExists = await _unitOfWork.UserRepository.Find(x=> x.UserName.ToUpper().Equals(DNI));
            if (userExists == null) throw new BusinessException(GeneralMessage.UserNoExist);

            return new UserResponseDto
            {
                userEntity = userExists,
                Role = _userManager.GetRolesAsync(userExists).Result[0],
            };
            throw new NotImplementedException();
        }

        public async Task<UserResponseDto> getUserById(string Id)
        {
            var userExists = await _userManager.FindByIdAsync(Id.Trim());
            if (userExists == null) throw new BusinessException(GeneralMessage.UserNoExist);

            return new UserResponseDto
            {
                userEntity = userExists,
                Role = _userManager.GetRolesAsync(userExists).Result[0],
            };
            throw new NotImplementedException();
        }
        public async Task<List<UserListResponseDto>> getUsers()
        {
            List<UserListResponseDto> list = new List<UserListResponseDto>();
            list = await _unitOfWork.UserRepository.AsQueryable().Select(x => new UserListResponseDto
            {
                Email = x.Email,
                UserName = x.UserName,
                Name = x.Name,
                LastName = x.LastName,
                state = x.State,
            }).ToListAsync();

            return list;
        }
        public async Task<LoginResponseDto> Login(string user, string password)
        {
            var userExists = await _userManager.FindByNameAsync(user);
            if (_userManager.IsLockedOutAsync(userExists).Result) throw new BusinessException(GeneralMessage.UserBloqValid);

            if (userExists == null || _userManager.PasswordHasher.VerifyHashedPassword(userExists, userExists.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                if (userExists != null) await _userManager.AccessFailedAsync(userExists);
                throw new BusinessException(GeneralMessage.UserNoValid);
            }

            if (await _userManager.IsLockedOutAsync(userExists))
            {
                throw new BusinessException(GeneralMessage.UserBloqValid);
            }

            await _userManager.ResetAccessFailedCountAsync(userExists);

            var signingCredentials = _jwtServices.GetSigningCredentials();
            var claims = await _jwtServices.GetClaims(userExists);
            var tokenOptions = _jwtServices.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            await _userManager.SetAuthenticationTokenAsync(userExists, "Luveck", "TokenLuveck", token);
            LoginResponseDto loginResponseDto = new LoginResponseDto
            {
                name = userExists.Name,
                lastName = userExists.LastName,
                token = token,
                HoraDesbloqueo = DateTime.Now,
                intentos = await _userManager.GetAccessFailedCountAsync(userExists),
                changePass = userExists.changePass,
                role = _userManager.GetRolesAsync(userExists).Result[0]
            };

            return loginResponseDto;
        }

        public async Task<LoginResponseDto> Register(RegisterUserRequestDto registerUserRequestDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerUserRequestDto.DNI.Trim());

            if (userExists != null) throw new BusinessException(GeneralMessage.DataRegistered);

            userExists = await _userManager.FindByEmailAsync(registerUserRequestDto.Email.Trim());

            if (userExists != null) throw new BusinessException(GeneralMessage.DataRegistered);

            User user = new User
            {
                UserName = registerUserRequestDto.DNI,
                Email = registerUserRequestDto.Email,
                LastName = registerUserRequestDto.LastName,
                Name = registerUserRequestDto.Name,
                NormalizedEmail = registerUserRequestDto.Email.ToUpper(),
                changePass = false,
                State = true,
                PhoneNumber = registerUserRequestDto.Phone,
                BornDate = registerUserRequestDto.BornDate,
                Sex = registerUserRequestDto.Sex,
                Address = registerUserRequestDto.Address
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registerUserRequestDto.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Cliente");

                    await _userManager.ResetAccessFailedCountAsync(user);

                    var signingCredentials = _jwtServices.GetSigningCredentials();
                    var claims = await _jwtServices.GetClaims(user);
                    var tokenOptions = _jwtServices.GenerateTokenOptions(signingCredentials, claims);
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    await _userManager.SetAuthenticationTokenAsync(user, "Luveck", "TokenLuveck", token);
                    LoginResponseDto loginResponseDto = new LoginResponseDto
                    {
                        name = user.Name,
                        lastName = user.LastName,
                        token = token,
                        HoraDesbloqueo = DateTime.Now,
                        intentos = await _userManager.GetAccessFailedCountAsync(user),
                        changePass = user.changePass,
                        role = _userManager.GetRolesAsync(user).Result[0]
                    };

                    return loginResponseDto;
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            throw new BusinessException(GeneralMessage.ErrorRegistrando);
        }

        public async Task<LoginResponseDto> resetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var userExists = await _userManager.FindByEmailAsync(resetPasswordRequestDto.Email.Trim());

            if (userExists == null) throw new BusinessException(GeneralMessage.UserNoExist);

            try
            {
                var result = await _userManager.ResetPasswordAsync(userExists, resetPasswordRequestDto.Code, resetPasswordRequestDto.newPassword);
                if (result.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(userExists);

                    var signingCredentials = _jwtServices.GetSigningCredentials();
                    var claims = await _jwtServices.GetClaims(userExists);
                    var tokenOptions = _jwtServices.GenerateTokenOptions(signingCredentials, claims);
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    await _userManager.SetAuthenticationTokenAsync(userExists, "Luveck", "TokenLuveck", token);
                    LoginResponseDto loginResponseDto = new LoginResponseDto
                    {
                        name = userExists.Name,
                        lastName = userExists.LastName,
                        token = token,
                        HoraDesbloqueo = DateTime.Now,
                        intentos = await _userManager.GetAccessFailedCountAsync(userExists),
                        changePass = userExists.changePass,
                        role = _userManager.GetRolesAsync(userExists).Result[0]
                    };

                    return loginResponseDto;
                }
                throw new BusinessException(GeneralMessage.Error500);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public async Task<RegisterUserResponseDto> updateUser(RegisterUserRequestDto user, string roleRequestor)
        {
            var userExists = await _userManager.FindByNameAsync(user.DNI.Trim());
            if (userExists == null) throw new BusinessException(GeneralMessage.UserNoExist);
            if (!userExists.Email.ToUpper().Equals(user.Email.ToUpper()))
            {
                var emailExist = await _userManager.FindByEmailAsync(user.Email.Trim());
                if (emailExist != null) throw new BusinessException(GeneralMessage.EmialUsed);
            }

            userExists.Email = user.Email.Trim();
            userExists.NormalizedEmail = user.Email.ToUpper().Trim();
            userExists.Name = user.Name.Trim();
            userExists.LastName = user.LastName.Trim();
            userExists.PhoneNumber = user.Phone.Trim();
            userExists.Sex = user.Sex.Trim();
            userExists.State = user.state;
            userExists.Address = user.Address;

            try
            {
                var result = await _userManager.UpdateAsync(userExists);

                if (result.Succeeded)
                {
                    var roleRequest = await _roleManager.FindByNameAsync(roleRequestor);
                    if (!string.IsNullOrEmpty(user.Role) && roleRequest.ConcurrencyStamp == "0")
                    {
                        var currentRole = _userManager.GetRolesAsync(userExists).Result[0];
                        await _userManager.RemoveFromRoleAsync(userExists, currentRole);
                        await _userManager.AddToRoleAsync(userExists, user.Role);
                    }
                    return new RegisterUserResponseDto() { Code = "201", Message = GeneralMessage.UserUpdate };
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            throw new BusinessException(GeneralMessage.GeneralError);
        }

        public async Task<bool> sendMail(string user)
        {
            var userExists = await _userManager.FindByIdAsync(user);

            if (userExists == null) throw new BusinessException(GeneralMessage.UserNoExist);
            try
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(userExists);
                MailRequest request = new MailRequest();
                request.Body = "Hola " + userExists.Name + "<br><br>" +
                    "Le informamos que usted cuenta con canjes pendientes, por favor acérquese a su farmacia de confianza para reclamarlos." +
                    "<br><br><br>Feliz Dia !!";
                request.Subject = "Buenas Noticias de Luveck !!";
                request.ToEmail = userExists.Email;

                await _mailService.SendEmailAsync(request);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
