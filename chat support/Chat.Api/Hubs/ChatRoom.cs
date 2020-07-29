using Chat.Api.Controllers;
using Chat.Api.TestData;
using Chat.Common;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="Connections"></param>
        public ChatRoom(RedisHelper client, ConnectionList Connections)
        {
            _redis = client.GetUserDB();
            _connections = Connections;
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            _connections.Add(CurrentUser);   
            await Clients.Caller.SendAsync("OnlineCount", _connections.Count);
            await Clients.Others.SendAsync("UserOnline", CurrentUser.UserName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            _connections.Remove(CurrentUser.Id);
            await Clients.Others.SendAsync("UserOffline", CurrentUser.UserName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;
            Clients.Others.SendAsync("Receive", new { Msg = message, UserAvatar = "", UserName = CurrentUser.UserName, Mine = false });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="message"></param>
        public void SendChatMessage(string userid, dynamic message)
        {
            Message returnMsg = new Message();
            returnMsg.author = CurrentUser.UserName;
            returnMsg.id = CurrentUser.Id;
            returnMsg.type = message.type;

            if (message.type == "text")
            {
                returnMsg.data = new 
                {
                    text = message.data.text,
                    meta = DateTime.Now.ToDateTimeString_V()
                };
            } 
            else if (message.type == "emoji")
            {
                returnMsg.data = new
                {
                    code = message.data.code,
                    meta = DateTime.Now.ToDateTimeString_V()
                };
            }
            else if (message.type == "file")
            {
                returnMsg.data = new
                {
                     file= new
                     {
                         name = message.data.file.name,
                         url = message.data.file.url
                     },
                     meta = DateTime.Now.ToDateTimeString_V()
                };
            }

            Clients.User("dbd6f4e8-9867-43c9-ae36-5839c990d6fb").SendAsync("ReceiveMessage", returnMsg).ContinueWith((value)=> {
                if (value.IsFaulted)
                {

                }
            });
        }
    }
}
