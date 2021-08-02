using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using QCodes.Models;
using QCodes.Repository;

namespace QCodes.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PrivateMessageHub : Hub
    {
        private readonly IUserAndPersonRepository _userAndPersonRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private InMemoryDbForUserInfo _inMemoryDbForUserInfo;
        public PrivateMessageHub(IUserAndPersonRepository userAndPersonRepository, IHttpContextAccessor httpContextAccessor,InMemoryDbForUserInfo inMemoryDbForUserInfo)
        {
            _inMemoryDbForUserInfo = inMemoryDbForUserInfo;
            _userAndPersonRepository = userAndPersonRepository;
            _httpContextAccessor = httpContextAccessor;
            
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
        public Task SendDirectMessage(PrivateMessageModel message)
        {
            string senderUserId = GetUserId().ToString();
            message.Sender = senderUserId;
            var receiverUserId = message.Receiver;
            var userInfoSender = _inMemoryDbForUserInfo.GetUserInfo(senderUserId);
            var userInfoReciever = _inMemoryDbForUserInfo.GetUserInfo(receiverUserId);
            return Clients.Client(userInfoReciever.ConnectionId).SendAsync("SendDM", message);
        }

    }
}
