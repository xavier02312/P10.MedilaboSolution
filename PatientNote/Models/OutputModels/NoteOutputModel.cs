using MongoDB.Bson;

namespace PatientNote.Models.OutputModels
{
    public class NoteOutputModel
    {
        public ObjectId Id { get; set; }
        public string Content { get; set; }
        public int PatientId { get; set; }
    }
}
