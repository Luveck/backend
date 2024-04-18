using Luveck.Service.Administration.Utils.Jwt.Interface;
using Luveck.Service.Administration.Utils.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace Luveck.Service.Administration.Handlers
{
    [ExcludeFromCodeCoverage]
    public class CacheGlobalVariables
    {
        private readonly IHeaderClaims _headerClaims;
        private readonly IUtils _utils;

        public CacheGlobalVariables(IHeaderClaims headerClaims, IUtils utils)
        {
            _headerClaims = headerClaims;
            _utils = utils;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"];
            string id = _headerClaims.GetClaimValue(token, "UserId");
            var tokenDto = new TokenDto()
            {
                Token = token.Replace("Bearer ", string.Empty)
            };

            _utils.SaveDataInCache("SecurityToken", tokenDto, "Default");
            _utils.SaveDataInCache("UserId", id, "Default");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _utils.RemoveDataInCache("id");
        }
    }
}
