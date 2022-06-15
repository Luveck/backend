using AutoMapper;
using Luveck.Service.Security.Data;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Models.Dtos;
using Luveck.Service.Security.Repository.IRepository;
using Luveck.Service.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Luveck.Service.Security.Controllers
{
    [Authorize]
    [Route("api/Security")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiSecurityAccount")]
    [ProducesResponseType(StatusCodes.Status501NotImplemented)]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext _db;

        private readonly IMailService _mailService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IConfiguration configuration, RoleManager<IdentityRole> roleManager, IEmailSender emailSender,
            IMailService mailService, IMapper mapper, AppDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _mailService = mailService;
            _mapper = mapper;
            _db = db;
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Register(RegisterUserDto userDto)
        {
            var role = new IdentityRole();
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {                
                role.Name = "Admin";
                role.NormalizedName = "ADMIN";
                role.ConcurrencyStamp = "0";
                await _roleManager.CreateAsync(role);
                role = new IdentityRole();
                role.Name = "Cliente";
                role.NormalizedName = "CLIENTE";
                role.ConcurrencyStamp = "1";
                await _roleManager.CreateAsync(role);
            }
            if (userDto.DNI.Length < 6)
            {
                return BadRequest("DNI Invalido");
            }
            var reviewMail = await _userManager.FindByEmailAsync(userDto.Email);
            if (reviewMail != null) return BadRequest("Usuario ya existe con el correo electronico creado");

            var reviewUserName = await _userManager.FindByNameAsync(userDto.DNI);
            if (reviewUserName != null) return BadRequest("nombre de usuario ya existe.");

            var user = new User
            {
                UserName = userDto.DNI,
                Email = userDto.Email,
                Name = userDto.Name,
                LastName = userDto.LastName,
                PhoneNumber = userDto.Phone,
                changePass = false,
                State = true
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
            {
                var roles = _db.Roles.ToList();
                role = roles.FirstOrDefault(x => x.ConcurrencyStamp == "1");
                await _userManager.AddToRoleAsync(user, role.Name);
                await _signInManager.SignInAsync(user, isPersistent: false);

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescritor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescritor);

                return Ok(new
                {
                    resultado = result,
                    token = tokenHandler.WriteToken(token),
                    intentos = user.AccessFailedCount,
                    HoraDesbloqueo = DateTime.Now,
                    changePass = false,
                    role = role,
                });
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> Create(CreateUserDto userDto)
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = "ADMIN";
                role.ConcurrencyStamp = "0";
                await _roleManager.CreateAsync(role);
                role = new IdentityRole();
                role.Name = "Cliente";
                role.NormalizedName = "CLIENTE";
                role.ConcurrencyStamp = "1";
                await _roleManager.CreateAsync(role);
            }
            var reviewMail = await _userManager.FindByEmailAsync(userDto.Email);
            if (reviewMail != null) return BadRequest("Usuario ya existe con el correo electronico creado");

            var reviewUserName = await _userManager.FindByNameAsync(userDto.DNI);
            if (reviewUserName != null) return BadRequest("nombre de usuario ya existe.");

            var reviewRol = await _roleManager.FindByNameAsync(userDto.Role);
            if (reviewUserName == null) return BadRequest("El role seleccionado no existe.");

            var user = new User
            {
                UserName = userDto.DNI,
                Email = userDto.Email,
                Name = userDto.Name,
                LastName = userDto.LastName,
                changePass = true,
                State = true
            };

            string passwordGenerated = Shared.GenerateRandomPassword();
            var result = await _userManager.CreateAsync(user, passwordGenerated);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, reviewRol.Name);
                MailRequest request = new MailRequest();
                request.Body = "Se genero una contraseña para su primer ingreso, esta debe ser cambiada una vez ingrese. Contraseña: " + passwordGenerated;
                request.Subject = "Contraseña Generada";
                request.ToEmail = user.Email;

                await _mailService.SendEmailAsync(request);

                return Ok(new
                {
                    mensaje = "Usuario creado exitosamente",
                    cracion = result
                });
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("LogOff")]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return Ok(_signInManager.Logger);
        }

        /// <summary>
        /// Metodo para hacer Login
        /// </summary>
        /// <param name="loginAuthDto"></param>
        /// <returns>
        /// {
        ///    "resultado": {
        ///        "succeeded": true,
        ///        "isLockedOut": false,
        ///        "isNotAllowed": false,
        ///        "requiresTwoFactor": false
        ///    },
        ///    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI2ZjhkZjJiMi0yNzZiLTQwOTAtOWY0My1hMDNlNWEwZTkxM2YiLCJ1bmlxdWVfbmFtZSI6Imxlc3NuZXIiLCJyb2xlIjoiMSIsIm5iZiI6MTY0NzI5MDAzNSwiZXhwIjoxNjQ3MzE4ODM1LCJpYXQiOjE2NDcyOTAwMzV9.ZbfQR9l_TPJIdI1H5_mjptPIIJt2dszWLGE3_gBOsW4",
        ///    "intentos": 0,
        ///    "horaDesbloqueo": "2021-03-03T15:23:07.307753"
        ///}
        /// </returns>
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> Login(LoginAuthDto loginAuthDto)
        {
            var valid = new IdentityUser();
            if (!string.IsNullOrEmpty(loginAuthDto.DNI))
            {
                valid = await _userManager.FindByNameAsync(loginAuthDto.DNI);
            }
            else
            {
                valid = await _userManager.FindByEmailAsync(loginAuthDto.Email);
            }

            if (valid == null)
            {
                return BadRequest("Usuario o contraseña invalido.");
            }
            var result = await _signInManager.PasswordSignInAsync(valid.UserName, loginAuthDto.Password,
            loginAuthDto.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginAuthDto.Email);
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescritor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescritor);
                var changePass = _db.User.FirstOrDefault(x => x.Id == user.Id).changePass;
                var role = (from ur in _db.UserRoles
                            join r in _db.Roles on ur.RoleId equals r.Id
                            where ur.UserId == user.Id
                            select r).Take(1).FirstOrDefault();

                return Ok(new
                {
                    resultado = result,
                    token = tokenHandler.WriteToken(token),
                    intentos = user.AccessFailedCount,
                    HoraDesbloqueo = DateTime.Now,
                    changePass = changePass,
                    role = role,
                });
            }

            if (result.IsLockedOut)
            {
                return BadRequest("Usuario bloqueado por maximo de intentos.");
            }

            return Unauthorized();
        }

        /// <summary>
        /// Metodo para el cambio de contraseña
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ChangePassword")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(IdentityUser))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            var user = await _userManager.FindByEmailAsync(changePassword.mail);

            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, changePassword.password, changePassword.newPassword);

                if (result.Succeeded)
                {
                    return Ok(user);
                }
                return Unauthorized("Se presento un error realizando el cambio del password.");
            }

            return BadRequest("Información enviada no es valida.");
        }

        [HttpPost]
        [Route("Forgot")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgot)
        {
            var user = await _userManager.FindByEmailAsync(forgot.Email);
            if (user == null)
            {
                return BadRequest("Información incorrecta.");
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            try
            {
                MailRequest request = new MailRequest();
                request.Body = "Para cambiar la contraseña <a href=\"" + _config.GetSection("Genearls:UrlForgot").Value + "/?mail=" + user.Email + "?id=" + code + "\">Da click aqui</a>";
                request.Subject = "Resetear Contraseña";
                request.ToEmail = user.Email;

                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Error enviando el correo para cambiar contraseña",
                    error = ex.Message,
                });

            }
            // await _emailSender.SendEmailAsync(user.Email, "Resetear Contraseña", "Para cambiar la contraseña <a href=\"" + _config.GetSection("Genearls:UrlForgot").Value + "?id=" + code + "\">Da click aqui</a>");


        }

        [HttpPost]
        [Route("Reset")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto reset)
        {
            var user = await _userManager.FindByEmailAsync(reset.Email);
            if (user == null)
            {
                return BadRequest("Información incorrecta.");
            }

            var result = await _userManager.ResetPasswordAsync(user, reset.Code, reset.Password);

            if (result.Succeeded)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescritor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescritor);
                var changePass = _db.User.FirstOrDefault(x => x.Id == user.Id).changePass;
                return Ok(new
                {
                    resultado = result,
                    token = tokenHandler.WriteToken(token),
                    intentos = user.AccessFailedCount,
                    HoraDesbloqueo = DateTime.Now,
                    changePass = changePass,
                });
            }
            return BadRequest("Información incorrecta.");
        }

        [HttpGet]
        [Route("getUser")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> getUser(string emailUser)
        {
            var user = new IdentityUser();
            if (!string.IsNullOrEmpty(emailUser))
            {
                user = await _userManager.FindByNameAsync(emailUser);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(emailUser);
                    if (user == null)
                    {
                        return BadRequest("Usuario ingresado no existe.");
                    }
                }
            }
            else
            {
                return BadRequest("Debe enviar datos para la consulta.");
            }

            var role = (from ur in _db.UserRoles
                        join r in _db.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select r).Take(1).FirstOrDefault();

            return Ok(new
            {
                user = _mapper.Map<userDto>(user),
                roleId = role.Id,
                roleName = role.Name,

            });
        }

        [HttpPost]
        [Route("UpdateUser")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> UpdateUser(userDto userDto, string emailAdmin)
        {
            var valid = await _userManager.FindByNameAsync(userDto.UserName);
            if (valid == null)
            {
                return BadRequest("la información suministrada no existe en sistema.");
            }
            var validMail = await _userManager.FindByEmailAsync(userDto.Email);
            if (validMail != null) return BadRequest("Correo electronico ya esta asignado.");

            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Name = userDto.Name,
                LastName = userDto.LastName,
                State = userDto.State,
            };

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded && string.IsNullOrEmpty(emailAdmin.Trim()))
            {
                return Ok(new
                {
                    user = user,
                    mensaje = "Usuario actualizado correctamente."
                });
            }

            if (result.Succeeded)
            {
                var userChange = await _userManager.FindByEmailAsync(emailAdmin);

                if ((_db.Roles.ToList()).FirstOrDefault(x => x.ConcurrencyStamp == "0").Id ==
                    ((from ur in _db.UserRoles
                      join r in _db.Roles on ur.RoleId equals r.Id
                      where ur.UserId == userChange.Id
                      select r).Take(1).FirstOrDefault()).Id)
                {
                    var deleteRole = (from ur in _db.UserRoles
                                      join r in _db.Roles on ur.RoleId equals r.Id
                                      where ur.UserId == user.Id
                                      select r).Take(1).FirstOrDefault();
                    await _userManager.RemoveFromRoleAsync(user, deleteRole.Name);
                    await _userManager.AddToRoleAsync(user, userDto.role);
                    return Ok(new
                    {
                        user = user,
                        mensaje = "Usuario actualizado correctamente."
                    });
                }
            }

            return BadRequest("Se presento un problema actualizando la información");
        }

        //[HttpPost]
        //[Route("ConfirmEmail")]
        //[AllowAnonymous]
        //public async Task<IActionResult> ConfirmEmail(string userId, string Code)
        //{
        //    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(Code))
        //    {
        //        return BadRequest("Los campos estan vacios.");
        //    }
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return BadRequest("Información incorrecta.");
        //    }

        //    var result = await _userManager.ConfirmEmailAsync(user, Code);

        //    if (result.Succeeded)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest("Información incorrecta.");
        //}
    }
}
