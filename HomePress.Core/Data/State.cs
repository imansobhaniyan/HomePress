using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomePress.Core.Data
{
    public class State
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? CountryId { get; set; }
        public string? Name { get; set; }
    }
}
