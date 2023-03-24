using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Utils.Jwt.Interface
{
    public interface IHeaderClaims
    {
        /// <summary>
        /// Method to get value claim from JwtToken
        /// </summary>
        /// <param name="token"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        string GetClaimValue(string token, string claim);
        string GetJwtToken(string token);
    }
}
