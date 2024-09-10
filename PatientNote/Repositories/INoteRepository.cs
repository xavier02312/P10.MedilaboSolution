using MongoDB.Bson;
using PatientNote.Domain;

namespace PatientNote.Repositories
{
    public interface INoteRepository
    {
        Task<Note> CreateAsync(Note note);
        Task<Note?> UpdateAsync(Note note);
        Task<Note?> DeleteAsync(ObjectId id);
        Task<Note?> GetByIdAsync(ObjectId id);
        Task<List<Note>> ListAsync(int patientId);
    }
}
