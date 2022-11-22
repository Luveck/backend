using Luveck.Service.Security.DTO;
using Luveck.Service.Security.DTO.Response;
using Luveck.Service.Security.Handlers;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Repository.IRepository;
using Luveck.Service.Security.Utils.Jwt.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateUser(RegisterUserRequestDto requestUser, string role)
        {
            var token = await autenticationServices.Createuser(requestUser, role);

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

        [Route("getUser")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getUser(string Email)
        {
            var token = await autenticationServices.getUser(Email);

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
        public async Task<IActionResult> UpdateUser(RegisterUserRequestDto user, string role)
        {
            string roleRequestor = this.headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.Role);
            var token = await autenticationServices.updateUser(user, role, roleRequestor);

            var response = new ResponseModel<RegisterUserResponseDto>()
            {
                IsSuccess = token.Code == "201" ? true : false,
                Messages = token.Message,
                Result = token

            };
            return Ok(response);
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
