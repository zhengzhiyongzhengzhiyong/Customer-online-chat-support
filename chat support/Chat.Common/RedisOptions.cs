using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Common
{
    public class RedisOptions : IOptions<RedisOptions>
    {
        /// <summary>
        /// The configuration used to connect to Redis.
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// The configuration used to connect to Redis.
        /// This is preferred over Configuration.
        /// </summary>
        public ConfigurationOptions ConfigurationOptions { get; set; }

        /// <summary>
        /// The Redis instance name.
        /// </summary>
        public string InstanceName { get; set; }

        RedisOptions IOptions<RedisOptions>.Value
        {
            get { return this; }
        }
    }
}
