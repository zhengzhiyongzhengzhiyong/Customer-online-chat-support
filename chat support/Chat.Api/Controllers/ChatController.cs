using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Api.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub, IChatClient> _chatHubContext;
        public ChatController(IHubContext<ChatHub, IChatClient> chatHubContext)
        {
            _chatHubContext = chatHubContext;
        }

        [HttpPost]
        public async Task SendMessage()
        {
            var data = new MessageDto { Title = "您收到一条验证消息", Message = "消息内容", Type = 1 };
            //这里可写获取需要发送分组的名称 
            var groupName = "group_1";
            await _chatHubContext.Clients.Group(groupName).ReceiveStoreMessage(data);

        }
    }
}