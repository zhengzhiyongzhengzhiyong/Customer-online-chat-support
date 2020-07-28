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

    /// <summary>
    /// 
    /// </summary>
    public class Message
    {
        public string author { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        
        public Object data { get; set; }
    }

    public class FileData
    {
        public file file { get; set; }
    }

    public class file
    {
        public string name { get; set; }
        public string url { get; set; }
        public string meta { get; set; }
    }

    public class MessageData
    {
        public string text { get; set; }
        public string meta { get; set; }
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum Msg_type { 
        file,
        text,
        emoji,
        system
    }

}
