using MongoDB.Bson.Serialization.Attributes;

namespace helium_api.Models.HeliumApi
{
    [BsonIgnoreExtraElements]
    public class BlockTransactions
    {
        public Transaction[] Data { get; set; }
    }
}
