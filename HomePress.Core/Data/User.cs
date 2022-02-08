using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace HomePress.Core.Data
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? AvatarUrl { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        
        public Gender Gender { get; set; }
        public UserStatus UserStatus { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public UserTypes UserType { get; set; }

        public List<string>? LanguageIds { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public static string EncryptPassword(string crudePassword)
        {
            return UTF8Encoding.UTF8.GetString(MD5.HashData(UTF8Encoding.UTF8.GetBytes(crudePassword)));
        }
    }
}
