using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServerSide
{
    public class Goods
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string GoodsName { get; set; }
        public float Price { get; set; }
        public float PriceSale { get; set; }
        public string Categorie { get; set; }
        public int BuyCount { get; set; }
        public string[] Tags { get; set; }
        public string GoodsImageLocation { get; set; }
    }
}
