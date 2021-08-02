using QCodes.DbObjects;
using QCodes.Services;
using System.Threading.Tasks;

namespace QCodes.Repository
{
    public interface IMessageRepository
    {
        Task<GlobalMessage> SendGlobalMessage(GlobalMessage globalMessage);
        Task<PaginationService<GlobalMessage>> GetGlobalMessages(MessageParams messageParams);
        Task<PrivateMessage> SendPrivateMessage(PrivateMessage privateMessage);
        Task<PaginationService<PrivateMessage>> GetPrivateMessages(MessageParams messageParams, string userId);
    }
}