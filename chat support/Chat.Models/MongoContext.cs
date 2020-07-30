using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Models
{
    public class MongoContext
    {
        public readonly IMongoDatabase _db;

        public MongoContext(IOptions<MongoOptions> options)
        {
            string connectionstring = string.Empty;
            if (string.IsNullOrEmpty(options.Value.UserName) || string.IsNullOrEmpty(options.Value.Password))
            {
                connectionstring = $"mongodb://{options.Value.Host}:{options.Value.Port}";
            }
            else
            {
                connectionstring = $"mongodb://{options.Value.UserName}:{ options.Value.Password}@{options.Value.Host}:{options.Value.Port}";
            }
            var _mongoClient = new MongoClient(connectionstring);
            _db = _mongoClient.GetDatabase(options.Value.DataBase);
        }
    }
}
