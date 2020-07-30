using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chat.IServices;
using Chat.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chat.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseModel
    {
        private readonly  IMongoCollection<T> _collection;

        public BaseService(MongoContext MongoContext)
        {
            var database = MongoContext._db;
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public List<T> Get()
        {
            return _collection.Find(T => true).ToList();
        }

        public T Get(string id)
        {
            return _collection.Find<T>(T => T.Id == id).FirstOrDefault();
        }

        public T Create(T T)
        {
            _collection.InsertOne(T);
            return T;
        }
        public void Update(string id, T TIn)
        {
            _collection.ReplaceOne(T => T.Id == id, TIn);
        }

        public void Remove(T TIn)
        {
            _collection.DeleteOne(T => T.Id == TIn.Id);
        }

        public void Remove(string id)
        {
            _collection.DeleteOne(T => T.Id == id);
        }
    }
}
