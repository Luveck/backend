using Luveck.Service.Security.Models;
using Luveck.Service.Security.Models.Dtos;
using Luveck.Service.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly IEmailSender _emailSender;

        private readonly IMailService _mailService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, 
            IConfiguration configuration, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _mailService = mailService;
        }

        //[HttpPost]
        //[Route("Register")]
        //[AllowAnonymous]
        //[ProducesResponseType(200, Type = typeof(string))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status501NotImplemented)]
        //[ProducesDefaultResponseType]
        //public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        //{

        //    var user = new User
        //    {
        //        UserName = userRegisterDto.Email,
        //        Email = userRegisterDto.Email,
        //        State = true,
        //    };

        //    var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

        //    if (result.Succeeded)
        //    {
        //        //Asignacion role - Pendiente rol a la creacion
        //        //await _userManager.addToleAsync(user, rol);
        //        //
        //        // Creacion Token para confirmacion de email si es necesario
        //        // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //        // Se envia por correo en caso tal de ser necesario

        //        await _signInManager.SignInAsync(user, isPersistent: false);
        //        return Ok(result);
        //    }

        //    return BadRequest();
        //}

        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<IActionResult> Create(CreateUserDto userDto)
        {
            var reviewMail = await _userManager.FindByEmailAsync(userDto.Email);
            if (reviewMail != null) return BadRequest("Usuario ya existe con el correo electronico creado");

            var reviewUserName = await _userManager.FindByNameAsync(userDto.UserName);
            if (reviewMail != null) return BadRequest("nombre de usuario ya existe.");

            var user = new User
            {
                UserName = userDto.UserName,
                DNI = userDto.DNI,
                Email = userDto.Email,
                Name = userDto.Name,
                LastName = userDto.LastName,
                State = true
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
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
            //var user = _userManager.FindByEmailAsync(email);
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
            loginAuthDto.RememberMe, lockoutOnFailure: false);

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
                return Ok(new
                {
                    resultado = result,
                    token = tokenHandler.WriteToken(token),
                    intentos = user.AccessFailedCount,
                    HoraDesbloqueo = DateTime.Now

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
                request.Body = "Para cambiar la contraseña <a href=\"" + _config.GetSection("Genearls:UrlForgot").Value + "?id=" + code + "\">Da click aqui</a>";
                request.Subject = "Resetear Contraseña";
                request.ToEmail = user.Email;

                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;

            }
           // await _emailSender.SendEmailAsync(user.Email, "Resetear Contraseña", "Para cambiar la contraseña <a href=\"" + _config.GetSection("Genearls:UrlForgot").Value + "?id=" + code + "\">Da click aqui</a>");

            return Ok();
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
                return Ok(result);
            }
            return BadRequest("Información incorrecta.");
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
