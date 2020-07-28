using Chat.Api.Controllers;
using Chat.Api.TestData;
using Chat.Common.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using StackExchange.Redis;
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
        private readonly IDatabase _redis;
        private ConnectionList _connections;
        public ChatRoom(RedisHelper client, ConnectionList Connections)
        {
            _redis = client.GetUserDB();
            _connections = Connections;
        }

        private User CurrentUser
        {
            get
            {
                string key = Context.User.FindFirst("id").Value;
                string result = _redis.StringGet(key);
                User user = JsonConvert.DeserializeObject<User>(result);                
                return user;
            }
        }
        public override async Task OnConnectedAsync()
        {
            _connections.Add(CurrentUser);   
            await Clients.Caller.SendAsync("OnlineCount", _connections.Count);
            await Clients.Others.SendAsync("UserOnline", CurrentUser.UserName);
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            _connections.Remove(CurrentUser.Id);
            await Clients.Others.SendAsync("UserOffline", CurrentUser.UserName);
        }

        public void Send(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;
            Clients.Others.SendAsync("Receive", new { Msg = message, UserAvatar = "", UserName = CurrentUser.UserName, Mine = false });
        }

        public void SendChatMessage(string userid, dynamic message)
        {
            Clients.User(userid).SendAsync("ReceiveMessage", new { Msg = message, UserAvatar = "", UserName = CurrentUser.UserName, Mine = false });
        }
    }
}
