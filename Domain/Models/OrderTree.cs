using MongoDB.Bson.Serialization.Attributes;

namespace Ipe.Domain.Models
{
    public class OrderTree
    {
        [BsonElement("treeId")]
        public string Id { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }
    }
}
