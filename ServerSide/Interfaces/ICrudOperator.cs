using MongoDB.Bson;
using System.Collections.Generic;

namespace ServerSide
{
    public interface ICrudOperator
    {
        void CreateAsync(Goods type);
        void Delete(ObjectId id);
        List<Goods> GetGoods(string bsonFilter);
        List<Goods> ReadAll(int count = int.MaxValue);
        Goods ReadById(ObjectId id);
        void Update(ObjectId id, string field, string value);
    }
}