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
            var messageObj = await _dataContext.PrivateMessages.AddAsync(privateMessage);
            var messageEntity = messageObj.Entity;
            var res = await _dataContext.SaveChangesAsync();
            return messageEntity;
        }

        public async Task<PaginationService<PrivateMessage>> GetPrivateMessages(MessageParams messageParams, string userId)
        {
            IQueryable<PrivateMessage> privateMesageList = _dataContext.Set<PrivateMessage>().Where(m=>m.Sender == userId || m.Receiver == userId).OrderByDescending(m => m.CreatedAt).AsQueryable();
            return await PaginationService<PrivateMessage>.CreateAsync(privateMesageList, messageParams.PageNumber, messageParams.PageSize);
        }

    }
}
