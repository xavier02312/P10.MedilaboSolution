using MongoDB.Bson;
using PatientNote.Models.InputModels;
using PatientNote.Models.OutputModels;

namespace PatientNote.Service
{
    public interface INoteService
    {
        Task<NoteOutputModel> Create(NoteInputModel noteModel);
        Task<NoteOutputModel?> GetById(ObjectId id);
        Task<NoteOutputModel?> Update(NoteInputModel noteModel, ObjectId id);
        Task<NoteOutputModel?> Delete(ObjectId id);
        Task<List<NoteOutputModel>> GetNotes(int patientId);
    }
}
