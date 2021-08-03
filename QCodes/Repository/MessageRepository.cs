using QCodes.Data;
using QCodes.DbObjects;
using QCodes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _dataContext;
        public MessageRepository(DataContext datacontext)
        {
            _dataContext = datacontext;
        }
        public async Task<GlobalMessage> SendGlobalMessage(GlobalMessage globalMessage)
        {
            var messageObj = await _dataContext.GlobalMessages.AddAsync(globalMessage);
            var messageEntity = messageObj.Entity;
            await _dataContext.SaveChangesAsync();

            return messageEntity;
        }

        public async Task<PaginationService<GlobalMessage>> GetGlobalMessages(MessageParams messageParams)
        {
            IQueryable<GlobalMessage> GlobalMesageList =  _dataContext.Set<GlobalMessage>().AsQueryable().OrderByDescending(m=>m.date);
            return await PaginationService<GlobalMessage>.CreateAsync(GlobalMesageList, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<PrivateMessage> SendPrivateMessage(PrivateMessage privateMessage)
        {
            privateMessage.signature = privateMessage.Sender + "-" + privateMessage.Receiver;
            var messageObj = await _dataContext.PrivateMessages.AddAsync(privateMessage);
            var messageEntity = messageObj.Entity;
            var res = await _dataContext.SaveChangesAsync();
            return messageEntity;
        }

        public async Task<PaginationService<PrivateMessage>> GetPrivateMessages(MessageParams messageParams, string userId, string receiverId)
        {
            string signature = userId + "-" + receiverId;
            string signatureReverse = receiverId + "-" + userId;
            IQueryable<PrivateMessage> privateMesageList = _dataContext.Set<PrivateMessage>().Where(m=>m.signature == signature || m.signature == signatureReverse).OrderByDescending(m => m.CreatedAt).AsQueryable();
            return await PaginationService<PrivateMessage>.CreateAsync(privateMesageList, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<List<PrivateMessage>> GetChatList(string userId)
        {
            List<string> uniqueChatList = new List<string>();

            var allChats = _dataContext.PrivateMessages.Where(m => m.Sender == userId || m.Receiver == userId).OrderByDescending(m=>m.CreatedAt).ToList();

            foreach(var msg in allChats)
            {
                if(msg.Sender == userId)
                {
                    uniqueChatList.Add(msg.Receiver);
                }
                if(msg.Receiver== userId)
                {
                    uniqueChatList.Add(msg.Sender);
                }
            }
            List<PrivateMessage> latestMsgList = new List<PrivateMessage>();
            var chats = uniqueChatList.Distinct().ToList();
            foreach(var person in chats)
            {
                var lastMsg = _dataContext.PrivateMessages.Where(m => m.Sender == person || m.Receiver == person).OrderByDescending(m=>m.CreatedAt).FirstOrDefault();
                latestMsgList.Add(lastMsg);
            }
            return latestMsgList;
        }

    }
}
