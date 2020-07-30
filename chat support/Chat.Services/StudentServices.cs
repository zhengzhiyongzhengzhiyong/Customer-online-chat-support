using Chat.IServices;
using Chat.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Services
{
    public class StudentServices : BaseService<Student>, IStudentServices
    {
        public StudentServices(MongoContext MongoContext) : base(MongoContext)
        {
        }
    }
}
