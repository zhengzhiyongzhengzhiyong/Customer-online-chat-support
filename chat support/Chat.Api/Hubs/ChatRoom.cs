using Chat.Api.Controllers;
using Chat.Api.TestData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Chat.Api.Hubs
{
    [Authorize]
    public class ChatRoom : Hub
    {
        private ConnectionList Connections
        {
            get
            {
                return Context.GetHttpContext().RequestServices.GetService(typeof(ConnectionList)) as ConnectionList;
            }
        }
        private string CurrentUser
        {
            get
            {
               return Context.User.FindFirst("sub").ToString();
            }
        }
        /// <summary>
        /// 客户端连接时触发
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var currUser = SessionManager.CurrUser;
            Connections.Add(currUser);
            //发送在线人数给当前客户端
            await Clients.Caller.SendAsync("OnlineCount", Connections.Count);
            //通知其他客户端有人上线
            await Clients.Others.SendAsync("UserOnline", currUser.Name);
        }
        /// <summary>
        /// 客户端断开连接时触发
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var currUser = SessionManager.CurrUser;
            Connections.Remove(currUser.Id);
            //通知其他客户端有人下线
            await Clients.Others.SendAsync("UserOffline", currUser.Name);
        }
        /// <summary>
        /// 客户端发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;
            var currUser = SessionManager.CurrUser;
            //发送给除自己以外的所有客户端
            Clients.Others.SendAsync("Receive", new { Msg = message, UserAvatar = currUser.Avatar, UserName = currUser.Name, Mine = false });
        }
    }
}
