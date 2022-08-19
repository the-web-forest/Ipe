using MongoDB.Bson.Serialization.Attributes;

namespace Ipe.Domain.Models
{
    public class Tree : Model
    {
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("image")]
        public string Image { get; set; }
        [BsonElement("value")]
        public double Value { get; set; }
        [BsonElement("biome")]
        public string Biome { get; set; }
        [BsonElement("deleted")]
        public bool Deleted{ get; set; }

        [BsonElement("lowercaseName")]
        public string? LowercaseName { get; set; }
        [BsonElement("lowercaseBiome")]
        public string? LowercaseBiome { get; set; }
        [BsonElement("lowercaseDescription")]
        public string? LowercaseDescription { get; set; }
    }
}
