using Chat.Api.TestData;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Api.Hubs
{
    public class ConnectionList : IReadOnlyCollection<User>
    {
        /// <summary>
        /// 在线连接的集合
        /// </summary>
        private readonly ConcurrentDictionary<string, User> _connections = new ConcurrentDictionary<string, User>();
        /// <summary>
        /// 用户Id对应的用户对象
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public User this[string userId]
        {
            get
            {
                if (_connections.TryGetValue(userId, out var connection))
                {
                    return connection;
                }
                return null;
            }
        }
        /// <summary>
        /// 连接数量
        /// </summary>
        public int Count => _connections.Count;
        /// <summary>
        /// 加入用户
        /// </summary>
        /// <param name="user">用户对象</param>
        public void Add(User user)
        {
            _connections.TryAdd(user.Id, user);
        }
        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        public void Remove(string userId)
        {
            _connections.TryRemove(userId, out var dummy);
        }
        /// <summary>
        /// 实现接口
        /// </summary>
        /// <returns></returns>
        public IEnumerator<User> GetEnumerator()
        {
            foreach (var item in _connections)
            {
                yield return item.Value;
            }
        }
        /// <summary>
        /// 实现接口
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
