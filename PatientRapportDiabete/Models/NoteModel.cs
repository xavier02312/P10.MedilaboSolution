using MongoDB.Bson;

namespace PatientRapportDiabete.Models
{
    public class NoteModel
    {
        public ObjectId Id { get; set; }
        public int PatientId { get; set; }
        public string Content { get; set; }
    }
}
