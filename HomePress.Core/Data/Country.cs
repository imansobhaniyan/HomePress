using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomePress.Core.Data
{

    public class Country
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? FlagUrl { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? Alpha2Code { get; set; }
        public string? Alpha3Code { get; set; }
        public string? InternetCode { get; set; }

        public int PhoneCode { get; set; }
        public int NumericCode { get; set; }

    }

}
