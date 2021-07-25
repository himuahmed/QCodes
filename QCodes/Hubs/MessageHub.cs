using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using QCodes.DbObjects;
using QCodes.Models;

namespace QCodes.Hubs
{
   // [Authorize(AuthenticationSchemes = "Bearer")]
    public class MessageHub : Hub
    {
       public async Task GlobalChat(GlobalMessageModel message)
        {
            message.date = DateTime.Now.ToString();
            await Clients.All.SendAsync("globalMessageReceived", message);
        }
    }
}
