using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using QCodes.DbObjects;
using QCodes.Models;
using QCodes.Repository;

namespace QCodes.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PrivateMessageHub : Hub
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private InMemoryDbForUserInfo _inMemoryDbForUserInfo;
        private IMessageRepository _messageRepository;
        public PrivateMessageHub(IHttpContextAccessor httpContextAccessor,InMemoryDbForUserInfo inMemoryDbForUserInfo, IMessageRepository messageRepository)
        {
            _inMemoryDbForUserInfo = inMemoryDbForUserInfo;
            _httpContextAccessor = httpContextAccessor;
            _messageRepository = messageRepository;
            
        }

        protected string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }

        public async Task Leave()
        {
            _inMemoryDbForUserInfo.Remove(Context.User.Identity.Name);

            await Clients.AllExcept(new List<string> { Context.ConnectionId }).SendAsync("UserLeft", Context.User.Identity.Name);
        }

        public async Task Join()
        {
            var userId = GetUserId().ToString();

            if (!_inMemoryDbForUserInfo.AddUpdate(userId, Context.ConnectionId))
            {
                // new user

                var list = _inMemoryDbForUserInfo.GetAllUsersExceptThis(userId).ToList();
                await Clients.AllExcept(new List<string> { userId }).SendAsync(
                    "NewOnlineUser",
                    _inMemoryDbForUserInfo.GetUserInfo(userId)
                    );
            }
            else
            {
                // existing user joined again

            }

            await Clients.Client(Context.ConnectionId).SendAsync(
                "Joined",
                _inMemoryDbForUserInfo.GetUserInfo(userId)
                );

            await Clients.Client(Context.ConnectionId).SendAsync(
                "OnlineUsers",
                _inMemoryDbForUserInfo.GetAllUsersExceptThis(userId)
            );
        }

        public async Task getConnectionId()
        {
           await  Clients.Client(Context.ConnectionId).SendAsync("getConnectionId", Context.ConnectionId);
        }
        public async Task SendDirectMessage(PrivateMessage message)
        {
            string senderUserId = GetUserId().ToString();
            message.Sender = senderUserId;
            message.isDelivered = false;
            message.CreatedAt = DateTime.Now;
            var receiverUserId = message.Receiver;
            var userInfoSender = _inMemoryDbForUserInfo.GetUserInfo(senderUserId);
            var userInfoReciever = _inMemoryDbForUserInfo.GetUserInfo(receiverUserId);
            if(userInfoReciever != null)
            {
                //message.isDelivered = true;

                var res = await _messageRepository.SendPrivateMessage(message);
                if (res != null)
                {
                  await Clients.Client(userInfoReciever.ConnectionId).SendAsync("SendDM", message);
                  await Clients.Client(userInfoSender.ConnectionId).SendAsync("SendDM", message);
                }
                
            }
            else
            {
                await _messageRepository.SendPrivateMessage(message);
                await Clients.Client(userInfoSender.ConnectionId).SendAsync("SendDM", message);
            }
            
        }

        public async Task UpdateMessageStatus(string receiverId)
        {
            string userId = GetUserId().ToString();
            var res = await _messageRepository.UpdateMessageStatus(userId, receiverId);
            var userInfoSender = _inMemoryDbForUserInfo.GetUserInfo(userId);
            await Clients.Client(userInfoSender.ConnectionId).SendAsync("updateMessageStatus", res);
        }



    }
}
