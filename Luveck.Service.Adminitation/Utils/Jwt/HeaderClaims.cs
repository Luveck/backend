using Luveck.Service.Administration.Utils.Jwt.Interface;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;


namespace Luveck.Service.Administration.Utils.Jwt
{
    [ExcludeFromCodeCoverage]
    public class HeaderClaims : IHeaderClaims
    {
        /// <summary>
        /// Method to get value claim from JwtToken
        /// </summary>
        /// <param name="authorization"> Request.Headers["Authorization"] </param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public string GetClaimValue(string token, string claim)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            string authHeader = token.Replace("Bearer ", "").Replace("bearer ", "");
            JwtSecurityToken tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;

            Claim claimData = tokenS.Claims.FirstOrDefault(cl => cl.Type.ToUpper() == claim.ToUpper());

            return claimData != null ? claimData.Value : string.Empty;
        }

        /// <summary>
        /// Method to get JwtToken
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string GetJwtToken(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            string authHeader = token.Replace("Bearer ", "").Replace("bearer ", "");

            return authHeader;
        }
    }
}
