using QCodes.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace QCodes.Hubs
{
    public class InMemoryDbForUserInfo
    {
        private ConcurrentDictionary<string, HubUserInfo> _onlineUser { get; set; } = new ConcurrentDictionary<string, HubUserInfo>();

        public bool AddUpdate(string UserId, string connectionId)
        {
            var userAlreadyExists = _onlineUser.ContainsKey(UserId);

            var userInfo = new HubUserInfo
            {
                UserId = UserId,
                ConnectionId = connectionId
            };

            _onlineUser.AddOrUpdate(UserId, userInfo, (key, value) => userInfo);

            return userAlreadyExists;
        }

        public void Remove(string name)
        {
            HubUserInfo userInfo;
            _onlineUser.TryRemove(name, out userInfo);
        }

        public IEnumerable<HubUserInfo> GetAllUsersExceptThis(string userId)
        {
            return _onlineUser.Values.Where(u => u.UserId != userId);
        }

        public HubUserInfo GetUserInfo(string userId)
        {
            HubUserInfo user;
            _onlineUser.TryGetValue(userId, out user);
            return user;
        }
    }
}
