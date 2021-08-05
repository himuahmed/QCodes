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
        private readonly IHubContext<PrivateMessageHub> _privateHMsgubContext;
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserAndPersonRepository _userAndPersonRepository;
        //private  PrivateMessageHub _privateMessageHub;

        public MessageController(IHubContext<MessageHub> hubContext, IHubContext<PrivateMessageHub> privateHMsgubContext,
        IMapper mapper,IMessageRepository messageRepository, IUserAndPersonRepository userAndPersonRepository/*, PrivateMessageHub privateMessageHub*/)
        {
            _hubContext = hubContext;
            _mapper = mapper;
            _messageRepository = messageRepository;
            _userAndPersonRepository = userAndPersonRepository;
            //_privateHMsgubContext = privateHMsgubContext;
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
            //await _hubContext.Clients.User("cd3de73c-1a37-4173-a235-d2afc9735262").SendAsync("globalMessageReceived", message);
            return Ok();
        }

        [AllowAnonymous]
        [Route("GetGlobalMessage")]
        [HttpGet]
        public async Task<IActionResult> GetGlobalMessages([FromQuery] MessageParams messageParams)
        {
            var messageList = await _messageRepository.GetGlobalMessages(messageParams);
            Response.Headers(messageList.TotalCount, messageList.TotalPage, messageList.CurrentPage, messageList.PageSize);
            if (messageList.Count != 0) return Ok(messageList);
            return Ok();
        }

        //[Route("sendPM")]
        //[HttpPost]
        //public async Task<IActionResult> sendPrivateMessage(PrivateMessage priaveMessage)
        //{
        //    await _privateMessageHub.SendDirectMessage(priaveMessage);
        //    return Ok();
        //}

        [Route("GetPrivateMessages/{receiverId}")]
        [HttpGet]
        public async Task<IActionResult> GetPrivateMessages(string receiverId,[FromQuery] MessageParams messageParams)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString();
            var messageList = await _messageRepository.GetPrivateMessages(messageParams, userId,receiverId);
            Response.Headers(messageList.TotalCount, messageList.TotalPage, messageList.CurrentPage, messageList.PageSize);
            if (messageList.Count != 0) return Ok(messageList);
            return Ok();
        }

        [Route("GetChatList")]
        [HttpGet]
        public async Task<IActionResult> GetChatList()
        {
            List<ChatPersonList> chatList = new List<ChatPersonList>();
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString();

            var chats = await _messageRepository.GetChatList(userId);
            foreach(var msg in chats)
            {
                if(msg.Sender != userId)
                {
                    var personAssociated = await _userAndPersonRepository.GetPersonByUserId(msg.Sender);
                    personAssociated = _userAndPersonRepository.FilterPersonData(personAssociated);
                    ChatPersonList chatPerson = new ChatPersonList();


                    chatPerson.PersonImChattingWith = personAssociated;
                    chatPerson.LastMessage = msg;
                    
                    chatList.Add(chatPerson);
                }
                else if(msg.Sender == userId)
                {
                    var personAssociated = await _userAndPersonRepository.GetPersonByUserId(msg.Receiver);
                    personAssociated = _userAndPersonRepository.FilterPersonData(personAssociated);
                    ChatPersonList chatPerson = new ChatPersonList();

                    chatPerson.PersonImChattingWith = personAssociated;
                    chatPerson.LastMessage = msg;
                    
                    chatList.Add(chatPerson);
                }
            }
            return Ok(chatList);

        }

       



        //[Route("sendDM")]
        //[HttpPost]
        //public async Task<IActionResult> SendDirectMessage([FromBody] string message)
        //{
        //    string userId = "cd3de73c-1a37-4173-a235-d2afc9735262";
        //    var targetUserId = "75c9e2ad-2f5b-433d-b842-d0c787cc06d1";
        //    //string userId = "himu5";
        //    var userInfoSender = _userAndPersonRepository.GetUserInfo(userId);
        //    var userInfoReciever = _userAndPersonRepository.GetUserInfo(targetUserId);
        //    await _privateHMsgubContext.Clients.Client(userInfoReciever.ConnectionId).SendAsync("SendDM", message, userInfoSender);
        //    return Ok();
        //}
    }
}
