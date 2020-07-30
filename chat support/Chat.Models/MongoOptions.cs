using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Models
{
    public class MongoOptions
    {
        public string DataBase { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

    }
}
