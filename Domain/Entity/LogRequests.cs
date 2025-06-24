using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entity
{
    public class LogRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string CorrelationId { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
        public long ExecutionTimeMs { get; set; }
        public string ErrorMessage { get; set; }
    }
}