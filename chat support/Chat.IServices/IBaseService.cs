using Chat.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.IServices
{
    public interface IBaseService<T>  where T : BaseModel
    {
        List<T> Get();
        T Get(string id);
        T Create(T T);
        void Update(string id, T TIn);
        void Remove(T TIn);
        void Remove(string id);
    }
}
