using Chat.IServices;
using Chat.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Services
{
    public class ManagerServices : BaseService<Student>, IManagerServices
    {
        public ManagerServices(MongoContext MongoContext) : base(MongoContext)
        {
        }
    }
}
