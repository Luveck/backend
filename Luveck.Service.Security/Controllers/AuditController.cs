using Luveck.Service.Security.DTO;
using Luveck.Service.Security.DTO.Response;
using Luveck.Service.Security.Handlers;
using Luveck.Service.Security.Repository.IRepository;
using Luveck.Service.Security.Utils.Jwt.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using static Luveck.Service.Security.Utils.enums.Enums;

namespace Luveck.Service.Security.Controllers
{
    [Authorize]
    [Route("api/Audit")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAudit")]
    [TypeFilter(typeof(CustomExceptionAttribute))]
    public class AuditController : ControllerBase
    {
        private readonly IHeaderClaims _headerClaims;
        private readonly IAuditService auditService;

        public AuditController(IHeaderClaims headerClaims, IAuditService auditService)
        {
            _headerClaims = headerClaims;
            this.auditService = auditService;
        }

        [Route("Audit")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Audit(AuditRequestDto audit)
        {
            string user = this._headerClaims.GetClaimValue(Request.Headers["Authorization"], ClaimsToken.UserId);
            bool result = await auditService.RegisterAudit(audit, user);

            return Ok(result);

        }
    }
}
