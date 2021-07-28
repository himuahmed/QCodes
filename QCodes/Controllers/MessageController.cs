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
using AutoMapper;
using QCodes.Repository;
using QCodes.Services;

namespace QCodes.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;

        public MessageController(IHubContext<MessageHub> hubContext, IMapper mapper,IMessageRepository messageRepository)
        {
            _hubContext = hubContext;
            _mapper = mapper;
            _messageRepository = messageRepository;
        }

        [Route("send")]                                 
        [HttpPost]
        public async Task<IActionResult> SendRequest([FromBody] GlobalMessageModel msg)
        {
            if (!ModelState.IsValid) return BadRequest("Couldn't sent message.");
            msg.date = DateTime.Now.ToString();
            msg.userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString();
            var messageObj = _mapper.Map<GlobalMessage>(msg);
            var isMessageSent = await _messageRepository.SendGlobalMessage(messageObj);
            var message = _mapper.Map<GlobalMessageModel>(isMessageSent);
            await _hubContext.Clients.All.SendAsync("globalMessageReceived", message);
            return Ok();
        }

        [AllowAnonymous]
        [Route("GetGlobalMessage")]
        [HttpGet]
        public async Task<IActionResult> GetGlobalMessages([FromQuery] MessageParams messageParams)
        {
            var messageList = await _messageRepository.GetGlobalMessages(messageParams);
            if (messageList.Count != 0) return Ok(messageList);
            return Ok();
        }
    }
}
