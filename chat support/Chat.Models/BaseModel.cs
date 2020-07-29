using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Models
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public class BaseModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement(nameof(CreateDateTime))]  
        public DateTime CreateDateTime { get; set; }

       [BsonElement(nameof(IsDelete))]
        public bool IsDelete { get; set; }

        public BaseModel()
        {
            CreateDateTime = DateTime.Now;
            IsDelete = false;
        }

    }
}
