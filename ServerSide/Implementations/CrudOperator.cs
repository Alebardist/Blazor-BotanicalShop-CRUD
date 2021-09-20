using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Extensions.Configuration;

using MongoDB.Bson;
using MongoDB.Driver;

namespace ServerSide
{
    public class CrudOperator : ICrudOperator
    {
        private readonly IMongoDatabase _mongoDatabase;
        private IMongoCollection<Goods> _mongoCollection;
        private IConfiguration _configuration;

        public CrudOperator(IConfiguration configuration)
        {
            _configuration = configuration;
            MongoClient mongoClient = new();

            _mongoDatabase = mongoClient.GetDatabase(configuration.GetSection("MongoDB:dbName").Value);
            _mongoCollection = _mongoDatabase.GetCollection<Goods>(configuration.GetSection("MongoDB:collectionName").Value);

            Debug.WriteLine("//// CRUD ctor OK");
        }

        public async void CreateAsync(Goods type)
        {
            await _mongoCollection.InsertOneAsync(type);
            Debug.WriteLine("create execute");
        }

        public List<Goods> GetGoods(string bsonFilter)
        {
            RefreshCollection();
            Debug.WriteLine("Read executed");

            return _mongoCollection.Find(bsonFilter).ToList();
        }

        public Goods ReadById(ObjectId id)
        {
            RefreshCollection();
            Goods goods = _mongoCollection.Find(x => x.Id == id).FirstOrDefault();
            Debug.WriteLine("ReadById execute");

            return goods ?? new Goods();
        }

        /// <summary>
        /// Gets all documents from collection
        /// </summary>
        /// <param name="count">max documents, by default - 1000 records</param>
        /// <returns></returns>
        public List<Goods> ReadAll(int count = 1000)
        {
            RefreshCollection();

            Debug.WriteLine("ReadAll execute");

            return _mongoCollection.Find(x => true).Limit(count).ToList();
        }

        public async void Update(ObjectId id, string field, string value)
        {
            RefreshCollection();

            var update = Builders<Goods>.Update
                .Set(field, value);

            await _mongoCollection.UpdateOneAsync(x => x.Id == id, update);
            Debug.WriteLine("Update execute");
        }

        public async void Delete(ObjectId id)
        {
            RefreshCollection();
            await _mongoCollection.DeleteOneAsync(x => x.Id == id);
            Debug.WriteLine("Delete executed");
        }

        private void RefreshCollection()
        {
            _mongoCollection = _mongoDatabase.GetCollection<Goods>(_configuration.GetSection("MongoDB:collectionName").Value);
        }
    }
}
