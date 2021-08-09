﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ServerSide
{
    public class CrudOperator : ICrudOperator
    {
        public string CollectionName { get; private set; }

        private readonly IMongoDatabase _mongoDatabase;
        private IMongoCollection<Goods> _mongoCollection;

        public CrudOperator(string collectionName = "Botanical", string dbName = "TestDB")
        {
            MongoClient mongoClient = new MongoClient();
            CollectionName = collectionName;

            _mongoDatabase = mongoClient.GetDatabase(dbName);
            _mongoCollection = _mongoDatabase.GetCollection<Goods>(CollectionName);

            Debug.WriteLine("//// CRUD ctor OK");
        }

        public async void Create(Goods type)
        {
            await _mongoCollection.InsertOneAsync(type);
            Debug.WriteLine("create execute");
        }

        public List<Goods> Read(string bsonFilter)
        {
            RefreshCollection();
            Debug.WriteLine("Read execute");

            return _mongoCollection.Find(bsonFilter).ToList();

        }


        //TODO: to optimize thq query, need to get ONLY IDs, 
        //but new BsonDocument("{}", "{roll:1, _id:1}") doesn't work
        /// <summary>
        /// Use to get list of IDs
        /// </summary>
        /// <returns>Generic list of ObjectIds</returns>
        public List<ObjectId> ReadAllIDs()
        {
            RefreshCollection();
            Debug.WriteLine("Read all IDs execute");

            List<Goods> goodsContainer = ReadAll();

            return FillUpIDList(goodsContainer);
        }

        public Goods ReadById(ObjectId id)
        {
            RefreshCollection();
            Goods article = _mongoCollection.Find(x => x.Id == id).FirstOrDefault();
            Debug.WriteLine("ReadById execute");

            return article ?? new Goods();
        }

        /// <summary>
        /// Gets all documents from collection
        /// </summary>
        /// <param name="count">max elements, by default - 1000 records</param>
        /// <returns></returns>
        public List<Goods> ReadAll(int count = 1000)
        {
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
            await _mongoCollection.DeleteOneAsync(x => x.Id == id);
            Debug.WriteLine("Delete execute");

        }

        //TODO: get rid of this by using property: 
        //get=> _mongoDatabase.GetCollection<Goods>(CollectionName);
        private void RefreshCollection()
        {
            _mongoCollection = _mongoDatabase.GetCollection<Goods>(CollectionName);
        }

        private List<ObjectId> FillUpIDList(List<Goods> goods)
        {
            List<ObjectId> IDsContainer = new List<ObjectId>();

            foreach (var item in goods)
            {
                IDsContainer.Add(item.Id);
            }

            return IDsContainer;
        }
    }
}
