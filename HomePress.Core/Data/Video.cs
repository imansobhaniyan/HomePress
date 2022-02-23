using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomePress.Core.Data
{
    public class Video
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? PropertyId { get; set; }
        public string? UserId { get; set; }
        public string? Dimention1080 { get; set; }
        public string? Dimention720 { get; set; }
        public string? Dimention480 { get; set; }
        public double? DurationInSeconds { get; set; }
        public string? Thumbnail { get; set; }
        public string? Caption { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
