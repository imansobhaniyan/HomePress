using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomePress.Core.Data
{
    public class Photo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? PropertyId { get; set; }
        public string? UserId { get; set; }
        public string? OriginalUrl { get; set; }
        public string? Width256 { get; set; }
        public string? Width512 { get; set; }
        public string? Width1024 { get; set; }
        public string? Width1920 { get; set; }
        public string? Caption { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
