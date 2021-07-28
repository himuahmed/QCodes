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
    }
}
