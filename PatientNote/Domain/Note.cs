using MongoDB.Bson;

namespace PatientNote.Domain
{
    public class Note
    {
        public ObjectId Id { get; set; }
        public int PatientId { get; set; }
        public string Content { get; set; }
    }
}
