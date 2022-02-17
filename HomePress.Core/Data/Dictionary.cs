using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomePress.Core.Data
{
    public class Dictionary
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Key { get; set; }
        public List<DictionaryTranslate>? Translates { get; set; }

        public class DictionaryTranslate
        {
            public string? LanguageISOCode { get; set; }
            public string? Value { get; set; }
        }
    }
}
