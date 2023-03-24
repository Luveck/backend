using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Controllers
{
    [Route("api/Massive")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiMassive")]
    public class MassiveMailController : ControllerBase
    {
        public ISendPedingExchange sendPedingExchange;

        public MassiveMailController(ISendPedingExchange sendPedingExchange )
        {
            this.sendPedingExchange = sendPedingExchange;
        }

        [HttpGet]
        [Route("GetMasive")]
        public async Task<IActionResult> GetCategories()
        {
            await sendPedingExchange.sendMasiveMailRemainder();
            return Ok();
        }
    }
}
