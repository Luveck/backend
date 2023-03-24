using Luveck.Service.Security.DTO;
using Luveck.Service.Security.DTO.Response;
using Luveck.Service.Security.Handlers;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Repository.IRepository;
using Luveck.Service.Security.Utils.Jwt.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static Luveck.Service.Security.Utils.enums.Enums;

namespace Luveck.Service.Security.Controllers
{    
    [Route("api/Security")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    [ApiExplorerSettings(GroupName = "ApiSecurityAccount")]
    public class AccountController : Controller
    {
        private readonly IAutenticationService autenticationServices;
        private readonly IHeaderClaims headerClaims;

        public AccountController(IAutenticationService autenticationServices, IHeaderClaims headerClaims)
        {
            this.autenticationServices = autenticationServices;
            this.headerClaims = headerClaims;
        }

        [Route("Register")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register(RegisterUserRequestDto requestUser)
        {
            var token = await autenticationServices.Register(requestUser);

            var response = new ResponseModel<LoginResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = token

            };
            return Ok(response);
        }

        [Route("Login")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login(LoginUserRequestDto requestUser)
        {
            var token = await autenticationServices.Login(requestUser.DNI, requestUser.Password);

            var response = new ResponseModel<LoginResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = token

            };
            return Ok(response);

        }

        [Route("CreateUser")]
        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateUser(RegisterUserRequestDto requestUser)
        {
            string user = this.headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            var token = await autenticationServices.Createuser(requestUser, user);

            var response = new ResponseModel<RegisterUserResponseDto>()
            {
                IsSuccess = token.Code == "201" ? true : false,
                Messages = token.Message,
                Result = token

            };
            return Ok(response);
        }

        [Route("ChangePassword")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto changePassword)
        {
            var token = await autenticationServices.changePassword(changePassword);

            var response = new ResponseModel<RegisterUserResponseDto>()
            {
                IsSuccess = token.Code == "201" ? true : false,
                Messages = token.Message,
                Result = token

            };
            return Ok(response);
        }

        [Route("Forgot")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto forgot)
        {
            var token = await autenticationServices.forgotPassword(forgot.Email);

            var response = new ResponseModel<RegisterUserResponseDto>()
            {
                IsSuccess = token.Code == "201" ? true : false,
                Messages = token.Message,
                Result = token

            };
            return Ok(response);
        }

        [Route("Reset")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto reset)
        {
            var token = await autenticationServices.resetPassword(reset);

            var response = new ResponseModel<LoginResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = token

            };
            return Ok(response);
        }

        [Route("getUserByEmail")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getUserByEmal(string Email)
        {
            var token = await autenticationServices.getUserByEmail(Email);

            var response = new ResponseModel<UserResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = token

            };
            return Ok(response);
        }

        [Route("getUserByDNI")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getUserByDNI(string DNI)
        {
            var token = await autenticationServices.getUserByDNI(DNI);

            var response = new ResponseModel<UserResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = token

            };
            return Ok(response);
        }

        [Route("getUserByID")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getUserByID(string Id)
        {
            var token = await autenticationServices.getUserById(Id);

            var response = new ResponseModel<UserResponseDto>()
            {
                IsSuccess = true,
                Messages = "",
                Result = token

            };
            return Ok(response);
        }

        [Route("UpdateUser")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUser(RegisterUserRequestDto user)
        {
            string roleRequestor = this.headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.Role);
            var token = await autenticationServices.updateUser(user, roleRequestor);

            var response = new ResponseModel<RegisterUserResponseDto>()
            {
                IsSuccess = token.Code == "201" ? true : false,
                Messages = token.Message,
                Result = token

            };
            return Ok(response);
        }

        [Route("GetUsers")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var result = await autenticationServices.getUsers();

            var response = new ResponseModel<List<UserListResponseDto>>()
            {
                IsSuccess = true,
                Messages = "",
                Result = result

            };
            return Ok(response);
        }

        [Route("SendMail")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SendMail(string user)
        {
            var result = await autenticationServices.sendMail(user);
            return Ok(result);
        }

        ////[HttpPost]
        ////[Route("ConfirmEmail")]
        ////[AllowAnonymous]
        ////public async Task<IActionResult> ConfirmEmail(string userId, string Code)
        ////{
        ////    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(Code))
        ////    {
        ////        return BadRequest("Los campos estan vacios.");
        ////    }
        ////    var user = await _userManager.FindByIdAsync(userId);
        ////    if (user == null)
        ////    {
        ////        return BadRequest("Información incorrecta.");
        ////    }

        ////    var result = await _userManager.ConfirmEmailAsync(user, Code);

        ////    if (result.Succeeded)
        ////    {
        ////        return Ok(result);
        ////    }
        ////    return BadRequest("Información incorrecta.");
        ////}
    }
}
