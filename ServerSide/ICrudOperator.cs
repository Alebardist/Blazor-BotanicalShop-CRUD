using MongoDB.Bson;
using System.Collections.Generic;

namespace ServerSide
{
    public interface ICrudOperator
    {
        void Create(Goods type);
        void Delete(ObjectId id);
        List<Goods> Read(string bsonFilter);
        List<Goods> ReadAll(int count = int.MaxValue);
        Goods ReadById(ObjectId id);
        void Update(ObjectId id, string field, string value);
    }
}