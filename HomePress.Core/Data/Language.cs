using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomePress.Core.Data
{
    public class Language
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? FlagUrl { get; set; }
        public string? EnglishName { get; set; }
        public string? LocalName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
