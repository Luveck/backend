﻿using Luveck.Service.Security.Data;
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

namespace Luveck.Service.Security.Repository
{
    public class AutenticationService : IAutenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtServices _jwtServices;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AutenticationService(UserManager<User> userManager, IJwtServices jwtServices,
             IConfiguration config, RoleManager<IdentityRole> roleManager, IMailService mailService)
        {
            _userManager = userManager;
            _jwtServices = jwtServices;
            _mailService = mailService;
            _config = config;
            _roleManager = roleManager;
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
                    return new RegisterUserResponseDto() { Code = "201", Message = GeneralMessage.PasswordChanged };
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            throw new BusinessException(GeneralMessage.GeneralError);
        }

        public async Task<RegisterUserResponseDto> Createuser(RegisterUserRequestDto user, string role)
        {
            var userExists = await _userManager.FindByNameAsync(user.DNI.Trim());

            if (userExists != null) throw new BusinessException(GeneralMessage.DataRegistered);

            userExists = await _userManager.FindByEmailAsync(user.Email.Trim());

            if (userExists != null) throw new BusinessException(GeneralMessage.DataRegistered);

            User userNew = new User
            {
                Email = user.Email,
                LastName = user.LastName,
                Name = user.Name,
                NormalizedEmail = user.Email.ToUpper(),
                changePass = false,
                State = true,
                PhoneNumber = user.Phone
            };

            string passwordGenerated = Shared.GenerateRandomPassword();

            try
            {
                var result = await _userManager.CreateAsync(userNew, user.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userNew, role);
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
                MailRequest request = new MailRequest();
                request.Body = "Para cambiar la contraseña <a href=\"" + _config.GetSection("Genearls:UrlForgot").Value + "/?mail=" + userExists.Email + "?id=" + code + "\">Da click aqui</a>";
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

        public async Task<UserResponseDto> getUser(string Email)
        {
            var userExists = await _userManager.FindByEmailAsync(Email.Trim());
            if (userExists == null) userExists = await _userManager.FindByNameAsync(Email.Trim());
            if (userExists == null) throw new BusinessException(GeneralMessage.UserNoExist);

            return new UserResponseDto
            {
                userEntity = userExists,
                Role = _userManager.GetRolesAsync(userExists).Result[0],
            };
            throw new NotImplementedException();
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
                PhoneNumber = registerUserRequestDto.Phone
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
            throw new NotImplementedException();

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
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public async Task<RegisterUserResponseDto> updateUser(RegisterUserRequestDto user, string role, string roleRequestor)
        {
            var userExists = await _userManager.FindByNameAsync(user.DNI.Trim());
            if (userExists == null) throw new BusinessException(GeneralMessage.UserNoExist);
            userExists = await _userManager.FindByEmailAsync(user.Email.Trim());
            if (userExists != null) throw new BusinessException(GeneralMessage.EmialUsed);
            User entityUser = new User
            {
                Email = user.Email.Trim(),
                NormalizedEmail = user.Email.ToUpper().Trim(),
                Name = user.Name.Trim(),
                LastName = user.LastName.Trim(),
                PhoneNumber = user.Phone.Trim(),
                UserName = user.DNI.Trim()
            };
            try
            {
                var result = await _userManager.UpdateAsync(entityUser);

                if (result.Succeeded)
                {
                    var roleRequest = await _roleManager.FindByNameAsync(roleRequestor);
                    if (!string.IsNullOrEmpty(role) && roleRequest.ConcurrencyStamp == "0")
                    {
                        var currentRole = _userManager.GetRolesAsync(entityUser).Result[0];
                        await _userManager.RemoveFromRoleAsync(entityUser, currentRole);
                        await _userManager.AddToRoleAsync(entityUser, role);
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
    }
}
