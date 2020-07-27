using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Api.Hubs
{
    /// <summary>
    /// 商户消息 强类型中心
    /// </summary>
    public interface IChatClient
    {
        /// <summary>
        /// 接收消息
        /// <para>客户端监听方法名为ReceiveStoreMessage</para>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ReceiveStoreMessage(MessageDto dto);
    }
}
