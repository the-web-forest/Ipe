using MongoDB.Bson.Serialization.Attributes;

namespace Ipe.Domain.Models
{
    [BsonIgnoreExtraElements]
    public class Plant : Model
    {
        [BsonElement("orderId")]
        public string OrderId { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("treeId")]
        public string TreeId { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("message")]
        public string? Message { get; set; }

        [BsonElement("biome")]
        public string Biome { get; set; }

        [BsonElement("species")]
        public string Species { get; set; }

        [BsonElement("image")]
        public string Image { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("value")]
        public double Value { get; set; }

        [BsonElement("hastags")]
        public List<string> Hastags { get; set; }
    }
}
