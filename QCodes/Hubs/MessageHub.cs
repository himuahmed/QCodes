using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace QCodes.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MessageHub : Hub
    {
        //public async Task GlobalChat(GlobalMessageModel message)
        //{
        //    //string userId = "kjfkjhf";
        //    message.date = DateTime.Now.ToString();
        //    //await Clients.All.SendAsync("globalMessageReceived", message);
        //    await Clients.User(userId).SendAsync("globalMessageReceived", message);
        //}
    }
}
