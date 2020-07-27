using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Api.Hubs
{
    /// <summary>
    /// 消息
    /// </summary>
    public class MessageDto
    {
        public MessageDto()
        {
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        public string Message { get; set; }

        public int Type { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
