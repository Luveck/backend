using Luveck.Service.Security.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Luveck.Service.Security.Models;

namespace Luveck.Service.Security.Repository
{
    public class JwtServices : IJwtServices
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;

        private readonly UserManager<User> _userManager;

        public JwtServices(UserManager<User> _userManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
            this._userManager = _userManager;

        }

        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        public async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
        {
            new Claim("UserName", user.Name),
            new Claim("UserId", user.Id),
            new Claim("Email", user.Email),
            new Claim("Name", user.Name),
            new Claim("LastName", user.LastName)
        };
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim("Role", String.Join(",", roles)));


            return claims;
        }
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings["validIssuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }
    }
}
