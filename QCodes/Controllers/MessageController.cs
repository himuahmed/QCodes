using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using QCodes.DbObjects;
using QCodes.Hubs;
using QCodes.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace QCodes.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageController(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [Route("send")]                                 
        [HttpPost]
        public IActionResult SendRequest([FromBody] GlobalMessageModel msg)
        {
            msg.date = DateTime.Now.ToString();
            msg.userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString();
            _hubContext.Clients.All.SendAsync("globalMessageReceived", msg);
            return Ok();
        }
    }
}
